<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="assign_tasks.aspx.cs" Inherits="Vocada.CSTools.assign_tasks" Theme="csTool" Title="CSTools: Assign Tasks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="JavaScript" src="Javascript/common.js" type="text/JavaScript"></script>
<script language="javascript" type="text/JavaScript">
var otherPostback =false;
var mapId = "assign_tasks.aspx";
    function Navigate()
    {
        try
        {
            window.location.href = "role_task.aspx";
        }
        catch(_error)
        {
            return;
        }
    }
    function SelectAllTasks(chkBox, srclist,chkAssigned,chkBoxUnselect,clearlist)
    {       
        var list = document.getElementById(srclist);          
        var checkVal = document.getElementById(chkBox).checked;
                
        for (var i=0; i<list.options.length; i++) 
        {        
		    list.options[i].selected = checkVal;
		}
						
		var list2 = document.getElementById(clearlist);          
        document.getElementById(chkBoxUnselect).checked = false;
                
        for (var i=0; i<list2.options.length; i++) 
        {        
		    list2.options[i].selected = false;
		}
		
		if(chkAssigned == 0 && checkVal)
		{
		    document.getElementById(btnAddClientID).disabled = false;                
            document.getElementById(btnRemoveClientID).disabled = true;             
		}
		else if(chkAssigned == 1 && checkVal)
		{
		    document.getElementById(btnAddClientID).disabled = true;                
            document.getElementById(btnRemoveClientID).disabled = false;
		}
		else if(document.getElementById(chkBox).checked == false && document.getElementById(chkBoxUnselect).checked	== false)
		{
		    document.getElementById(btnAddClientID).disabled = true;                
            document.getElementById(btnRemoveClientID).disabled = true;
		}				
    }
     /*Set value for the hidden varibale which is used to check value of form data change or not*/
    function formDataChange(value)
    {
        document.getElementById(hdnDataChangedClientID).value = value;
    }

     //Check whether other record has been edited, then ask for conformation.
    function confirmOnDataChange()
    {
        if(document.getElementById(hdnDataChangedClientID).value =="true")
        {
            if(confirm("Some data has been changed, do you want to continue?"))
            {
                document.getElementById(hdnDataChangedClientID).value = "false";
                return true;
            }
            else
                return false;                    
        }
        return true;
    }
    // On Role chnage check whether form data has been changed, if yes the ask for confirmation.
                function onComboChange()
                {
                    if(confirmOnDataChange())
                    {
                        __doPostBack('ctl00$ContentPlaceHolder1$cmbrole','');
                        document.getElementById(hdnRoleClientID).value = document.getElementById(cmbroleClientID).value ;
                        return true;
                    }
                    else
                    {
                        document.getElementById(cmbroleClientID).value = document.getElementById(hdnRoleClientID).value;
                        return false;
                    }
                }
                
        function btnAdd_Click()
        {              
            var frmObj = document.getElementById(lstbAllTasksClientID);    
            var toObj = document.getElementById(lstbAttachedTasksClientID);                         
            var i = 0;            
                                    
            if(frmObj.selectedIndex >=0)
            {  
                formDataChange('true'); 
                for (i = 0; i< frmObj.length; i++)
                {                        
                    if (frmObj.options[i].selected)
                    {
                        var newOpt = document.createElement('option');
                        newOpt.text = frmObj.options[i].text;
                        newOpt.value = frmObj.options[i].value;                                                
                        toObj.add(newOpt);                                                       
                        frmObj.remove(i);
                        i--;
                    }                    
                }                
            }
            frmObj.selectedIndex = -1 ;
            toObj.selectedIndex = -1;
            document.getElementById(chkAssignedAllClientID).checked = false;
            document.getElementById(chkSelectAllTasksClientID).checked = false;
            
            document.getElementById(btnAddClientID).disabled = true;                
            document.getElementById(btnRemoveClientID).disabled = true;
    
            return false;
        }

        function btnRemove_Click()
        {            
            var frmObj = document.getElementById(lstbAllTasksClientID);    
            var toObj = document.getElementById(lstbAttachedTasksClientID);                         
            var i = 0;            
                                    
            if(toObj.selectedIndex >=0)
            {   
                formDataChange('true'); 
                for (i = 0; i< toObj.length; i++)
                {                        
                    if (toObj.options[i].selected)
                    {
                        var newOpt = document.createElement('option');
                        newOpt.text = toObj.options[i].text;
                        newOpt.value = toObj.options[i].value;                                                
                        frmObj.add(newOpt);                                                       
                        toObj.remove(i);
                        i--;
                    }                    
                }                
            }
            
            document.getElementById(btnRemoveClientID).disabled = true;                
            document.getElementById(btnAddClientID).disabled = true;
                
            frmObj.selectedIndex = -1 ;
            toObj.selectedIndex = -1;            
            document.getElementById(chkAssignedAllClientID).checked = false;
            document.getElementById(chkSelectAllTasksClientID).checked = false;
            return false;
        }

        function getAssignedTasks()
        {            
            var taskList = document.getElementById(hdTasksClientID);
            var toObj = document.getElementById(lstbAttachedTasksClientID);                         
            
            taskList.value = "";
            
            for (i = 0; i< toObj.length; i++)
            {                        
                taskList.value += toObj.options[i].value + ",";      
            }
            taskList.value = taskList.value.substring(0,taskList.value.lastIndexOf(','));            
            document.getElementById(hdnDataChangedClientID).value = "false";
        }
                
        function EnableAddOrRemove(lstSource,lstDestination,btnToEnable,btnToDisable,chkBox)
        {

           document.getElementById(btnToEnable).disabled = false; 
           document.getElementById(btnToDisable).disabled = true; 
           document.getElementById(lstDestination).selectedIndex = -1;  
           
           var selectedCnt = 0;
           var list = document.getElementById(lstSource);
            
            for (i = 0; i< list.length; i++)
            {  
                if(list.options[i].selected)
                {
                    selectedCnt++;
                }
            }
            if(selectedCnt ==  list.length)
            {
                document.getElementById(chkBox).checked = true;
            }
            else
            {
                document.getElementById(chkBox).checked = false;
            }            
        }      
