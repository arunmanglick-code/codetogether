<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="XmlSerialization.aspx.cs" Inherits="XmlSerialization" Title="Untitled Page" %>

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
                            <li>Here you can see how the XML Serilaization works using below process</li>
                            <li>View the Object before Serialization</li>
                            <li>Serialize the Object</li>
                            <li>View the Serialized Object (Generated XML File) using the 'View NotePad'/'View XML NotePad' links</li>
                            <li>De-Serialze the object using De-Serialize button</li>
                            <li>To see the De-Serialization, u'll be required to Debug the code</li>
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
                                        Text="XML Serialization"></asp:Label>
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView1" runat="server" OnClick="lnkNotePad1_Click">View Object to Serialize</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnSerialize" runat="server" class="button" Width="250px" Text="Serialize" OnClick="btnSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClientClick="ShowNotePad();">View XML NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnDeSerialize" runat="server" Width="250px" class="button" Text="De-Serialize" OnClick="btnDeSerialize_Click" />
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView2" runat="server" OnClick="lnkNotePad2_Click">View Object to Serialize</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnNestedSerialize" runat="server" class="button" Width="250px" Text="Serialize Nested" OnClick="btnNestedSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad2" runat="server" OnClientClick="ShowNotePad();">View XML NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnNestedDeSerialize" runat="server" class="button" Width="250px" Text="Nested De-Serialize" OnClick="btnNestedDeSerialize_Click" />
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView3" runat="server" OnClick="lnkNotePad3_Click">View Object to Serialize</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnPurchaseOrderSerialize" runat="server" class="button" Width="250px" Text="Serialize Purchase-Order" OnClick="btnPurchaseOrderSerialize_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad3" runat="server" OnClientClick="ShowNotePad();">View XML NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                   <asp:Button ID="btnPurchaseOrderDeSerialize" runat="server" class="button" Width="250px" Text="Purchase-Order De-Serialize" OnClick="btnPurchaseOrderDeSerialize_Click" />
                                </td>
                            </tr>
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView4" runat="server" OnClick="lnkNotePad4_Click">View Object to Serialize</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnPurchaseOrderSerializeXMLAttribued" runat="server" class="button" Width="250px" Text="XML Attribued Serialize Purchase-Order" OnClick="btnPurchaseOrderSerializeXMLAttribued_Click" />
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad4" runat="server" OnClientClick="ShowNotePad();">View XML NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 49%" class="inputcolumn">
                                   <asp:Button ID="btnPurchaseOrderDeSerializeXMLAttribued" runat="server" class="button" Width="250px" Text="XML Attribued Purchase-Order De-Serialize" OnClick="btnPurchaseOrderDeSerializeXMLAttribued_Click" />
                                </td>
                            </tr>                                                  
                        </table>                        
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
