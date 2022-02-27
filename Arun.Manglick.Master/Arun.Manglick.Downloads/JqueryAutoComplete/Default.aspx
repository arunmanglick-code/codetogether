<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JqueryAutoComplete._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/jquery-1.3.2.js" type="text/javascript"></script>

    <script src="Scripts/jquery.autocomplete.customized.Try.js" type="text/javascript"></script>

    <link href="App_Themes/Default/jquery.autocomplete.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function findValue(li) {
            if (li == null) return alert("No match!");

            // if coming from an AJAX call, let's use the CityId as the value
            if (!!li.extra) var sValue = li.extra[0];

            // otherwise, let's just display the value in the text box
            else var sValue = li.selectValue;

            alert("The value you selected was: " + sValue);
        }

        function selectItem(li) {
            findValue(li);
        }

        function formatItem(row) {
            return row[0];
        }

        function lookupAjax() {
            var oSuggest = $("#CityAjax")[0].autocompleter;

            oSuggest.findValue();

            return false;
        }



        function lookupLocal() {
            var oSuggest = $("#CityLocal")[0].autocompleter;

            oSuggest.findValue();

            return false;
        }

        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(pageLoaded);

        function pageLoaded(sender, args) {

            $(document).ready(function() {

                $("#CityAjaxGet").autocomplete(
		            "CityList.aspx",
		                {
		                    delay: 10,
		                    minChars: 1,
		                    matchSubset: 1,
		                    matchContains: 1,
		                    cacheLength: 1000,
		                    onItemSelect: selectItem,
		                    onFindValue: findValue,
		                    formatItem: formatItem,
		                    autoFill: true,
		                    lineSeparator: ',',
		                    ajaxMethod: 'Get',
		                    loadOnce: true,
		                    extraParams: { page: "1", author: "newAuthor" },
		                    maxItemsToShow: 1000

		                }
	            );
            });

        }

        if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
    </script>
    
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <p>
                    <asp:Button ID="Button1" runat="server" Text="Button" /></p>
                <p>
                    Ajax(POST - WebService) City Autocomplete:
                    <input type="text" id="CityAjax" value="" style="width: 200px;" />
                </p>
                <p>
                    Ajax(GET - ASPX) City Autocomplete:
                    <input type="text" id="CityAjaxGet" value="" runat="server" style="width: 200px;" />
                </p>
                <p>
                    Local City Autocomplete:
                    <input type="text" id="CityLocal" value="" />
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
