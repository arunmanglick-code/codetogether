<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CustomValidation1.aspx.cs" Inherits="CustomValidation1" Title="Custom Validation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

<script type="text/javascript">

function CheckMoveRowSelectionValidator(source, args)
    {
        //debugger;
        var txtMove = document.getElementById('<%= txtMoveRows.ClientID %>');
        var requiredMsg = 'Please enter some value';
        var regExMesg = '<%=GetGlobalResourceObject("ErrorMessages", "PositiveRowIndex")%>';
		var lbl = document.getElementById('<%= lblError.ClientID %>');
		var lblNo = document.getElementById('<%= lblNoError.ClientID %>');
        
      
        if(txtMove.value == '')
	    {
		    txtMove.focus();
		    source.errormessage = requiredMsg;
            args.IsValid = false;
            lblNo.innerHTML = '';
	    }
	    else if(txtMove.value !='')
	    {
		    var str=txtMove.value;	
		    var regexp=/^0*[1-9]+\d*$/;
		    result=regexp.test(str);
		    if (!result)
		    {
			   txtMove.focus();
               source.errormessage = regExMesg;   
               args.IsValid =false;
               lblNo.innerHTML = '';
		    }
		    else
		    {
		        args.IsValid =true;
		    }
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
                <asp:Label ID="lblHeader" runat="server" Text="Your Text"></asp:Label></td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="validation-no-error">
                <asp:Label ID="lblNoError" runat="server" EnableViewState="false" CssClass="validation-no-error"></asp:Label>
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
                            <li>Usage of Custom Validator</li>
                            <li>Notice the use of 'ValidateEmptyText' property.</li>
                            <li>This property is required, otherwise CustomValidator bydefault, does not fire even if the value is blank</li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                    
                    <table border="0">
                        <tr>
                            <td align="left" class="gridcell">
                                <asp:Label ID="lblMoveRows" runat="server" Text="Move Counter"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:CustomValidator ID="ctvMoveRows" ControlToValidate="txtMoveRows"  ValidateEmptyText="true"
                                    runat="server" Text="<img src='../Images/error-icon.gif' />" Display="Dynamic" ClientValidationFunction="CheckMoveRowSelectionValidator"
                                    ErrorMessage=""></asp:CustomValidator>
                            </td>
                            <td align="left" class="gridcell">
                                <asp:TextBox ID="txtMoveRows" runat="server" class="inputfield"  MaxLength="4" Width="12px"></asp:TextBox>
                            </td>
                            <td align="left" class="gridcell">
                                <asp:Button ID="btnMove" runat="server" class="button" Text="Move" OnClick="btnMove_Click" />
                            </td>
                        </tr>
                    </table>
                    
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
