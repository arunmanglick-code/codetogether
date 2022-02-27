using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using Arun.Manglick.BL;
using Arun.Manglick.UI;

public partial class XmlSerialization : BasePage
{
    #region Private Variables

    private string pathName1 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"TempFolder\\FileName.xml";
    private string pathName2 = AppDomain.CurrentDomain.BaseDirectory.ToString() + "../" + @"JS\\Calendar.js";
    private string pathName3 = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"XML\\AuditXML.xml";

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowHideError(String.Empty, false);
    }

    #endregion

    #region Private Methods

    private void Serialize()
    {
        SerializeClass myObject = new Arun.Manglick.BL.SerializeClass();
        myObject.FirstName = "Arun";
        myObject.LastName = "Manglick";
        myObject.Age = 30;

        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializeClass));
        StreamWriter myWriter = new StreamWriter(pathName1);
        mySerializer.Serialize(myWriter, myObject);
        myWriter.Close();

        base.ReadTextStream(pathName1);
    }
    private void DeSerialize()
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializeClass));
        StreamReader myReader = new StreamReader(pathName1);
        SerializeClass myObject = mySerializer.Deserialize(myReader) as SerializeClass;
        myReader.Close();
    }

    private void SerializeNested()
    {
        Address address = new Address();
        address.Address1 = "Address 1";
        address.Address2 = "Address 2";
        address.City = "Pune";
        address.Country = "India";

        SerializeNestedClass myObject = new Arun.Manglick.BL.SerializeNestedClass();
        myObject.FirstName = "Arun";
        myObject.LastName = "Manglick";
        myObject.Age = 30;
        myObject.Address = address;

        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializeNestedClass));
        StreamWriter myWriter = new StreamWriter(pathName1);
        mySerializer.Serialize(myWriter, myObject);
        myWriter.Close();

        base.ReadTextStream(pathName1);
    }
    private void DeSerializeNested()
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializeNestedClass));
        StreamReader myReader = new StreamReader(pathName1);
        SerializeNestedClass myObject = mySerializer.Deserialize(myReader) as SerializeNestedClass;
        myReader.Close();
    }

    private void SerializePurchaseOrder()
    {
        // --------------------------------------------------------
        AddressTo billAddress = new AddressTo();
        billAddress.Name = "Teresa Atkinson";
        billAddress.Line1 = "1 Main St.";
        billAddress.City = "AnyTown";
        billAddress.State = "WA";
        billAddress.Zip = "00000";

        OrderedItem i1 = new OrderedItem();
        i1.ItemName = "Widget S";
        i1.Description = "Small widget";
        i1.UnitPrice = (decimal)5.23;
        i1.Quantity = 3;
        i1.Calculate();

        OrderedItem i2 = new OrderedItem();
        i2.ItemName = "Widget S";
        i2.Description = "Small widget";
        i2.UnitPrice = (decimal)5.23;
        i2.Quantity = 3;
        i2.Calculate();
        OrderedItem[] items = { i1,i2 };

        SerializePurchaseOrderClass po = new SerializePurchaseOrderClass();        
        po.ShipTo = billAddress;
        po.OrderDate = System.DateTime.Now.ToLongDateString();
        po.OrderedItems = items;
        decimal subTotal = new decimal();
        foreach (OrderedItem oi in items)
        {
            subTotal += oi.LineTotal;
        }
        po.SubTotal = subTotal;
        po.ShipCost = (decimal)12.51;
        po.TotalCost = po.SubTotal + po.ShipCost;
        // --------------------------------------------------------

        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializePurchaseOrderClass));
        StreamWriter myWriter = new StreamWriter(pathName1);
        mySerializer.Serialize(myWriter, po);
        myWriter.Close();

        base.ReadTextStream(pathName1);
    }
    private void DeSerializePurchaseOrder()
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializePurchaseOrderClass));
        StreamReader myReader = new StreamReader(pathName1);
        SerializePurchaseOrderClass myObject = mySerializer.Deserialize(myReader) as SerializePurchaseOrderClass;
        myReader.Close();
    }

    private void SerializeXMLAttribuedPurchaseOrder()
    {
        // --------------------------------------------------------
        AddressToXMLAtrribued billAddress = new AddressToXMLAtrribued();
        billAddress.Name = "Teresa Atkinson";
        billAddress.Line1 = "1 Main St.";
        billAddress.City = "AnyTown";
        billAddress.State = "WA";
        billAddress.Zip = "00000";

        OrderedItemXMLAtrribued i1 = new OrderedItemXMLAtrribued();
        i1.ItemName = "Widget S";
        i1.Description = "Small widget";
        i1.UnitPrice = (decimal)5.23;
        i1.Quantity = 3;
        i1.Calculate();

        OrderedItemXMLAtrribued i2 = new OrderedItemXMLAtrribued();
        i2.ItemName = "Widget S";
        i2.Description = "Small widget";
        i2.UnitPrice = (decimal)5.23;
        i2.Quantity = 3;
        i2.Calculate();
        OrderedItemXMLAtrribued[] items = { i1, i2 };

        SerializePurchaseOrderClassXMLAtrribued po = new SerializePurchaseOrderClassXMLAtrribued();
        po.ShipTo = billAddress;
        po.OrderDate = System.DateTime.Now.ToLongDateString();
        po.OrderedItems = items;
        decimal subTotal = new decimal();
        foreach (OrderedItemXMLAtrribued oi in items)
        {
            subTotal += oi.LineTotal;
        }
        po.SubTotal = subTotal;
        po.ShipCost = (decimal)12.51;
        po.TotalCost = po.SubTotal + po.ShipCost;
        // --------------------------------------------------------

        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializePurchaseOrderClassXMLAtrribued));
        StreamWriter myWriter = new StreamWriter(pathName1);
        mySerializer.Serialize(myWriter, po);
        myWriter.Close();

        base.ReadTextStream(pathName1);
    }
    private void DeSerializeXMLAttribuedPurchaseOrder()
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(SerializePurchaseOrderClassXMLAtrribued));
        StreamReader myReader = new StreamReader(pathName1);
        SerializePurchaseOrderClassXMLAtrribued myObject = mySerializer.Deserialize(myReader) as SerializePurchaseOrderClassXMLAtrribued;
        myReader.Close();
    }

    private string GetClassFilePath(string fileName)
    {
        string pathName = AppDomain.CurrentDomain.BaseDirectory.ToString();
        pathName = pathName.Substring(0, pathName.LastIndexOf("Arun.Manglick"));
        pathName = pathName + "//Arun.Manglick.BL" + "//" + fileName;
        return pathName;
    }
    private void ShowHideError(string errorText, bool show)
    {
        lblError.Text = errorText;
        lblError.Visible = show;
    }

    #endregion
    
    #region Control Events
    
    protected void btnSerialize_Click(object sender, EventArgs e)
    {
        try
        {
            Serialize();
            ShowHideError("Success", true);
        }
        catch (Exception ex)
        {            
            throw;
        }
    }
    protected void btnDeSerialize_Click(object sender, EventArgs e)
    {
        try
        {
            DeSerialize();
            ShowHideError("Successful DeSerialization", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnNestedSerialize_Click(object sender, EventArgs e)
    {
        SerializeNested();
        ShowHideError("Success", true);
    }
    protected void btnNestedDeSerialize_Click(object sender, EventArgs e)
    {
        DeSerializeNested();
        ShowHideError("Successful DeSerialization", true);
    }

    protected void btnPurchaseOrderSerialize_Click(object sender, EventArgs e)
    {
        try
        {
            SerializePurchaseOrder();
            ShowHideError("Success", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnPurchaseOrderDeSerialize_Click(object sender, EventArgs e)
    {
        DeSerializePurchaseOrder();
        ShowHideError("Successful DeSerialization", true);
    }

    protected void btnPurchaseOrderSerializeXMLAttribued_Click(object sender, EventArgs e)
    {
        try
        {
            SerializeXMLAttribuedPurchaseOrder();
            ShowHideError("Success", true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    protected void btnPurchaseOrderDeSerializeXMLAttribued_Click(object sender, EventArgs e)
    {
        DeSerializeXMLAttribuedPurchaseOrder();
        ShowHideError("Successful DeSerialization", true);
    }

    protected void lnkNotePad1_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(GetClassFilePath("SerializeClass.cs"));
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
        
    }
    protected void lnkNotePad2_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(GetClassFilePath("SerializeNestedClass.cs"));
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }
    protected void lnkNotePad3_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(GetClassFilePath("SerializePurchaseOrderClass.cs"));
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }
    protected void lnkNotePad4_Click(object sender, EventArgs e)
    {
        base.ReadTextStream(GetClassFilePath("SerializePurchaseOrderClassXMLAtrribued.cs"));
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }
    protected void lnkNotePad_Click(object sender, EventArgs e)
    {
        Response.Redirect(Page.ResolveUrl("~/NotePad.aspx"));
    }

    #endregion
}
