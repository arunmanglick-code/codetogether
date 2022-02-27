<%@ Page Language="C#" AutoEventWireup="true" CodeFile="assign_test.aspx.cs" Inherits="Vocada.CSTools.assign_test" MasterPageFile="~/cs_tool.master" Title="CSTools: Lab Test Master List" SmartNavigation="true" %>
<asp:Content ID="cpTestsAndValues" ContentPlaceHolderID="contentPlaceHolder1" runat="Server">
 <asp:UpdatePanel ID="upAddTestResult" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  <table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">   
   <tr align="center">
    <td valign="top">
     <div style="overflow-y:  Auto; height: 100%;width:100%; background-color:White">
     <table width="100%" align="center" height="100%" border="0" cellpadding="0" cellspacing="0">
      <tr style="width:100%;">
       <td valign="top">        
       <table width="100%" border="0" cellpadding="=0" cellspacing="0">
        <tr align="left">
            <td class="Hd1" style="height: 19px;width:100%;">
                <asp:Label ID="lblMessageSearchHeader" runat="server" CssClass="UserCenterTitle">Lab Test Master List</asp:Label>
            </td>
        </tr>
       </table>                
        
         <table align="center" width="98%" border="0" cellpadding="0" cellspacing="0">          
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
           <td colspan="2" align="center" style="width: 100%; height:100%;" valign="top">
            <fieldset class="fieldsetCBlue" style="width: 100%; height:100%;">
             <legend class="Hd4">Assign Lab Test(s) to Group</legend> <br />
             <div id="TestResultsDiv" class="TDiv" style="width: 97%;">
              <asp:UpdatePanel ID="upGridTestResult" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:DataGrid ID="grdTestResults" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyField="TestID"
                  CellPadding="0" CssClass="GridHeader" PageSize="200" Width="100%" BorderWidth="1px"                                      
                  OnSortCommand="grdTestResults_SortCommand" AlternatingItemStyle-CssClass="row3"
                  OnItemDataBound="grdTestResults_ItemDataBound">
               <HeaderStyle CssClass="THeader" Font-Bold="True" HorizontalAlign="Left" VerticalAlign="Middle"   Height="26"/>
               <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="center" Width="35px" />
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" onclick="CheckAll();" />
                    </HeaderTemplate>
                    <ItemStyle Height="21px" Width="35px" VerticalAlign="Middle" HorizontalAlign="Center"  />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkTest" runat="server" onclick="CheckMain();" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Full Test Name" SortExpression="TestDescription">
                    <ItemStyle Height="21px" Width="220px" />
                    <ItemTemplate>
                        <asp:Label ID="lblGridFullTestName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.TestDescription") %>' Text='<%# DataBinder.Eval(Container, "DataItem.TestDescription") %>'></asp:Label>
                    </ItemTemplate>                   
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Short Name" SortExpression="TestShortDescription">
                    <ItemStyle Height="21px" Width="120px" />
                    <ItemTemplate>
                        <asp:Label ID="lblGridShortTestName" runat="server" ToolTip='<%# DataBinder.Eval(Container, "DataItem.TestShortDescription") %>' Text='<%# DataBinder.Eval(Container, "DataItem.TestShortDescription") %>'></asp:Label>
                    </ItemTemplate>                    
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Result Type">
                    <ItemStyle Height="21px" Width="120px" />
                    <ItemTemplate>
                        <asp:Label ID="lblGridResultType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ResultType") %>'></asp:Label>
                    </ItemTemplate>                    
                </asp:TemplateColumn>              
                <asp:TemplateColumn HeaderText="Measure">
                    <ItemStyle Height="21px" Width="220px" />
                    <ItemTemplate>
                        <asp:Label ID="lblGridMeasure" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MeasurementDescription") %>'></asp:Label>
                    </ItemTemplate>                    
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Lab Test Area" SortExpression="LabTestArea">
                    <ItemStyle Height="21px" Width="165px" />
                    <ItemTemplate>
                        <asp:Label ID="lblGridTestArea" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LabTestArea") %>'></asp:Label>
                    </ItemTemplate>                    
                </asp:TemplateColumn>               				
               </Columns>
               <PagerStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                Font-Underline="False" Height="12%" HorizontalAlign="Right" />
              </asp:DataGrid>
                </ContentTemplate>                
                </asp:UpdatePanel>
             </div>
            </fieldset>
            &nbsp;
           </td>
          </tr> 
          <tr>
              <td align="center">
               <asp:UpdatePanel ID="upnlNoRecords" runat="server" UpdateMode="Always">
                  <ContentTemplate>
                      <asp:Label ID="lblNorecord" runat="server" Font-Size="Small" ForeColor="green" Style="position: relative; text-align:center;"></asp:Label>
                  </ContentTemplate> 
                </asp:UpdatePanel>      
              </td>
            </tr> 
            <tr height="40px"><td align="center" width="100%">
                <asp:Button ID="btnSave" TabIndex="24" runat="server" Text="OK" CssClass="Frmbutton" Width="69" Height="20" OnClick="btnSave_Click" Enabled="false" />
                <asp:Button ID="btnCAncel" TabIndex="25" runat="server" Text="Cancel" CssClass="Frmbutton" Width="69" Height="20" OnClick="btnCancel_Click" /></td>
                <asp:HiddenField ID="hdnTestIds" runat="server" />
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
</asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
    var mapId = "assign_test.aspx" ;
    function CheckAll()
    {                
        var i=0;
        var chkBoxId = "";
        var arrList = document.getElementsByTagName("input");                
        var checkAll = false;
        
        for (i=0 ; i< arrList.length; i++)
        {
            chkBoxId = arrList[i].id.toString();                              
            
            if(chkBoxId.indexOf("chkAll") != -1)
            {
                checkAll = arrList[i].checked;
            }
            
            if (chkBoxId.indexOf("chkTest") != -1)
            {                
                arrList[i].checked = checkAll;                
            }
        }
        
        if(checkAll)
        {
            document.getElementById(btnSaveClientId).disabled=false;
        }
        else
        {
            document.getElementById(btnSaveClientId).disabled=true;
        }
        
    }
    
    function CheckMain()
    {
        var i=0;
        var chkBoxId = "";
        var arrList = document.getElementsByTagName("input");                
        var chkAllChkBox;
        var chkTestCnt = 0;
        var chkTestCheckedCnt = 0;
        
        for (i=0 ; i< arrList.length; i++)
        {
            chkBoxId = arrList[i].id.toString();                              
            
            if(chkBoxId.indexOf("chkAll") != -1)
            {
                chkAllChkBox = arrList[i];
            }
            
            if (chkBoxId.indexOf("chkTest") != -1)
            {         
                chkTestCnt++;
                if(arrList[i].checked)
                {
                    chkTestCheckedCnt++;
                }                                
            }                        
        }
        
        if(chkTestCheckedCnt > 0)
        {
            document.getElementById(btnSaveClientId).disabled=false;
        }
        else
        {
            document.getElementById(btnSaveClientId).disabled=true;
        }
        
        if(chkTestCheckedCnt == chkTestCnt)
        {
            chkAllChkBox.checked = true;
        }
        else
        {
            chkAllChkBox.checked = false;
        }        
    }
    
    function isNumericKeyStroke()
    {
     var returnValue = false;
     var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

     if ( ((keyCode >= 48) && (keyCode <= 57)) || // All numerics
               (keyCode ==  8) ||     // Backspace
               (keyCode == 13) ||
               (keyCode ==  9) ||
               (keyCode == 46) || // delete key
               (keyCode == 16) || // shift key
               (keyCode == 17) || // ctrl key
               (keyCode == 35) || //End key
               (keyCode == 36) || //Home key
               (keyCode == 37) || // Left
               (keyCode == 39) || // right
               ((keyCode >= 96) && (keyCode <= 105)))     // Number pad numbers
             returnValue = true;

     if ( window.event.returnValue )
      window.event.returnValue = returnValue;

     return returnValue;
    }
    // JS Code : For editing Row inside the Grid
    // if (document.all.editedRowClientID != null) { document.all.editedRowClientID.scrollIntoView(); }
    </script>
</asp:Content>