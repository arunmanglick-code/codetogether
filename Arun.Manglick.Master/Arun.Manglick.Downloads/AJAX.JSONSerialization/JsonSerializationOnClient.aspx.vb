
Partial Class JsonSerializationOnClient
    Inherits System.Web.UI.Page

    Protected Function CreateCheckBox(ByVal categoryId As Integer, ByVal categoryName As String) As String
        Return String.Format("<input type=""checkbox"" value=""{0}"" id=""Category{0}"" onclick=""SetContextKey(this)""><label for=""Category{0}"">{1}</label>", categoryId, categoryName)
    End Function

End Class
