from crewai import Agent, Task, Crew
from SOAPtoJSONConverter import SOAPtoJSONConverter

class SOAPtoJSONAgent(Agent):
    def __init__(self, config_path):
        super().__init__(
            role="SOAP to REST Converter",
            goal="Convert SOAP XML payloads to RESTful JSON using a mapping configuration.",
            backstory="This agent automates the transformation of legacy SOAP XML payloads into modern RESTful JSON format for easier integration.",
            name="SOAPtoJSONAgent"
        )
        self.converter = SOAPtoJSONConverter(config_path)

    def run(self, soap_xml_path):
        with open(soap_xml_path, "r", encoding="utf-8") as f:
            soap_xml = f.read()
        try:
            json_output = self.converter.convert(soap_xml)
            return json_output
        except Exception as e:
            return f"Error during conversion: {e}"

if __name__ == "__main__":
    # Instantiate the agent with the mapping config
    agent = SOAPtoJSONAgent("mapping_config.yaml")
    # Define a task for the agent
    task = Task(
        agent=agent,
        description="Convert SOAP XML payload to RESTful JSON using mapping_config.yaml",
        input={"soap_xml_path": "soapinputpayload.xml"}
    )
    # Create a crew and kickoff the task
    crew = Crew(tasks=[task])
    results = crew.kickoff()
    print("AI Agent Conversion Result:")
    print(results[0]['output'])
