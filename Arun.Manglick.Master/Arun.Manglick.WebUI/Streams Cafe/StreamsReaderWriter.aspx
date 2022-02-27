<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StreamsReaderWriter.aspx.cs" Inherits="TemplatePage" Title="Untitled Page" %>

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
                <asp:Label ID="lblHeader" runat="server" Text="Streams Cafe"></asp:Label></td>
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
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Different approaches for Read-Write using Streams</li>
                            <li>First - StreamReader & StreamWriter</li>
                            <li>Second - FileStream (For Both) </li>
                            <li>Third - Stream (For Both) </li>
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
                                        Text="Streams Reader Writer"></asp:Label>
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnStreamReader" runat="server" class="button" Width="250px" Text="Stream Reader" OnClick="btnStreamReader_Click" />
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnStreamWriter" runat="server" Width="250px" class="button" Text="Stream Writer" OnClick="btnStreamWriter_Click" />
                                </td>
                                <td style="width: 49%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad2" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnFileStreamReader" runat="server" class="button" Width="250px" Text="FileStream Reader" OnClick="btnFileStreamReader_Click" />
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad3" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnFileStreamWriter" runat="server" Width="250px" class="button" Text="FileStream Writer" OnClick="btnFileStreamWriter_Click" />
                                </td>
                                <td style="width: 49%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad4" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnStreamReading" runat="server" class="button" Width="250px" Text="Stream Reading" OnClick="btnStreamReading_Click" />
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad5" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnStreamWriting" runat="server" Width="250px" class="button" Text="Stream Writer" OnClick="btnStreamWriting_Click" />
                                </td>
                                <td style="width: 49%" class="labelcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad6" runat="server" OnClientClick="ShowNotePad();">View NotePad</asp:LinkButton>
                                </td>
                            </tr>
                        </table>                        
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
