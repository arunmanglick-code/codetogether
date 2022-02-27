<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SequenceDigarams.aspx.cs" Inherits="SequenceDigarams" Title="Sequence Digarams Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/Browser.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Sequence Digarams"></asp:Label></td>
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
                                        Text="Sequence Digarams"></asp:Label><br /><br />
                                </td>
                            </tr>
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label12" runat="server" Text="General.FragmentOperators"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton12" runat="server" OnClick="lnkNotePad0_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Reference: <a href='http://www.websequencediagrams.com/examples.html'> Link</a></li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="lblScript1" runat="server" Text="DigitalDownload.Preview1"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="lnkXMLNotePad1" runat="server" OnClick="lnkNotePad1_Click" >View Diagram</asp:LinkButton>
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
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label1" runat="server" Text="DigitalDownload.Preview2"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkNotePad2_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label2" runat="server" Text="DigitalDownload.Preview3"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="lnkNotePad3_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                            <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label3" runat="server" Text="Tyrelink.CreateOrder"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton3" runat="server" OnClick="lnkNotePad4_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>   
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label4" runat="server" Text="Tyrelink.LoginProcess"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnkNotePad5_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>  
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label5" runat="server" Text="Tyrelink.ReviewOrder"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton5" runat="server" OnClick="lnkNotePad6_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label6" runat="server" Text="Tyrelink.Search"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton6" runat="server" OnClick="lnkNotePad7_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>       
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label7" runat="server" Text="Tyrelink.SubstituteTyre"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton7" runat="server" OnClick="lnkNotePad8_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>   
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label8" runat="server" Text="Tyrelink.UploadOrder"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton8" runat="server" OnClick="lnkNotePad9_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label9" runat="server" Text="Monetrics.AuditHistory"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton9" runat="server" OnClick="lnkNotePad10_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>     
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label10" runat="server" Text="Monetrics.PrintPreview"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton10" runat="server" OnClick="lnkNotePad11_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
                                        </ol>
                                    </div>
                                </td>
                           </tr>     
                           <!-- Row 5 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 30%" class="labelcolumn">
                                    <asp:Label ID="Label11" runat="server" Text="Monetrics.SavePrompt"></asp:Label>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:LinkButton ID="LinkButton11" runat="server" OnClick="lnkNotePad12_Click" >View Diagram</asp:LinkButton>
                                </td>
                                <td style="width: 59%; text-align:left;" class="requiredcolumn">
                                    <div class="DivClassFeature1">
                                        <ol>
                                            <li>Enter Text</li>                                            
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
