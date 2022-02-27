Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Partial Class JsonSerializationOnServerVB
    Inherits System.Web.UI.Page

    Protected Sub cblCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cblCategories.SelectedIndexChanged
        Dim selectedCategories As New List(Of Integer)
        For Each li As ListItem In cblCategories.Items
            If li.Selected Then
                selectedCategories.Add(Convert.ToInt32(li.Value))
            End If
        Next

        Dim json As New JavaScriptSerializer()
        autoComplete1.ContextKey = json.Serialize(selectedCategories)
    End Sub
End Class
