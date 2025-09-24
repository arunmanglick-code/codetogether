import xml.etree.ElementTree as ET
import json
import yaml
import re # For regex to strip namespaces more robustly

class SOAPtoJSONConverter:
    def __init__(self, config_filepath: str):
        """
        Initializes the converter with a mapping configuration file.
        :param config_filepath: Path to the YAML configuration file.
        """
        self.config = self._load_config(config_filepath)
        self.global_settings = self.config.get('global_settings', {})
        
        self.strip_namespaces = self.global_settings.get('strip_namespaces', True)
        self.attribute_prefix = self.global_settings.get('attribute_prefix', '@')
        self.text_content_key = self.global_settings.get('text_content_key', '#text')
        self.handle_multiple_as_list = self.global_settings.get('handle_multiple_as_list', True)

        self.json_root_element_name = self.config.get('json_root_element')
        self.field_mappings = self.config.get('fields', [])
        self.exclude_paths = self.config.get('exclude_paths', [])

    def _load_config(self, filepath: str) -> dict:
        """Loads and validates the YAML configuration file."""
        try:
            with open(filepath, 'r') as f:
                config = yaml.safe_load(f)
            # Basic validation
            if not isinstance(config, dict):
                raise ValueError("Config file must be a dictionary.")
            return config
        except FileNotFoundError:
            raise FileNotFoundError(f"Configuration file not found at {filepath}")
        except yaml.YAMLError as e:
            raise ValueError(f"Error parsing YAML config file: {e}")
        except Exception as e:
            raise RuntimeError(f"An unexpected error occurred loading config: {e}")

    def _strip_namespace(self, tag):
        """Removes namespace URI from an XML tag."""
        if self.strip_namespaces:
            match = re.match(r'(\{.*?\})?(.*)', tag)
            return match.group(2) if match else tag
        return tag

    def _apply_type_conversion(self, value, target_type):
        """Converts a string value to a target type."""
        if value is None:
            return None
        try:
            if target_type == "integer":
                return int(value)
            elif target_type == "float":
                return float(value)
            elif target_type == "boolean":
                return str(value).lower() in ('true', '1', 't', 'y', 'yes')
            # Add more types as needed
            return value # Default to string if type is unknown or 'string'
        except ValueError:
            print(f"Warning: Could not convert '{value}' to type '{target_type}'. Keeping as string.")
            return value

    def _find_mapping_for_path(self, current_soap_path: str, mappings: list):
        """Finds the most specific mapping for a given SOAP path."""
        best_match = None
        for mapping in mappings:
            # We need to handle relative paths carefully here.
            # For simplicity, this assumes `soap_path` in config is a direct match
            # relative to the current XML traversal point.
            # A more robust solution might use actual XPath evaluation.
            
            # For now, let's treat current_soap_path as the simple tag name
            # and mapping['soap_path'] as the simple tag name or attribute name
            
            # This logic needs refinement for truly complex paths like "Parent/Child"
            # For this example, let's assume 'soap_path' in config maps directly to 
            # the current element's stripped tag name OR "@attribute_name" OR "#text"
            
            # If the mapping refers to an attribute:
            if mapping.get('soap_path', '').startswith('@'):
                if current_soap_path == mapping['soap_path']:
                    return mapping
            # If the mapping refers to text content:
            elif mapping.get('soap_path') == self.text_content_key:
                if current_soap_path == self.text_content_key:
                    return mapping
            # If the mapping refers to an element:
            elif self._strip_namespace(current_soap_path) == mapping.get('soap_path'):
                return mapping
        return None

    def _process_element_with_config(self, element: ET.Element, current_json_path: list = None, parent_mapping: dict = None):
        """
        Recursively processes an XML element and applies mapping configurations.
        :param element: The current XML element to process.
        :param current_json_path: A list representing the current path in the JSON output for exclusion checks.
        :param parent_mapping: The mapping dict that applies to the parent of the current element.
        """
        if current_json_path is None:
            current_json_path = []

        stripped_tag = self._strip_namespace(element.tag)

        # Check for exclusion
        current_full_soap_path = "/".join(current_json_path + [stripped_tag])
        if any(current_full_soap_path.startswith(ex_path) for ex_path in self.exclude_paths):
             return None # Exclude this element and its children

        element_mapping = None
        if parent_mapping and 'children' in parent_mapping:
            # Look for mapping specific to this child element
            element_mapping = self._find_mapping_for_path(stripped_tag, parent_mapping['children'])
        if not element_mapping: # If no specific child mapping, check global fields if this is a top-level element
            if not current_json_path or parent_mapping is None: # Only check top-level fields for elements not nested under a specific field mapping
                 element_mapping = self._find_mapping_for_path(stripped_tag, self.field_mappings)

        json_field_name = element_mapping.get('json_name') if element_mapping else stripped_tag
        new_json_path = current_json_path + [json_field_name]

        result = {}

        # Handle attributes
        attribute_mappings = element_mapping.get('attributes', []) if element_mapping else []
        for attr_name, attr_value in element.attrib.items():
            stripped_attr_name = self._strip_namespace(attr_name)
            attr_full_path_for_exclusion = "/".join(new_json_path + [self.attribute_prefix + stripped_attr_name])
            if any(attr_full_path_for_exclusion.startswith(ex_path) for ex_path in self.exclude_paths):
                continue # Exclude this attribute

            mapped_attr = self._find_mapping_for_path(stripped_attr_name, attribute_mappings)
            json_attr_name = mapped_attr.get('json_name') if mapped_attr else self.attribute_prefix + stripped_attr_name
            
            attr_val = self._apply_type_conversion(attr_value, mapped_attr.get('type')) if mapped_attr else attr_value
            result[json_attr_name] = attr_val

        # Handle children elements
        children_elements = list(element)
        if children_elements:
            for child in children_elements:
                processed_child = self._process_element_with_config(child, new_json_path, element_mapping)
                if processed_child is None: # Child was explicitly excluded
                    continue

                child_stripped_tag = self._strip_namespace(child.tag)
                child_mapping = self._find_mapping_for_path(child_stripped_tag, element_mapping.get('children', [])) if element_mapping else None
                
                child_json_name = child_mapping.get('json_name') if child_mapping else child_stripped_tag

                is_list_config = child_mapping.get('is_list', False) if child_mapping else False
                
                if is_list_config or (self.handle_multiple_as_list and child_json_name in result and not isinstance(result[child_json_name], list)):
                    if child_json_name not in result:
                         result[child_json_name] = []
                    elif not isinstance(result[child_json_name], list):
                        result[child_json_name] = [result[child_json_name]] # Convert existing single item to list
                    result[child_json_name].append(processed_child)
                elif child_json_name in result and not isinstance(result[child_json_name], list):
                    # If handle_multiple_as_list is false and no is_list config,
                    # and a duplicate tag exists, we'll overwrite (last one wins)
                    # or you could choose to error out. For now, last one wins.
                    result[child_json_name] = processed_child
                else:
                    result[child_json_name] = processed_child
        else:
            # Handle text content if no children
            text_content = element.text.strip() if element.text else ""
            if text_content:
                text_mapping = None
                if element_mapping and 'children' in element_mapping: # Check if text content is explicitly mapped
                    text_mapping = self._find_mapping_for_path(self.text_content_key, element_mapping['children'])
                
                if text_mapping:
                    json_text_name = text_mapping.get('json_name', self.text_content_key)
                    text_val = self._apply_type_conversion(text_content, text_mapping.get('type'))
                    result[json_text_name] = text_val
                elif result: # Element has attributes, store text under specific key
                    result[self.text_content_key] = text_content
                else: # Element has only text, its value is the text
                    # Apply type conversion if specified for the element itself
                    if element_mapping and 'type' in element_mapping:
                        result = self._apply_type_conversion(text_content, element_mapping['type'])
                    else:
                        result = text_content
            else:
                # If element is empty and has no attributes, map to null or empty string
                if not result and element_mapping and 'default' in element_mapping:
                    result = element_mapping['default']
                elif not result: # If no attributes and no text, it's an empty object/string
                    result = "" # Or {} depending on desired empty representation


        # Apply default if result is empty and default is specified for the element
        if element_mapping and 'default' in element_mapping and (result == "" or result == {} or result is None):
            result = element_mapping['default']
        
        # Check required field
        if element_mapping and element_mapping.get('required', False) and (result == "" or result == {} or result is None):
            raise ValueError(f"Required field '{json_field_name}' (SOAP path: '{'/'.join(current_json_path + [stripped_tag])}') is missing or empty.")

        return result

    def convert(self, soap_payload: str) -> str:
        """
        Converts a SOAP XML payload string to a JSON string using the configured mappings.
        :param soap_payload: The input SOAP XML string.
        :return: The resulting JSON string.
        """
        try:
            root = ET.fromstring(soap_payload)

            body = root.find('{http://schemas.xmlsoap.org/soap/envelope/}Body')
            if body is None:
                body = root.find('{http://www.w3.org/2003/05/soap-envelope}Body')
                if body is None:
                    raise ValueError("SOAP Body element not found in the payload.")

            if not list(body):
                return json.dumps({})

            # Find the main content element based on configured json_root_element
            main_content_element = None
            if self.json_root_element_name:
                for child in body:
                    if self._strip_namespace(child.tag) == self.json_root_element_name:
                        main_content_element = child
                        break
                if main_content_element is None:
                    raise ValueError(f"Configured JSON root element '{self.json_root_element_name}' not found in SOAP Body.")
            else:
                main_content_element = list(body)[0] # Default to first child if not specified

            # Check if the main_content_element itself is excluded
            if self._strip_namespace(main_content_element.tag) in self.exclude_paths:
                return json.dumps({}) # If the root itself is excluded, return empty JSON

            json_output = self._process_element_with_config(main_content_element, current_json_path=[])
            
            # If the root element itself has a rename mapping in fields, apply it
            root_mapping = self._find_mapping_for_path(self._strip_namespace(main_content_element.tag), self.field_mappings)
            final_root_name = root_mapping.get('json_name') if root_mapping else self._strip_namespace(main_content_element.tag)
            
            return json.dumps({final_root_name: json_output}, indent=4)

        except ET.ParseError as e:
            raise ValueError(f"Invalid XML payload: {e}")
        except Exception as e:
            raise RuntimeError(f"An unexpected error occurred during conversion: {e}")

