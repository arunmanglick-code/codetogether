<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="add_CC_agent.aspx.cs"
    Inherits="add_agent" Title="CSTools: Add Agent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlAddCC" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
            <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">
                <tr class="ContentBg">
                    <td valign="top">
                        <div style="overflow-y: Auto; height: 100%">
                            <table height="100%" align="center" width="100%" style="background-color: White"
                                border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                            <tr>
                                                <td class="Hd1" style="height: 19px">
                                                    <asp:Label ID="lblDirectoryListHeader" runat="server" CssClass="UserCenterTitle">Manage Agents</asp:Label>
                                                </td>
                                                <td align="right" nowrap width="0%" class="Hd1">
                                                    <asp:HyperLink ID="hlinkCallCenterSetup" runat="server" CssClass="Link"> Agent Team Setup</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                            <tr valign="top" id="trGrid" runat="server">
                                                <td class="Hd2" align="center">
                                                    <fieldset id="fldShift" class="fieldsetCBlue" runat="server">
                                                        </br> <legend class="">
                                                            <asp:Label ID="lblManageUser" runat="server" CssClass="UserCenterTitle">Current Agents</asp:Label></legend>
                                                        </br>
                                                        <div id="UserInfoDiv" class="TDiv" style="margin-left: 0px; margin-right: 0px;">
                                                          <asp:UpdatePanel ID="upnlGrdUsers" runat="server" UpdateMode="Conditional">
                                                              <ContentTemplate>
                                                            <asp:DataGrid ID="grdUsers" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                                                                BorderWidth="1px" DataKeyField="VOCUserID" runat="server" CssClass="GridHeader"
                                                                CellPadding="0" ItemStyle-Height="25" AlternatingItemStyle-Height="25" ItemStyle-CssClass="Row2"
                                                                OnEditCommand="grdUsers_EditCommand" OnSortCommand="grdUsers_SortCommand" OnDeleteCommand="grdUsers_DeleteCommand"
                                                                OnItemCreated="grdUsers_OnItemCreated" OnItemDataBound="grdUsers_ItemDataBound"
                                                                ShowHeader="true">
                                                                <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                                                <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="AgentName" HeaderText="User Name" SortExpression="FirstName">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RoleDescription" HeaderText="Role" SortExpression="RoleDescription">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Email" HeaderText="Email Address">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Phone" HeaderText="Phone">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Active" HeaderText="Status">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit">
                                                                        <HeaderStyle Width="10%" HorizontalAlign="left" />
                                                                        <ItemStyle Width="30px" HorizontalAlign="Left" />
                                                                    </asp:ButtonColumn>
                                                                    <asp:TemplateColumn HeaderText="Remove" Visible="true">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Remove" CommandName="Delete"
                                                                                CausesValidation="false" Visible="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="RoleID" ReadOnly="True" HeaderText="RoleID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="VOCUserID" ReadOnly="True" HeaderText="UserID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="LoginID" ReadOnly="True" HeaderText="LoginID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="Password" ReadOnly="True" HeaderText="Password">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="FirstName" ReadOnly="True" HeaderText="Password">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="LastName" ReadOnly="True" HeaderText="Password">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="Cell" ReadOnly="True" HeaderText="Cell">
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                           </ContentTemplate>
                                                            <Triggers>
                                                               <asp:AsyncPostbackTrigger ControlID="grdUsers" EventName="DataBinding"/>
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr valign="top" style="width: 100%;" align="center">
                                                <td class="Hd2">
                                                   <asp:UpdatePanel ID="upnlAddEdit" runat="server" UpdateMode="Conditional">
                                                     <ContentTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvFirstName" Style="z-index: 101; left: 438px; position: absolute;
                                                        top: 281px" SetFocusOnError="true" runat="server" Display="None" ErrorMessage="You must enter a First Name"
                                                        ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvLastName" Style="z-index: 103; left: 442px; position: absolute;
                                                        top: 314px" SetFocusOnError="true" runat="server" Display="None" ErrorMessage="You must enter a Last Name"
                                                        ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRole" SetFocusOnError="true" runat="server" ErrorMessage="You must select a Role."
                                                        InitialValue="-1" ControlToValidate="cmbRoles" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" SetFocusOnError="true"
                                                        runat="server" Display="None" ErrorMessage="Please Enter Valid Phone area code."
                                                        ControlToValidate="txtPhoneAreaCode" ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revPhonePrefix" runat="server" SetFocusOnError="true"
                                                        Display="None" ErrorMessage="Please Enter Valid Phone prefix." ValidationExpression="\d{3}"
                                                        ControlToValidate="txtPhonePrefix" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revPhoneNNNN" runat="server" SetFocusOnError="true"
                                                        Display="None" ErrorMessage="Please Enter Valid Phone extension." ValidationExpression="\d{4}"
                                                        ControlToValidate="txtPhoneNNNN" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revalPassword" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="Password must be 8-15 characters." ControlToValidate="txtPassword"
                                                        Display="None" ValidationExpression="^[\s\S]{8,15}$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revEmailFormat" Style="z-index: 110; left: 435px;
                                                        position: absolute; top: 221px" runat="server" Display="None" ErrorMessage="Email format incorrect"
                                                        ControlToValidate="txtEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revMobileArea" SetFocusOnError="true" runat="server"
                                                        Display="None" ErrorMessage="Please Enter Valid Mobile area code." ControlToValidate="txtmobileArea"
                                                        ValidationExpression="\d{3}" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revMobilePrefix" runat="server" SetFocusOnError="true"
                                                        Display="None" ErrorMessage="Please Enter Valid Mobile prefix." ValidationExpression="\d{3}"
                                                        ControlToValidate="txtprefix" ForeColor="White"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revMobileNNNN" runat="server" SetFocusOnError="true"
                                                        Display="None" ErrorMessage="Please Enter Valid Mobile extension." ValidationExpression="\d{4}"
                                                        ControlToValidate="txtMobileNNNN" ForeColor="White"></asp:RegularExpressionValidator>
                                                    </br>
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend class="">Agent Profile</legend>
                                                        <table align="center" width="98%" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    First Name*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstName" runat="server" Columns="35" MaxLength="20" Width="200"
                                                                        TabIndex="1"></asp:TextBox>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    Email:</td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="txtEmail" runat="server" Columns="35" MaxLength="100" Width="200"
                                                                        TabIndex="4"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    Last Name*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLastName" runat="server" Columns="35" MaxLength="20" Width="200"
                                                                        TabIndex="2"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    Phone:</td>
                                                                <td valign="middle">
                                                                    (<asp:TextBox ID="txtPhoneAreaCode" TabIndex="5" Columns="4" runat="server" MaxLength="3"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtPhonePrefix" TabIndex="6" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                    <asp:TextBox ID="txtPhoneNNNN" TabIndex="7" runat="server" Columns="6" MaxLength="4"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    Role*:</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbRoles" runat="server" Width="200" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    Mobile:</td>
                                                                <td>
                                                                    (<asp:TextBox ID="txtmobileArea" runat="server" Columns="4" MaxLength="3" TabIndex="8"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtprefix" runat="server" Columns="4" MaxLength="3" TabIndex="9"></asp:TextBox>-
                                                                    <asp:TextBox ID="txtMobileNNNN" runat="server" Columns="6" MaxLength="4" TabIndex="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    Status:</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkActive" runat="server" Text=" Active" Checked="true" TabIndex="11" />
                                                                </td>
                                                                <td>
                                                                    Login ID:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLoginID" runat="server" Columns="10" MaxLength="10" Width="200"
                                                                        TabIndex="13"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td align="left" valign="top">
                                                                    Default Groups :</td>
                                                                <td rowspan="4" align="left" valign="top">
                                                                    <div id="Div1" runat="server" style="overflow-y: Auto; height: 85px; width: 300px;
                                                                        vertical-align: top; margin-bottom: 10px; margin-left: 0px; margin-right: 0px;"
                                                                        class="TDiv">
                                                                        <asp:CheckBoxList ID="clstGroupList" runat="server" Width="250px" Height="1px" RepeatLayout="Flow"
                                                                            TabIndex="12">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                    <asp:Label ForeColor="Green" Font-Size="X-Small" ID="lblNoRecords" runat="server"
                                                                        Text="No Groups available" Visible="false"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    Password:</td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="txtPassword" runat="server" Columns="12" MaxLength="15" TabIndex="14"></asp:TextBox>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnGeneratePassword" runat="server" CssClass="Frmbutton" Width="105px"
                                                                        TabIndex="15" CausesValidation="False" Text=" Generate Password " UseSubmitBehavior="false"
                                                                        OnClick="btnGeneratePassword_Click" onKeyDown="if(event.keyCode==13) return false;">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <p align="center">
                                                            <asp:Button ID="btnAdd" runat="server" Text=" Save " CssClass="Frmbutton" OnClick="btnAdd_Click"
                                                                TabIndex="16" />&nbsp;&nbsp;
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Frmbutton" Enabled="true"
                                                                TabIndex="17" OnClick="btnCancel_Click" CausesValidation="false" />
                                                            <br />
                                                            <asp:Label ID="lblLoginUserMessage" runat="server" ForeColor="Red" Text="Agent already logged in to Veriphy System." />
                                                        </p>
                                                    </fieldset>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />   
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />                                                                 
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="ValidationSummary1" Style="z-index: 107; left: 416px;
                                            position: absolute; top: 163px" runat="server" ShowSummary="False" ShowMessageBox="True">
                                        </asp:ValidationSummary>
                                    </td>
                                </tr>
                                <tr><td>
                                 <asp:UpdatePanel ID="upnlHidenData" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                 <asp:HiddenField ID="hdnTestDataChanged" runat="server" Value="false" />
                                           </ContentTemplate>
                                         </asp:UpdatePanel>
                                </td></tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
                var mapId = "add_CC_agent.aspx";
                var textchanged;
                var isEditMode = false; 
                /*Change value of hidden variable with given value*/
                function testdataChanged()
                {       
                    document.getElementById(hdnTestDataChangedClientID).value = "true";
                    textchanged = true;
                    return;
                }

                /*Change value of hidden variable with given value*/
                function resetTestDataChanged()
                {   
                    document.getElementById(hdnTestDataChangedClientID).value = "false";
                    textchanged = false;
                    return;
                }
                 /* Alert user if data chnaged in edit section of lab test */
                function onTestdataChanged()
                {
                    if(document.getElementById(hdnTestDataChangedClientID).value == 'true')
                    {
                        if(confirm("Some data has been changed, do you want to continue?"))
                        {
                            document.getElementById(hdnTestDataChangedClientID).value = 'false';
                            return true;
                        }
                        return false;         
                     }
                     return true;
                }
               
    </script>

</asp:Content>
