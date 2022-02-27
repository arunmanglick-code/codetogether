<%@ Page Language="c#" Inherits="Vocada.CSTools.DirectoryMaintenance" MasterPageFile="~/cs_tool.master"
    CodeFile="directory_maintenance.aspx.cs" Theme="csTool" Title="CSTools: Directory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMessageList" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

            <script language="javascript">
          function changeLinkColor(link)
          {
            document.getElementById(link).style.color="Brown";
            document.getElementById(link).style.fontWeight="Bold";
          }
           //Redirects user to the given URL as the Response.Redirect doesn't works sometime.
                function Navigate(url)
                {
                    try
                    {
                        window.location.href = url;
                    }
                    catch(_error)
                    {
                        return;
                    }
                }

    var mapId = "directory_maintenance.aspx";          
            </script>

            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr height="2%">
                    <td valign="top">
                        <table width="100%" border="0" cellpadding="=0" cellspacing="0" style="height: 1%">
                            <tr>
                                <td width="80%" class="Hd1">
                                    OC Directory</td>
                                <td align="right" nowrap width="0%" class="Hd1">
                                    <% if (strUserSettings != "YES")
                                       { %>
                                    <img src="img/ic_add.gif" /><% } %><asp:HyperLink ID="hlinkUserMaintenance" runat="server"
                                        CssClass="Link" NavigateUrl="./add_rp.aspx"> Add Ordering Clinician</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td height="1%" colspan="2">
                                    <img src="img/1px.gif" width="90%" height="1%" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr height="4%" class="ContentBg">
                    <td class="Hd2" valign="top">
                        <table width="98%" border="0" cellpadding="0" cellspacing="0" style="margin-left: 10px;
                            margin-top: 0px;">
                            <tr>
                                <td class="Hd2">
                                    <fieldset class="fieldsetCBlue">
                                        <legend class="">
                                            <asp:Label ID="lblDirectoryInfoLine" runat="server"></asp:Label></legend>
                                        <table height="20%" align="center" border="0" cellpadding="2" cellspacing="1" style="width: 59%">
                                            <tr>
                                                <td width="20%">
                                                    &nbsp;&nbsp;&nbsp;</td>
                                                <td width="9%" nowrap>
                                                    Search full or partial first or last name:</td>
                                                <td nowrap width="15%%">
                                                    <asp:TextBox ID="txtSearch" Width="100" runat="server" CssClass="txtSearch"></asp:TextBox>
                                                </td>
                                                <td width="77%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="Frmbutton" Text="Search" Height="19px"
                                                        OnClick="btnSearch_Click"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" nowrap>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblSearchResult" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="DivBg" valign="top">
                        <table width="98%" align="center" border="0" cellpadding="1" cellspacing="1">
                            <tr>
                                <td nowrap>
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
                            <tr>
                                <td height="12%">
                                    <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="True"></asp:Label>
                                    <input type="hidden" id="hidAlphabetSelected" value="" runat="server" />
                                    <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="DivBg" valign="top"><br />
                                    <!-- START div for Message details -->
                                    <div id="PhysiciansDiv" class="TDiv">
                                        <asp:DataGrid ID="dgPhysicians" AutoGenerateColumns="False" Width="100%" AllowSorting="True"
                                            runat="server" CssClass="GridHeader" OnPageIndexChanged="dgPhysicians_PageIndexChanged"
                                            PageSize="200" OnSortCommand="dgPhysicians_SortCommand" CellPadding="0" ItemStyle-Height="25px"
                                            EnableViewState="false">
                                            <AlternatingItemStyle CssClass="AltRow"></AlternatingItemStyle>
                                            <HeaderStyle VerticalAlign="Top" CssClass="THeader" HorizontalAlign="Left" Font-Bold="True">
                                            </HeaderStyle>
                                            <Columns>
                                                <asp:HyperLinkColumn DataNavigateUrlField="ReferringPhysicianID" DataNavigateUrlFormatString="./edit_oc.aspx?ReferringPhysicianID={0}&DirectoryMaintenance=1"
                                                    DataTextField="LastName" HeaderText="Last Name" SortExpression="LastName">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:HyperLinkColumn>
                                                <asp:HyperLinkColumn DataNavigateUrlField="ReferringPhysicianID" DataNavigateUrlFormatString="./edit_oc.aspx?ReferringPhysicianID={0}&DirectoryMaintenance=1"
                                                    DataTextField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:HyperLinkColumn>
                                                <asp:BoundColumn DataField="Nickname" HeaderText="Nickname">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="VoiceOverURL" HeaderText="OC Name VoiceOver">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Height="21px" HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Specialty" HeaderText="Specialty" SortExpression="Specialty">
                                                    <HeaderStyle Width="9%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PracticeGroup" HeaderText="Practice Group" SortExpression="PracticeGroup">
                                                    <HeaderStyle Width="9%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PrimaryPhone" HeaderText="Office Phone">
                                                    <HeaderStyle Width="11%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Status" HeaderText="Status">
                                                    <HeaderStyle Width="15%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:BoundColumn>
                                                <asp:HyperLinkColumn Text="Edit" DataNavigateUrlField="ReferringPhysicianID" DataNavigateUrlFormatString="./edit_oc.aspx?ReferringPhysicianID={0}&DirectoryMaintenance=1"
                                                    HeaderText="Edit">
                                                    <HeaderStyle Width="6%" />
                                                    <ItemStyle Height="21px" />
                                                </asp:HyperLinkColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="Next" PrevPageText="Previous" Height="12%" Font-Bold="True"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                HorizontalAlign="Right" />
                                        </asp:DataGrid>
                                    </div>
                                    </br>
                                    <table width="100%">
                                        <tr>
                                            <td align="center" valign="top">
                                                <asp:Label ID="lblNoRecordFound" runat="server" Font-Size="Small" ForeColor="Green"
                                                    Visible="true" Style="position: relative; text-align: center;"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!-- END div for Message details -->
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
