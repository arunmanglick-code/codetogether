<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Simple2.aspx.cs" Inherits="Simple2" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
<script type="text/javascript">
    
    function SumNumbers() 
    {        
        //var txtNum1 = $get("txtNumber1");
        //var txtNum2 = $get("txtNumber2");
        //var txtResult = $get("txtResult");
        
        var txtNum1 = document.getElementById('<%=txtNumber1.ClientID %>');
        var txtNum2 = document.getElementById('<%=txtNumber2.ClientID %>');
        var txtResult = document.getElementById('<%=txtResult.ClientID %>');
        
        PageMethods.Sum(txtNum1.value,txtNum2.value,OnSumComplete,OnSumError,txtResult); // txtresult is usercontext parameter
        
    }
    
    function OnSumComplete(result,txtResult,methodName)
    {
        // First argument is always "result" if server side code returns void then this value will be null
        // Second argument is usercontext control pass at the time of call
        // Third argument is methodName (server side function name) In this example the methodName will be "Sum"
        
        txtResult.value = result;
        alert('Save Successful');
    }
    
    function OnSumError(error,txtResult,methodName)
    {
        // First argument is always "error" if server side code throws any exception
        // Second argument is usercontext control pass at the time of call
        // Third argument is methodName (server side function name) In this example the methodName will be "Sum"
        
        if(error !== null) 
        {
            alert(error.get_message());
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Page Methods"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:ValidationSummary ID="vlsStipulations" DisplayMode="List" CssClass="validation"
                    runat="server"></asp:ValidationSummary>
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Page Methods</li>
                            <li> <a href='../../HTML/Ajax_Call_using_AjaxNet.aspx.htm'>Link</a></li>
                            <li>Note - Full Method Signatures have been used</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="4">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Page Methods - Save Action"></asp:Label>
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblChange" runat="server" Text="Number 1"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:TextBox ID="txtNumber1" runat="server" Text="5"></asp:TextBox>
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="Number 2"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:TextBox ID="txtNumber2" runat="server" Text="5"></asp:TextBox>
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="Label2" runat="server" Text="Result"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                            </tr>                           
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 5%" class="labelcolumn">
                                    <asp:Button ID="btnSave" runat="server" Width="150px" class="button" Text="Sum"
                                        OnClientClick="SumNumbers(); return false;" />
                                </td>
                                <td style="width: 92%" class="requiredcolumn" colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    
</asp:Content>
