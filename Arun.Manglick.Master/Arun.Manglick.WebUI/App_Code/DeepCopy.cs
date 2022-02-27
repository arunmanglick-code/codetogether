using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arun.Manglick.UI
{
    /// <summary>
    /// Summary description for DeepCopy
    /// </summary>
    [Serializable]
    public class DeepCopy
    {
        public static string CompanyName = "My Company";
        public int Age;
        public string EmployeeName;
        public CopyObject Salary;

        // Note: The classes to be cloned (Deep Copy) must be flagged as [Serializable]. Therefore the CopyObject must be declared as 'Serializable'

        /// <summary>
        /// Generic approach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T MakeDeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        /// <summary>
        /// Non-Generic Approach
        /// </summary>
        /// <param name="inputcls"></param>
        /// <returns></returns>
        public DeepCopy MakeDeepCopy(DeepCopy inputcls)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, inputcls);
            m.Position = 0;
            return (DeepCopy)b.Deserialize(m);
        }

    }
}
