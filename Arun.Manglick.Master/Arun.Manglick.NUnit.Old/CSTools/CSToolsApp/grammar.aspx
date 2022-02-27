<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" Theme="csTool" AutoEventWireup="true"
    CodeFile="grammar.aspx.cs" Inherits="Vocada.CSTools.grammar" Title="CSTools: OC Grammar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel id="upnlGrammer" runat="server" UpdateMode="Conditional">
<ContentTemplate>
    <script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>

    <script language="JavaScript" type="text/JavaScript">
        var mapId = "grammar.aspx";
        /* Validate Search criteria specified or not on button 'Get Grammar' Click*/
        function Validate()
        {
            var errorMessage = "";
            if(confirmOnDataChange())
            {
                var hdnIsSystemAdmin = document.getElementById(hdnIsSystemAdminClientID).value;
                if (hdnIsSystemAdmin == "1")
                {
                    if(document.getElementById(cmbInstitutionsClientID).value == "-1")
                        errorMessage = "Please select Institution.\r\n";
                }
                if(document.getElementById(cmbDirectoriesClientID).value == "-1")
                    errorMessage += "Please select Directory."; 
               
                if(errorMessage.length > 0)
                  {
                    alert(errorMessage);
                    return false;
                  }
                else
                {
                   PageLoad(2);
                }   
                return true;   
            }
            else
            {
                return false;
            }
            
        }
       
       /* Validate input grammar, and show msg if it is invalid*/ 
       function validateGrammar(txtGrammar, index)
       {
            var txtGrammarVal = document.getElementById(txtGrammar).value;
            var exp = new RegExp("^[a-zA-Z0-9;\\s\\.\\']{0,4000}$");
            var returnVal = txtGrammarVal.match(exp);
           
            document.getElementById(hdnEditedTextClientID).value = txtGrammarVal;
            if (returnVal == null)
                {
                    alert("Please enter valid grammar.");
                    return false;
                }
            else
                {
                    document.getElementById(hdnGrammarDataChangedClientID).value = "false";   
                    if(index < 10)
                        __doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl0' + index + '$ctl02', '')
                    else
                        __doPostBack('ctl00$ContentPlaceHolder1$grdOCGrammar$ctl' + index + '$ctl02', '')
                    return false;    
                }
                return false; 
       }
       
       //Set value for the hidden varibale which is used to check value of form data change or not
       function formDataChange(value)
       {
            document.getElementById(hdnGrammarDataChangedClientID).value = value;
       }
       
       //Check whether other record has been edited, then ask for conformation.
       function confirmOnDataChange()
       {
            if(document.getElementById(hdnGrammarDataChangedClientID).value =="true")
            {
                if(confirm("Some data has been changed, do you want to continue?"))
                {
                    document.getElementById(hdnGrammarDataChangedClientID).value = "false";
                    return true;
                }
                else
                    return false;                    
            }
            return true;
       }
       
       // On Institution chnage check whether form data has been changed, if yes the ask for confirmation.
       function onComboChange(isInstiCombo)
       {
            if(confirmOnDataChange())
            {
                if(isInstiCombo == 'true')
                     {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbInstitutions','');
                        document.getElementById(hdnInstitutionValClientID).value = document.getElementById(cmbInstitutionsClientID).value ;
                        document.getElementById(hdnDirectoryValClientID).value = "-1";
                     }
                else
                     {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbDirectories','');
                        document.getElementById(hdnDirectoryValClientID).value = document.getElementById(cmbDirectoriesClientID).value ;
                     }
                 return true;
            }
            else
            {
               if(isInstiCombo == 'true')
                     {
                        document.getElementById(cmbInstitutionsClientID).value = document.getElementById(hdnInstitutionValClientID).value;
                     }
                else
                     {
                        document.getElementById(cmbDirectoriesClientID).value = document.getElementById(hdnDirectoryValClientID).value;
                     }
                return false;
            }
       }
       
       /* Enable Disable button on selection of insti/directory */
       function enableControls()
       {    
            var directory;
            var institution; 
            var btnOCGrammarC;
            var alphaTableC;
            directory = document.getElementById(cmbDirectoriesClientID);
            institution = document.getElementById(cmbInstitutionsClientID);
            btnOCGrammarC = document.getElementById(btnOCGrammarClientID);
            alphaTableC =  document.getElementById(tblAlphabetClientID);
            
            if(directory  !=null && btnOCGrammarC !=null && institution != null)
            {
                if(directory.options[directory.selectedIndex].value != "-1")
                {
                    btnOCGrammarC.disabled = false;
                    if(alphaTableC != null)
                        alphaTableC.visible = true;
                }
                else
                {
                    btnOCGrammarC.disabled = true;
                    if(alphaTableC != null)
                        alphaTableC.visible = false;
                }
            }        
       }
       function CheckMaxLenght(controlId, length)
                {
                    var text = document.getElementById(controlId).value;
                    if(text.length > length)
                    {
                        document.getElementById(controlId).value = text.substring(0,length);
                    }        
                }    
                
       function isValidKeyStroke()
        {
            var keyCode = window.event.keyCode ? window.event.keyCode: window.event.charCode;
            
            if ( ((keyCode >= 48) && (keyCode <= 57))  ||   // All numerics
                 ((keyCode >= 65) && (keyCode <= 90))  ||   // All CAPS alphabets
                 ((keyCode >= 97) && (keyCode <= 122)) ||   // All alphabets
                 (keyCode == 32)  ||                        // Space
                 (keyCode == 39)  ||                        // SINGLE QUOTE
                 (keyCode == 59) )                          // Semi Colon
                {        
                    return;                                  
                }
            
            window.event.returnValue = null;     
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
                <asp:HiddenField ID="hdnGrammarDataChanged" runat="server" Value="false" />
                <asp:HiddenField ID="hdnInstitutionVal" runat="server" Value="false" />
                <asp:HiddenField ID="hdnDirectoryVal" runat="server" Value="false" />
                <div style="overflow-y: Auto; height: 100%; ">
                    <table align="center" style="height: 100%; width: 100%;" border="0" cellpadding="0"
                        cellspacing="0">
                        <tr style="background-color: White; height: 100%">
                            <td class="DivBg" valign="top">
                                <table width="100%" border="0" cellpadding="=0" cellspacing="0">
                                    <tr>
                                        <td class="Hd1">
                                            OC Grammars
                                        </td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                    <tr>
                                        <td colspan="5">
                                            <fieldset class="fieldsetCBlue">
                                                <legend class="">Select</legend>
                                                  <table width="60%" border="0" align="center">
                                                    <tr>
                                                        <td style="white-space: nowrap; width: 10%;">
                                                            Institution Name:&nbsp;</td>
                                                        <td style="width: 25%;">
                                                            <asp:DropDownList runat="server" ID="cmbInstitutions" AutoPostBack="true" OnSelectedIndexChanged="cmbInstitutions_SelectedIndexChanged"
                                                                Width="200px">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblInstName" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td style="width: 10%;">
                                                            &nbsp;&nbsp;Directory:&nbsp;</td>
                                                        <td style="width: 25%;">
                                                            
                                                                    <asp:DropDownList runat="server" ID="cmbDirectories" OnSelectedIndexChanged="cmbDirectories_SelectedIndexChanged"
                                                                        AutoPostBack="true" Width="200px">
                                                                    </asp:DropDownList>
                                                                
                                                        </td>
                                                        <td style="white-space: nowrap; width: 10%;">
                                                            &nbsp;&nbsp;Search full or partial first or last name:
                                                        </td>
                                                        <td style="width: 10%;">
                                                            
                                                                    &nbsp;&nbsp;<asp:TextBox ID="txtSearch" Width="100" runat="server" CssClass="txtSearch"
                                                                        AutoPostBack="false"></asp:TextBox>
                                                               
                                                        </td>
                                                        <td style="width: 20%;">
                                                            
                                                                    &nbsp;&nbsp;<asp:Button Text="Get OC Grammar" ID="btnOCGrammar" runat="server" OnClientClick="return Validate();"
                                                                        CssClass="Frmbutton" OnClick="btnOCGrammar_Click" />
                                                                
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap;">
                                            
                                                    <div id="divLinks" runat="server">
                                                        <asp:Table ID="tblAlphabet" runat="server" Width="98%" align="Center" Style="border: 2px;">
                                                            <asp:TableRow Font-Names="verdana" Font-Size="Smaller">
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aA">A</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aB">B</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aC">C</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aD">D</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aE">E</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aF">F</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aG">G</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aH">H</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aI">I</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aJ">J</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aK">K</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aL">L</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aM">M</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aN">N</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aO">O</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aP">P</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aQ">Q</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aR">R</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aS">S</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aT">T</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aU">U</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aV">V</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aW">W</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aX">X</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aY">Y</asp:LinkButton></asp:TableCell>
                                                                <asp:TableCell Width="3.85%" HorizontalAlign="Center">
                                                                    <asp:LinkButton runat="server" OnClick="searchText_SelectedIndexChanged" ID="aZ">Z</asp:LinkButton></asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0" width="98%" border="0" align="center">
                                    <tr>
                                        <td style="margin-left: 10px;">
                                            
                                                    <asp:Label ID="lblRecordCount" runat="server" Font-Bold="true"></asp:Label>
                                                <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"><br />
                                            <input type="hidden" id="hidPageIndex" value="0" runat="server" />
                                            <asp:HiddenField ID="hdnEditedText" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnUpdateAlpha" runat="server" Value="1" />
                                            <asp:HiddenField ID="hdnIsSystemAdmin" runat="server" Value="1" />
                                            <input type="hidden" id="hdnDivScrollPos" runat="server" value="0" />
                                            
                                                    <div id="divOCGrammar" class="TDiv" runat="server" style="overflow-v: Auto;border:0px; width: 100%;" onscroll="document.getElementById(hdnDivScrollPosClientID).value=this.scrollTop;">
                                                        <asp:DataGrid runat="server" ID="grdOCGrammar" CssClass="GridHeader" Width="100%"
                                                            AllowSorting="true" AutoGenerateColumns="False" BorderStyle="Solid" BorderColor="LightGray"
                                                            AlternatingItemStyle-CssClass="Row3" OnEditCommand="grdOCGrammar_EditCommand"
                                                            OnItemDataBound="grdOCGrammar_ItemDataBound" OnItemCreated="grdOCGrammar_OnItemCreated"
                                                            OnUpdateCommand="grdOCGrammar_UpdateCommand" OnCancelCommand="grdOCGrammar_CancelCommand"
                                                            DataKeyField="ReferringPhysicianID" ItemStyle-Height="30" HeaderStyle-Height="25"
                                                            EnableViewState="true" OnSortCommand="grdOCGrammar_SortCommand">
                                                            <HeaderStyle CssClass="THeader" BorderColor="LightGray" BorderStyle="Solid" BorderWidth="0px">
                                                            </HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundColumn HeaderText="OCID" Visible="false" DataField="ReferringPhysicianID">
                                                                </asp:BoundColumn>
                                                                <asp:HyperLinkColumn DataNavigateUrlField="ReferringPhysicianID" DataNavigateUrlFormatString="./edit_oc.aspx?ReferringPhysicianID={0}"
                                                                    DataTextField="ReferringPhysicianDisplayName" HeaderText="Ordering Clinician"
                                                                    SortExpression="ReferringPhysicianDisplayName" ItemStyle-Width="30%"></asp:HyperLinkColumn>
                                                                <asp:TemplateColumn HeaderText="Grammar" ItemStyle-Width="60%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGrammar" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Grammar") %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtGrammar" MaxLength="500" TextMode="MultiLine" runat="server"
                                                                            Width="95%" Text='<%# DataBinder.Eval(Container, "DataItem.Grammar") %>'>
                                                                        </asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:EditCommandColumn HeaderText="Edit" UpdateText="Update" CancelText="Cancel"
                                                                    EditText="Edit" CausesValidation="true">
                                                                    <HeaderStyle Width="10%" HorizontalAlign="left" />
                                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                </asp:EditCommandColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            
                                                    <asp:Label ID="lblNorecord" runat="server" Font-Size="Small" ForeColor="green" Style="position: relative;
                                                        text-align: center;"></asp:Label>
                                                
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
     </ContentTemplate>
      <Triggers>
             <asp:AsyncPostBackTrigger ControlID="grdOCGrammar"/>
       </Triggers> 
    </asp:UpdatePanel>
</asp:Content>
