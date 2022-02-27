<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SP Snippets.aspx.cs" Inherits="SPSnippets" Title="SP Snippets Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/Browser.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label></td>
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
                            <li>Enter Text</li>
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
                                <td colspan="4">
                                    <asp:Label CssClass="formheader" Width="100%" ID="lblAuditReport" runat="server"
                                        Text="Stored Procedure Scripts"></asp:Label><br /><br />
                                </td>
                            </tr>
                           <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:Label ID="lblScript1" runat="server" Text="Mx Script1"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClick="lnkNotePad1_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript2" runat="server" Text="XML Packet"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad2" runat="server" OnClick="lnkNotePad2_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Convert(datetime, convert(nvarchar..))
                                            <li>DatePart, DateDiff</li>
                                            <li>Execute sp_executesql</li>
                                            <li>Case Statement</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript3" runat="server" Text="Convert Function"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad3" runat="server" OnClick="lnkNotePad3_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Convert Fucntion in SQL</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript4" runat="server" Text="SearchEvent"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad4" runat="server" OnClick="lnkNotePad4_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Union</li>
                                            <li>DatePart, LTrim, RTrim</li>
                                            <li>isnull</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript5" runat="server" Text="ShowEventTickets"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad5" runat="server" OnClick="lnkNotePad5_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Temporary Tables</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="lblScript6" runat="server" Text="ShowEmployeeDetailsBanner"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad6" runat="server" OnClick="lnkNotePad6_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Exists</li>
                                            <li>JOIN</li>
                                            <li>Trim</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="Create Table"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkNotePad7" runat="server" OnClick="lnkNotePad7_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Create Table</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>  
                            <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label2" runat="server" Text="Alter Table"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkNotePad8" runat="server" OnClick="lnkNotePad8_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Alter Column</li>
                                            <li>Rename Column</li>
                                            <li>Add Column</li>
                                            <li>Add Constraint</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Insert Table"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkNotePad9" runat="server" OnClick="lnkNotePad9_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Insert into Table</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>   
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 10%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Create Procedure"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkNotePad10" runat="server" OnClick="lnkNotePad10_Click" >View NotePad</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Follow Syntax</li>
                                        </ol>
                                    </div>
                                </td>
                           </tr>                               
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
