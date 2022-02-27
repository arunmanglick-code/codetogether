<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProfileCode.aspx.cs" Inherits="ProfileCode" Title="Profile Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script type="text/javascript">
        function ShowNotePad()
        {   
            window.open('<%= Page.ResolveUrl("~/XMLNotePad.aspx")%>', '', 'fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no');
        }        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="XML Serialization Cafe"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
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
                    <div class="DivClassFeature" style="width:550px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>                        
                        <br />
                        <br />
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">
                            <!-- Row 1 -->
                            <tr>
                                <td colspan="5">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Code Profiling"></asp:Label>
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView1" runat="server" OnClick="lnkNotePad1_Click">View Code to Profile</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnSerialize" runat="server" class="button" Width="250px" Text="Non Profiled Code - String" OnClick="btnSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClientClick="ShowNotePad();">View Profiled Code</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnDeSerialize" runat="server" Width="250px" class="button" Text="Profiled Code - String" OnClick="btnDeSerialize_Click" />
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView2" runat="server" OnClick="lnkNotePad2_Click">View Code to Profile</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnNestedSerialize" runat="server" class="button" Width="250px" Text="Non Profiled Code - Brush" OnClick="btnNestedSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad2" runat="server" OnClientClick="ShowNotePad();">View Profiled Code</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnNestedDeSerialize" runat="server" class="button" Width="250px" Text="Profiled Code - Brush" OnClick="btnNestedDeSerialize_Click" />
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView3" runat="server" OnClick="lnkNotePad3_Click">View Code to Profile</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnPurchaseOrderSerialize" runat="server" class="button" Width="250px" Text="Non Profiled Code - Stream" OnClick="btnPurchaseOrderSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad3" runat="server" OnClientClick="ShowNotePad();">View Profiled Code</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                   <asp:Button ID="btnPurchaseOrderDeSerialize" runat="server" class="button" Width="250px" Text="Profiled Code - Stream" OnClick="btnPurchaseOrderDeSerialize_Click" />
                                </td>
                            </tr>                                               
                        </table>                        
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
