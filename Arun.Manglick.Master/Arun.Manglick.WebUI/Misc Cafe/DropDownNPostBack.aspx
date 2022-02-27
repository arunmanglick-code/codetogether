<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DropDownNPostBack.aspx.cs" Inherits="DropDownNPostBack" Title="DropDown N PostBack Problem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">
    <script src="../JS/Common.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    
    function ConfirmChange()
    {
        var idx = hiddenLanguageClientIndex.value; 
        var res = window.confirm("Do you want to continue");
        if(res)
        {
            
            var hiddenButton=document.getElementById('<%= btnHidden.ClientID %>');
            //hiddenButton.click();    
        }
        else
        {
            var countryList = document.getElementById('<%= ddlCountry.ClientID %>');
            countryList.selectedIndex = idx;
        }
        
    }
      
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Which Control Made PostBack"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" EnableViewState="false" Visible="true"></asp:Label>
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
                    <div class="DivClassFeature" style="width:650px;">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Solution to the Problem- Dropdown requires a hidden click if it has a JS validation attahced</li>
                            <li><a href='http://forums.asp.net/t/1129832.aspx'>Here is the Link</a></li>
                            <li>Test Case - Select/Change Item from Country Dropdown.</li>                            
                        </ol>
                         <ol>
                            <li>However this solution gives birth to one problem in FireFox.</li>
                            <li>This problem only exist when there is some Validator attached on Button Click.</li>
                            <li>For example, here with Save Button.</li>
                            <li>Test Case - Load the page for first time</li>                            
                            <li>Hit Save. This will fire Validators.</li>
                            <li>Change Country from DropDown.</li>
                            <li>Changing country will automatically fire Save Button, as it keeps the piled up event.</li>
                            <li>Solution to this - Follow 'mTogglePipedSave' variable in code.</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                         <br />
                        <br />
                        <table border="0" width="100%" cellpadding="1" cellspacing="3">                            
                           <!-- Row 3 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblCountry" runat="server" Text="Select Country"></asp:Label>
                                </td>
                                <td style="width: 27%" class="inputcolumn">
                                    <asp:DropDownList ID="ddlCountry"  runat="server" CausesValidation="false" AutoPostBack="true" onchange="ConfirmChange();" onselectedindexchanged="ddlCountry_SelectedIndexChanged">
                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                        <asp:ListItem>America</asp:ListItem>
                                        <asp:ListItem>India</asp:ListItem>
                                        <asp:ListItem>Australia</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator Display="Dynamic" ControlToValidate="ddlCountry" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Country"></asp:RequiredFieldValidator>
                                    <asp:HiddenField id="HiddenLanguageClientIndex" runat="server" Value=""></asp:HiddenField>
                                    <asp:Button ID="btnHidden" runat="server" Width="150px" class="button" Style="visibility: hidden;" CausesValidation="false" OnClick="btnHidden_Click" />
                                </td>
                                <td style="width: 70%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                           </tr> 
                           <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="labelcolumn">
                                    <asp:Label ID="lblState" runat="server" Text="Country's States"></asp:Label>
                                </td>
                                <td style="width: 5%" class="inputcolumn">
                                    <asp:DropDownList ID="ddlStates" runat="server" Enabled="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                           </tr> 
                            <!-- Row 4 -->
                            <tr>
                                <td style="width: 1%" class="requiredcolumn">
                                    &nbsp;
                                </td>
                                <td style="width: 2%" class="inputcolumn">                                    
                                    <br />
                                   <asp:Button ID="btnPostback" CausesValidation="false" runat="server" Width="150px" class="button" Text="Simple Postback" OnClick="btnPostback_Click" />
                                </td>
                                <td style="width: 5%" class="labelcolumn">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Width="150px" class="button" Text="Save" OnClick="btnSave_Click" />
                                </td>
                                <td style="width: 92%" class="requiredcolumn">
                                </td>
                             </tr>                            
                        </table>    
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var hiddenLanguageClientIndex=document.getElementById('<%= HiddenLanguageClientIndex.ClientID %>');
        var countryListTemp = document.getElementById('<%= ddlCountry.ClientID %>');
        if(hiddenLanguageClientIndex != null)
        {
            hiddenLanguageClientIndex.value = countryListTemp.selectedIndex;
        }

    </script>
</asp:Content>