</script>

<table height="100%" align="center" width="98%" border="0" cellpadding="0" cellspacing="0">        
        <tr class="ContentBg">
          <td valign="top" >
          <div style="overflow-y: Auto; height: 100%">
            <table width="100%" border="0" cellpadding="=0" cellspacing="0">
             <tr >
                <td class="Hd1">
                    Assign Tasks
                </td>
              </tr>
            </table>
            <table width="96%" border="0" align="center" cellpadding="=0" cellspacing="0">
                <tr align="center">
                 <td class="ContentBg" >
                    <asp:HiddenField id="hdnDataChanged" runat="server" Value="false" EnableViewState="true" />
                    <asp:HiddenField id="hdnRole" runat="server" Value="false" EnableViewState="true" />
                   
                    <fieldset class="fieldsetBlue" >
                      <legend class=""><b>Assign Tasks</b></legend>
                      <table id="Table2" cellspacing="1" cellpadding="2" width="100%" border="0">
                            <tr>
                                <td width="10%"></td>
                                <td width="10%"> </td>
                                <td width="20%"></td>
                                <td width="5%"></td>
                                <td Width="20%"></td>
                                <td width="20%"></td>
                            </tr>  
                            <tr>
                                <td align="right" style="padding-right:10" >Role:</td>
                                <td>                                
                                    <asp:DropDownList ID="cmbrole" runat="Server" Width="150" OnSelectedIndexChanged="cmbrole_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>                                 
                                </td>
                                <td colspan="4">&nbsp;</td>
                            </tr>
                            <tr>
                              <td colspan="6">&nbsp;</td>
                            </tr>
                            <tr>                                                               
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>   
                                <td align="center"  valign="top" >
                                    <table>
                                      <tr>
                                        <td align="left" >
                                            <asp:Label ID="lbltask" Text="Select Tasks:" runat="server" Font-Bold="false" ></asp:Label>
                                       </td>
                                       <td align="Right" nowrap>
                                            <asp:CheckBox ID="chkSelectAllTasks" runat="server" AutoPostBack="false" Height="10px" Text="Select All"/><br />
                                       </td>
                                      </tr>
                                      <tr> 
                                        <td align="Center" colspan="2">      
                                                <asp:ListBox ID="lstbAllTasks" runat="server" Width="200" Height="300" SelectionMode="Multiple" EnableViewState="true"></asp:ListBox>                                            
                                        </td>
                                      </tr> 
                                   </table>         
                                </td>
                                <td align="center" >                                
                                              <asp:Button ID="btnAdd" Text="  Add >> " runat="server" Width="80" CssClass="Frmbutton" />
                                              <br /><br />
                                              <asp:Button ID="btnRemove" Text="<< Remove" Width="80" runat="server" CssClass="Frmbutton"  />
                                </td>
                                <td>
                                    <table>
                                      <tr>
                                        <td align="left" >
                                            <asp:Label ID="lblAttachTask" Text="Assigned Tasks:" runat="server" Font-Bold="false" Height="15px"></asp:Label>
                                        </td> 
                                        <td align="right" >
                                            <asp:CheckBox ID="chkAssignedAll" runat="server" AutoPostBack="false" Height="10px" Text="Select All" /><br />
                                        </td> 
                                      <tr> 
                                        <td align="Center" colspan="2">      
                                          
                                                <asp:ListBox ID="lstbAttachedTasks" runat="server" Width="200" Height="300" SelectionMode="Multiple" EnableViewState="true"></asp:ListBox>                                          
                                        </td>
                                     </table>          
                                </td>
                                <td>&nbsp;</td>                                  
                             </tr>
                             <tr><td colspan="6">&nbsp;</td></tr>
                             <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td> <br />
                                    <td colspan="3" align="center" >
                                        <p align="center">
                                        <asp:Button id="btnSave" Text=" Save " runat="server" CssClass="Frmbutton" OnClick="btnSave_Click" OnClientClick="getAssignedTasks()" />
                                            &nbsp;&nbsp;&nbsp;
                                        <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CssClass="Frmbutton" OnClick="btnCancle_Click"  />
                                        </p>    
                                    </td>
                                    <td>&nbsp;</td> <td>&nbsp;</td><td>&nbsp;</td>
                            </tr>
                      </table>
                   </fieldset> 
                  </td> 
                 </tr>
               </table>  
             </div> 
             <asp:HiddenField id = "hdTasks" runat="server"/>
          </td>
         </tr>
</table>              
</asp:Content>

