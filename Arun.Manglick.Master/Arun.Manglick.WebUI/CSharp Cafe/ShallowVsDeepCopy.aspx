<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ShallowVsDeepCopy.aspx.cs" Inherits="ShallowVsDeepCopy" Title="Shallow Vs DeepCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Shallow Vs Deep Copy"></asp:Label></td>
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
                    <div class="DivClassFeature" style="width:600px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Shallow copy is done by the object method MemberwiseClone()</li>
                            <li>DeepCopy requires the classes to be flagged as [Serializable]</li>
                        </ol>
                        
                        Shallow Copy
                        <ol>
                            <li>Value type --> A bit-by-bit copy of the field is performed</li>
                            <li>Reference type --> The reference is copied but the referred object is not; therefore, the original object and its clone refer to the same object</li>
                        </ol>
                        
                        Deep Copy
                        <ol>
                            <li>Value type --> A bit-by-bit copy of the field is performed</li>
                            <li>Reference type --> A new copy of the referred object is performed. Therefore, the original object and its clone refer to the seperate object</li>
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
                                        Text="Shallow Vs DeepCopy"></asp:Label>
                                </td>
                            </tr>                            
                            <!-- Row 2 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView1" runat="server" OnClick="lnkNotePad1_Click">View Object to Shallow-Copied</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnBeforeShallowCopy" runat="server" class="button" Width="250px" Text="Before ShallowCopy" OnClick="btnBeforeShallowCopy_Click" />
                                </td>                               
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnAfterShallowCopy" runat="server" Width="250px" class="button" Text="After ShallowCopy" OnClick="btnAfterShallowCopy_Click" />
                                </td>
                            </tr>     
                            <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 20%" class="labelcolumn">
                                    <asp:LinkButton ID="lbtnView2" runat="server" OnClick="lnkNotePad2_Click">View Object to Deep-Copied</asp:LinkButton>
                                </td>
                                <td style="width: 10%" class="inputcolumn">
                                    <asp:Button ID="btnBeforeDeepCopy" runat="server" class="button" Width="250px" Text="Before DeepCopy" OnClick="btnBeforeDeepCopy_Click" />
                                </td>                               
                                <td style="width: 49%" class="inputcolumn">
                                    <asp:Button ID="btnAfterDeepCopy" runat="server" Width="250px" class="button" Text="After DeepCopy" OnClick="btnAfterDeepCopy_Click" />
                                </td>
                            </tr>                                                                     
                        </table>                        
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
