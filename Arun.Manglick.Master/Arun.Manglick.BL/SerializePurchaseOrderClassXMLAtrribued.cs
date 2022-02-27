using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Arun.Manglick.BL
{
    // The XmlRootAttribute allows you to set an alternate name (ArunManglickPO) for the XML element and its namespace. 
    // By default, the XmlSerializer uses the class name. 
    // The attribute also allows you to set the XML namespace for the element. 
    // Lastly, the attribute sets the IsNullable property, which specifies whether the xsi:null attribute appears if the class instance is set to a null reference.

    [XmlRootAttribute("ArunManglickPO", Namespace = "http://www.Arun.Manglick.com", IsNullable = false)]
    public class SerializePurchaseOrderClassXMLAtrribued
    {
        public AddressToXMLAtrribued ShipTo;
        public string OrderDate;

        // The XmlArrayAttribute changes the XML element name from the default of "OrderedItems" to "Items"
        [XmlArrayAttribute("Items")]
        public OrderedItemXMLAtrribued[] OrderedItems;

        public decimal SubTotal;
        public decimal ShipCost;
        public decimal TotalCost;   
    }

    public class AddressToXMLAtrribued
    {
        // The XmlAttribute instructs the XmlSerializer to serialize the Name field as an XML attribute instead of an XML element (the default behavior).
        [XmlAttribute]
        public string Name;

        public string Line1;

        // Setting the IsNullable property to false instructs the XmlSerializer that the XML attribute will not appear if the City field is set to a null reference.
        [XmlElementAttribute(IsNullable = false)]
        public string City;

        public string State;
        public string Zip;
    }

    public class OrderedItemXMLAtrribued
    {
        public string ItemName;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public decimal LineTotal;

        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }
}