# --- Usage Example ---
if __name__ == "__main__":
    # Read SOAP XML input from a file instead of a string
    soap_input_file = "soapinputpayload.xml"
    with open(soap_input_file, "r", encoding="utf-8") as f:
        soap_input = f.read()

    print("--- Using Configured Converter ---")
    try:
        converter = SOAPtoJSONConverter("mapping_config.yaml")
        json_output = converter.convert(soap_input)
        print("Generated JSON:")
        print(json_output)
    except (ValueError, RuntimeError, FileNotFoundError) as e:
        print(f"Error: {e}")

    # print("\n--- Testing with missing required field ---")
    # soap_missing_required = """
    # <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
    #                   xmlns:web="http://www.example.com/webservice">
    #    <soapenv:Body>
    #       <web:GetUserDetailsRequest>
    #          <!-- <web:UserID>12345</web:UserID> UserID is required -->
    #          <web:UserName>Jane Doe</web:UserName>
    #          <web:EmailAddresses>
    #             <web:Email type="primary">jane.doe@example.com</web:Email>
    #          </web:EmailAddresses>
    #       </web:GetUserDetailsRequest>
    #    </soapenv:Body>
    # </soapenv:Envelope>
    # """
    # try:
    #     converter_missing = SOAPtoJSONConverter("mapping_config.yaml")
    #     converter_missing.convert(soap_missing_required)
    # except (ValueError, RuntimeError) as e:
    #     print(f"Caught expected error for missing required field: {e}")

    # print("\n--- Testing with a SOAP payload for product details (no specific config for it yet) ---")
    # soap_product_details = """
    # <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
    #   <soap:Body>
    #     <GetProductDetailsResponse xmlns="http://tempuri.org/products">
    #       <Product id="P101">
    #         <Name>Laptop Pro</Name>
    #         <Price currency="USD">1200.00</Price>
    #         <Features>
    #           <Feature>8GB RAM</Feature>
    #           <Feature>256GB SSD</Feature>
    #         </Features>
    #       </Product>
    #     </GetProductDetailsResponse>
    #   </soap:Body>
    # </soap:Envelope>
    # """
    # # For a completely different SOAP structure, you'd provide a different mapping_config.yaml
    # try:
    #     # Re-using the same converter, so it will still look for 'GetUserDetailsRequest' as root
    #     # This will fail because the root element won't match.
    #     # This demonstrates why a config is specific to a SOAP structure.
    #     converter.convert(soap_product_details)
    # except (ValueError, RuntimeError) as e:
    #     print(f"Caught expected error for incorrect root element due to config: {e}")

    # print("\n--- Testing with empty email address (required child) ---")
    # soap_empty_email = """
    # <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
    #                   xmlns:web="http://www.example.com/webservice">
    #    <soapenv:Body>
    #       <web:GetUserDetailsRequest>
    #          <web:UserID>12345</web:UserID>
    #          <web:UserName>Email Tester</web:UserName>
    #          <web:EmailAddresses>
    #             <web:Email type="primary"></web:Email> <!-- Empty primary email -->
    #          </web:EmailAddresses>
    #       </web:GetUserDetailsRequest>
    #    </soapenv:Body>
    # </soapenv:Envelope>
    # """
    # try:
    #     converter.convert(soap_empty_email)
    # except (ValueError, RuntimeError) as e:
    #     print(f"Caught expected error for empty required email address: {e}")