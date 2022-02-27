<%@ WebService Language="C#" Class="AutoCompleteCS" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoCompleteCS  : System.Web.Services.WebService {

    [WebMethod]
    public string[] GetCompletionList(string prefixText, int count, string contextKey) {
        // Deserialize the contextKey
        JavaScriptSerializer json = new JavaScriptSerializer();
        List<int> categories = null;
        if (contextKey != null)
            categories = json.Deserialize<List<int>>(contextKey);
        
        // Get the products whose name matches the characters entered
        OleDbConnection myConnection = new OleDbConnection();
        myConnection.ConnectionString = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;
        myConnection.Open();

        string sql = "SELECT ProductName FROM Products WHERE ProductName LIKE ? ";

        if (categories != null && categories.Count > 0)
        {
            List<string> filterClauses = new List<string>();
            foreach(int categoryId in categories)
                filterClauses.Add(" CategoryID = ? ");

            sql += string.Concat(" AND (", string.Join(" OR ", filterClauses.ToArray()), ") ");
        }
        
        sql += " ORDER BY ProductName";
        
        OleDbCommand myCommand = new OleDbCommand();
        myCommand.CommandText = sql;
        myCommand.Connection = myConnection;
        myCommand.Parameters.AddWithValue("?", prefixText + "%");

        if (categories != null && categories.Count > 0)
        {
            foreach (int categoryId in categories)
                myCommand.Parameters.AddWithValue("?", categoryId);
        }

        OleDbDataReader myReader = myCommand.ExecuteReader();
        List<string> matches = new List<string>();
        int itemsInSet = 0;
        while (myReader.Read() && itemsInSet < count)
        {
            matches.Add(myReader["ProductName"].ToString());
            itemsInSet++;
        }
        myReader.Close();
        myConnection.Close();

        return matches.ToArray();
    }
    
}

