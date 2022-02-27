using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Class1 : JavaScriptConverter
{
    public Class1()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<Type> SupportedTypes
    {
        get { throw new NotImplementedException(); }
    }
}
