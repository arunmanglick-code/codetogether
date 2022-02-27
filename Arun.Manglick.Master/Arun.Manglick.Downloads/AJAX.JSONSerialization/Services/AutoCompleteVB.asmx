<%@ WebService Language="VB" Class="AutoCompleteVB" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class AutoCompleteVB
    Inherits System.Web.Services.WebService
    
    <WebMethod()> _
    Public Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        ' Deserialize the contextKey
        Dim json As New JavaScriptSerializer()
        Dim categories As List(Of String) = Nothing
        If contextKey IsNot Nothing Then
            categories = json.Deserialize(Of List(Of String))(contextKey)
        End If
        
        ' Get the products whose name matches the characters entered
        Dim myConnection As New OleDbConnection()
        myConnection.ConnectionString = ConfigurationManager.ConnectionStrings("NorthwindConnectionString").ConnectionString
        myConnection.Open()

        Dim sql As String = "SELECT ProductName FROM Products WHERE ProductName LIKE ? "

        If categories IsNot Nothing AndAlso categories.Count > 0 Then
            Dim filterClauses As New List(Of String)
            For Each categoryId As Integer In categories
                filterClauses.Add(" CategoryID = ? ")
            Next

            sql &= String.Concat(" AND (", String.Join(" OR ", filterClauses.ToArray()), ") ")
        End If
        
        sql &= " ORDER BY ProductName"
        
        Dim myCommand As New OleDbCommand()
        myCommand.CommandText = sql
        myCommand.Connection = myConnection
        myCommand.Parameters.AddWithValue("?", prefixText + "%")

        If categories IsNot Nothing AndAlso categories.Count > 0 Then
            For Each categoryId As Integer In categories
                myCommand.Parameters.AddWithValue("?", categoryId)
            Next
        End If

        Dim myReader As OleDbDataReader = myCommand.ExecuteReader()
        Dim matches As New List(Of String)
        Dim itemsInSet As Integer = 0
        While myReader.Read() AndAlso itemsInSet < count
            matches.Add(myReader("ProductName").ToString())
            itemsInSet += 1
        End While
        myReader.Close()
        myConnection.Close()

        Return matches.ToArray()
    End Function

End Class
