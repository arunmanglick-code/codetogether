<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="user_management.aspx.cs"
    Inherits="user_management" Title="CSTools: User Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upnlUserManagement" runat="server">
        <ContentTemplate>

            <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
            <script language="javascript" type="text/javascript">
             function validateLoginID(source, arguments)
            {
                var len = document.getElementById(txtLoginIdClientID).value.length;
                if(len > 1 && len < 26)
                {
                    arguments.IsValid = true;
                    return true;
                }
                else
                {
                    arguments.IsValid = false;
                    document.getElementById(txtLoginIdClientID).focus();
                    return false;
                }   
            }
            
            </script>
            
            <asp:HiddenField ID="hdnTestDataChanged" runat="server" Value="false" />
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
                                                    <asp:Label ID="lblDirectoryListHeader" runat="server" CssClass="UserCenterTitle">Manage Users</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                            <tr valign="top" id="trGrid" runat="server">
                                                <td class="Hd2" align="center">
                                                    <fieldset id="fldShift" class="fieldsetCBlue" runat="server">
                                                        </br> <legend class="">Current Users</legend></br>
                                                        <div id="UserInfoDiv" class="TDiv" style="margin-left: 0px; margin-right: 0px;">
                                                            <asp:DataGrid ID="grdUsers" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                                                                BorderWidth="1px" DataKeyField="VOCUserID" runat="server" CssClass="GridHeader"
                                                                CellPadding="0" ItemStyle-Height="25" AlternatingItemStyle-Height="25" ItemStyle-CssClass="Row2"
                                                                OnEditCommand="grdUsers_EditCommand" OnSortCommand="grdUsers_SortCommand" OnItemCreated="grdUsers_OnItemCreated"
                                                                ShowHeader="true">
                                                                <AlternatingItemStyle CssClass="Row3"></AlternatingItemStyle>
                                                                <HeaderStyle CssClass="THeader" HorizontalAlign="Left" Font-Bold="True"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                                                                        <HeaderStyle Width="15%" />
                                                                        <ItemStyle Height="21px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName">
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
                                                                        <ItemStyle Width="10px" />
                                                                    </asp:ButtonColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="RoleID" ReadOnly="True" HeaderText="RoleID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="VOCUserID" ReadOnly="True" HeaderText="UserID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="LoginID" ReadOnly="True" HeaderText="LoginID">
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn Visible="False" DataField="Password" ReadOnly="True" HeaderText="Password">
                                                                    </asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr valign="top" style="width: 100%;" align="center">
                                                <td class="Hd2">
                                                    <asp:RequiredFieldValidator ID="rfvFirstName" Style="z-index: 101; left: 438px; position: absolute;
                                                        top: 281px" SetFocusOnError="true" runat="server" Display="None" ErrorMessage="You must enter a First Name"
                                                        ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvLastName" Style="z-index: 103; left: 442px; position: absolute;
                                                        top: 314px" SetFocusOnError="true" runat="server" Display="None" ErrorMessage="You must enter a Last Name"
                                                        ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvalLoginID" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="You must enter a Login ID." ControlToValidate="txtLoginID" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revLoginID" ValidationGroup="SubscriberInfo"
                                                            runat="server" ErrorMessage="Please enter valid Login ID." ControlToValidate="txtLoginId"
                                                            SetFocusOnError="true" Display="None" ValidationExpression="((\w)*(\d)*)"></asp:RegularExpressionValidator>
                                                    <asp:CustomValidator ID="ctmValLogin" runat="server" ControlToValidate="txtLoginID"
                                                        ClientValidationFunction="validateLoginID" 
                                                        Display="None" ErrorMessage="Login ID must be 2-25 characters/digits."></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="rfvalPasswordValue" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="You must enter a Password." ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" runat="server"
                                                        ErrorMessage="You must select a Role." InitialValue="-1" ControlToValidate="cmbRoles"
                                                        Display="None"></asp:RequiredFieldValidator>
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
                                                        ErrorMessage="Password must be 8-20 characters." ControlToValidate="txtPassword"
                                                        Display="None" ValidationExpression="^[\s\S]{8,20}$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revEmailFormat" Style="z-index: 110; left: 435px;
                                                        position: absolute; top: 221px" runat="server" Display="None" ErrorMessage="Email format incorrect"
                                                        ControlToValidate="txtEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    </br>
                                                    <fieldset class="fieldsetCBlue">
                                                        <legend class="">User Profile</legend>
                                                        <table align="center" width="98%" border="0" cellpadding="2" cellspacing="1">
                                                            <tr>
                                                                <td style="width: 5%;">
                                                                </td>
                                                                <td style="width: 19%">
                                                                </td>
                                                                <td style="width: 36%">
                                                                </td>
                                                                <td style="width: 15%">
                                                                </td>
                                                                <td style="width: 20%">
                                                                </td>
                                                                <td style="width: 5%">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    First Name*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFirstName" runat="server" Columns="35" MaxLength="75" Width="200"
                                                                        TabIndex="1"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    Email:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEmail" runat="server" Columns="35" MaxLength="100" Width="200"
                                                                        TabIndex="5"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    Last Name*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLastName" runat="server" Columns="35" MaxLength="75" Width="200"
                                                                        TabIndex="2"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    Phone:</td>
                                                                <td valign="middle">
                                                                    (<asp:TextBox ID="txtPhoneAreaCode" TabIndex="6" Columns="4" runat="server" MaxLength="3"></asp:TextBox>)
                                                                    <asp:TextBox ID="txtPhonePrefix" TabIndex="7" runat="server" Columns="4" MaxLength="3"></asp:TextBox>-
                                                                    <asp:TextBox ID="txtPhoneNNNN" TabIndex="8" runat="server" Columns="6" MaxLength="4"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
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
                                                                    Login ID*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLoginID" runat="server" Columns="10" MaxLength="25" Width="200"
                                                                        TabIndex="9"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    Active:</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="4" />
                                                                </td>
                                                                <td>
                                                                    Password*:</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPassword" runat="server" Columns="12" MaxLength="20" TabIndex="10"></asp:TextBox>
                                                                    &nbsp;&nbsp;
                                                                    <asp:Button ID="btnGeneratePassword" runat="server" CssClass="Frmbutton" Width="105px"
                                                                        TabIndex="11" CausesValidation="False" Text=" Generate Password " UseSubmitBehavior="false"
                                                                        OnClick="btnGeneratePassword_Click" onKeyDown="if(event.keyCode==13) return false;">
                                                                    </asp:Button>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                        <p align="center">
                                                            <asp:Button ID="btnAdd" runat="server" Text=" Save " CssClass="Frmbutton" OnClick="btnAdd_Click"
                                                                TabIndex="12" />&nbsp;&nbsp;
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Frmbutton" Enabled="true"
                                                                TabIndex="13" OnClick="btnCancel_Click" CausesValidation="false" />
                                                        </p>
                                                    </fieldset>
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
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
                var mapId = "user_management.aspx";
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
