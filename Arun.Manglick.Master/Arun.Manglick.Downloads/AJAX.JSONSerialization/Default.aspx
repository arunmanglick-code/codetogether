<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Theme="Theme1" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JSON Serialization Demos</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <h2>
        <a href="http://localhost/Arun.Manglick.AjaxUI/Home.aspx">AJAX UI</a> || JSON Serialization
        Demos</h2>
    <p>
        These demos illustrate how to use the JSON serialization functionality of the ASP.NET
        AJAX framework.
    </p>
    <div class="DivClassFeature">
        <b>Varoius Features Used.</b>
        <ul>
            <li>The demo available includes examples of using both the server- and client-side JSON
                serialization implementations in the ASP.NET AJAX Framework. Specifically, the demo
                pages illustrate using the AutoComplete control from the AJAX Control Toolkit.
            </li>
            <li>The AutoComplete control adds auto-complete support to textboxes in a web page,
                providing a list of possible matches based on the characters the user has already
                entered into the textbox. </li>
            <li>The control calls a script service that you specify, passing it the text the user
                has entered and expecting back a string array of possible matches, which is then
                displays in a list beneath the textbox. </li>
            <li>This script service may optionally include a string contextKey input parameter for
                any additional information needed. </li>
            <li>You need to select one or more categories from a series of checkboxes. The AutoComplete
                then only lists possible product matches based on the selected categories. For this
                to work, we need to pass in the set of selected category IDs into the script service.
                But all we have at our disposal is that single contextKey input parameter.  </li>
                <li>
                The solution
                is to serialize the array of category IDs into a JSON message and pass that message
                as the contextKey input parameter. This involves serializing the array of category
                IDs into a JSON message and then deserializing it from a string back into an array
                of category IDs in the C# or Visual Basic script service code.
                </li>
        </ul>
    </div>
    <br />
    <br />
    <ul>
        <li><a href="JsonSerializationOnServerCS.aspx">JSON Serialization on the Server (C#)</a>
            (<a href="JsonSerializationOnServerVB.aspx">VB Version</a>) - see how to serialize
            data into a JSON string in C# or VB code on the server.</li>
        <li><a href="JsonSerializationOnClient.aspx">JSON Serialization on the Client</a> -
            see how to serialize data into a JSON string using JavaScript and the ASP.NET AJAX
            framework's client libraries.</li>
    </ul>
    </form>
</body>
</html>
