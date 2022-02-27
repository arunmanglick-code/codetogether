<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" Theme="csTool" AutoEventWireup="true"
    CodeFile="voiceover_utility.aspx.cs" Inherits="Vocada.CSTools.voiceover_utility"
    Title="CSTools: OC Voice-over Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelAddInstitution" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="./Javascript/common.js" type="text/JavaScript"></script>

            <script language="javascript" type="text/javascript">
var mapId = "voiceover_utility.aspx";
function ConformBeforeDelete()
    {
        if(confirm("Are you sure you want to delete this record?"))
        {
            return true;
        }
        return false;                                          
    }    
    
        function Validate()
        {
            var errorMessage = "";
                       
            if(document.getElementById(cmbInstitutionClientID).value == "-1")
                    errorMessage = "Please select Institution.\r\n";
           
            if(document.getElementById(cmbDirectoryClientID).value == "-1")
                errorMessage += "Please select Directory."; 
           
            if(errorMessage.length > 0)
              {
                alert(errorMessage);
                return false;
              }
            else
                {
                   var tbl = document.getElementById(dgOCClientID);
                   if(tbl != null)
                   {
                        var tblBody = tbl.getElementsByTagName("tbody")[0];
                        if(tblBody != null)
                        {
                            tblBody.innerText = '';
                            document.getElementById("divOC").style.visibility = "hidden";
                            document.getElementById(lblRecordCountClientID).style.visibility= "hidden";
                        }
                    }
                   PageLoad(2);
                }  
            return true;   
        }
        function PageLoad(iVar)
        {   
            if (iVar ==1)
            {
                try
                {
                    //Added By Prerak for Loading icon changes.
                    document.getElementById("ctl00_tdTools").style.visibility='visible';
                    document.getElementById("ctl00_tdToolsLoading").style.visibility='hidden';
                    document.getElementById("ctl00_tdToolsLoading").style.display='none';
                    document.getElementById("ctl00_tdTools").style.display='inline';
                 }
               
                 catch(e){}
             }
             else
             {
                try
                {
                    //Added By Prerak for Loading icon changes.
                    document.getElementById("ctl00_tdToolsLoading").style.visibility='visible';
                    document.getElementById("ctl00_tdTools").style.visibility='hidden';
                    document.getElementById("ctl00_tdTools").style.display='none';
                    document.getElementById("ctl00_tdToolsLoading").style.display='block';
                    document.getElementById("ctl00_tdToolsLoading").style.width='79px';
                    return true;
                 }
               
                 catch(e){}
             }
        }
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr height="94%" class="ContentBg">
                    <td class="DivBg" valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="Hd1">
                                        OC Voice-over Utility
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                <tr valign="top">
                                    <td align="center">
                                        <fieldset class="fieldsetCBlue">
                                            <legend class="">Select</legend>
                                            <table border="0" style="width: 70%;">
                                                <tr valign="middle">
                                                    <td style="width: 10%; white-space: nowrap;" align="right">
                                                        Institution Name:&nbsp;&nbsp;</td>
                                                    <td style="width: 20%; white-space: nowrap;" align="left">
                                                        <asp:DropDownList ID="cmbInstitution" runat="server" DataValueField="InstitutionID"
                                                            Width="200" DataTextField="InstitutionName" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitution_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%; white-space: nowrap;" align="right">
                                                        Directory:&nbsp;&nbsp;</td>
                                                    <td style="width: 25%; white-space: nowrap;" align="left">
                                                        <asp:DropDownList runat="server" ID="cmbDirectory" DataTextField="DirectoryDescription"
                                                            DataValueField="DirectoryID" AutoPostBack="true" OnSelectedIndexChanged="cmbDirectory_SelectedIndexChanged"
                                                            Width="200">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="middle" style="width: 40%; white-space: nowrap;">
                                                        <asp:RadioButtonList ID="rblApprove" runat="server" RepeatDirection="horizontal"
                                                            AutoPostBack="false" CssClass="radiobutton" >
                                                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Unapproved" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="All" Value="2" Selected="true"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table border="0" style="width: 70%;">
                                                <tr valign="middle">
                                                    
                                                    </td>
                                                    <td style="white-space: nowrap; width: 20%;" align="left">
                                                        Search full or partial first or last name:
                                                    </td>
                                                    <td style="width: 20%;">
                                                        <asp:TextBox ID="txtSearch" Width="200" runat="server" CssClass="txtSearch" AutoPostBack="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 40%;">
                                                        &nbsp;&nbsp;<asp:Button Text="Get OC Voice-over" ID="btnOCVoiceOver" runat="server"
                                                            OnClientClick="return Validate();" CssClass="Frmbutton" OnClick="btnOCVoiceOver_Click" />
                                                    </td>
                                                    <td colspan="2" style="width: 20%;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" height="25" border="0" align="center">
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hidAlphabetSelected" value="" runat="server" />
                                                    <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                                                    <div id="divLinks" runat="server">
                                                        <asp:Table ID="tblAlphabet" runat="server" Width="100%">
                                                            <asp:TableRow Font-Names="verdana" Font-Size="Smaller">
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aA">A</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aB">B</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aC">C</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aD">D</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aE">E</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aF">F</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aG">G</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aH">H</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aI">I</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aJ">J</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aK">K</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aL">L</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aM">M</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aN">N</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aO">O</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aP">P</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aQ">Q</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aR">R</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aS">S</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aT">T</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aU">U</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aV">V</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aW">W</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aX">X</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aY">Y</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="hl_SelectedIndexChanged" ID="aZ">Z</asp:LinkButton></asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                <tr>
                                    <td style="margin-left: 10px; height:35;">
                                        <asp:UpdatePanel ID="upnlRecords" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:Label ID="lblRecordCount" runat="server" Font-Bold="true"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <div class="TDiv" id="divOC">
                                            <asp:DataGrid runat="server" ID="dgOC" OnDeleteCommand="dgOC_DeleteCommand" CssClass="GridHeader"
                                                Width="100%" OnItemDataBound="dgOC_ItemDataBound" AutoGenerateColumns="False"
                                                OnUpdateCommand="dgOC_UpdateCommand" AllowSorting="True" OnSortCommand="dgOC_SortCommand"
                                                ItemStyle-Height="22" AlternatingItemStyle-CssClass="Row3" CellPadding="0">
                                                <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"
                                                    Height="22"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn HeaderText="OCID" Visible="false" DataField="ReferringPhysicianID">
                                                    </asp:BoundColumn>
                                                     <asp:BoundColumn HeaderText="Directory" Visible="false" DataField="DirectoryDescription" ItemStyle-Width="15%">
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="ReferringPhysicianID" DataNavigateUrlFormatString="./edit_oc.aspx?ReferringPhysicianID={0}"
                                                        DataTextField="ReferringPhysicianDisplayName" HeaderText="Ordering Clinician"
                                                        SortExpression="ReferringPhysicianDisplayName" ItemStyle-Width="30%">
                                                        <HeaderStyle Width="60%" HorizontalAlign="left" />
                                                        <ItemStyle Width="60%" HorizontalAlign="Left" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="VoiceOverURL" DataNavigateUrlFormatString="{0}"
                                                        DataTextField="VoiceOverURL" HeaderText="Voice over" DataTextFormatString="&lt;img border=0 src='./img/ic_play_msg.gif'&gt;">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:HyperLinkColumn>
                                                    <asp:TemplateColumn HeaderText="Approved">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgApproved" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Approve">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnApprove" runat="server" Text="Approve" CommandName="Update"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Delete Voice-over">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete" CommandName="Delete"
                                                                OnClientClick="return ConformBeforeDelete();" CausesValidation="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                                                        <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="upnlNoRecords" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:Label ID="lblNorecord" runat="server" Font-Size="Small" ForeColor="green" Style="position: relative;
                                                    text-align: center;"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="cmbDirectory" />
            <asp:AsyncPostBackTrigger ControlID="cmbInstitution" />
            <asp:AsyncPostBackTrigger ControlID="rblApprove" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
