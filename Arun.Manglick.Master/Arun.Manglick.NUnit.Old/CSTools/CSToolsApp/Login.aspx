<%@ Page Language="c#" Inherits="Vocada.CSTools.UI.login" CodeFile="login.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
 <title>CSTools: Login</title>
 <meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
 <meta content="C#" name="CODE_LANGUAGE">
 <meta content="JavaScript" name="vs_defaultClientScript">
 <link href="App_Themes/csTool/cstool.css" type="text/css" rel="stylesheet">
</head>

<script language="javascript">
	
	    function RemoveErrorMessage()
	    {
	      if(document.getElementById('txtUsername').value == "")
	      {
	        document.getElementById('txtUsername').focus();
	      }
	      else if(document.getElementById('txtPassword').value == "")
	      {
	        document.getElementById('txtPassword').focus();
	      }	      
	       
	      if(document.getElementById('lblErrorMessage').innerText.length != 0)
	       { 
	            document.getElementById('lblErrorMessage').innerText = "";
	       }
	    }
	    function RefreshParent()
	    {
	        
	        if (window.parent.frames.length == 2)
	        {
	            window.parent.location.href = 'login.aspx';
	        }
	    }
	    
</script>

<body onload="RefreshParent()">
 <form id="frmLogin" target="_top" method="post" runat="server">
  <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
   <tr>
    <td valign="middle">
     <p>
      &nbsp;</p>
     <p>
      &nbsp;</p>
     <p>
      &nbsp;</p>
     <table width="70%" height="70%" border="0" align="center" cellpadding="0" cellspacing="1">
      <tr>
       <td bgcolor="#FFFFFF" style="width: 37%; height: 373px">
        <div align="center">
         <p>
          <img src="./img/logo.gif" width="245" height="57"></p>
         <p>
          <br>
          &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
          &nbsp;
          <img src="./img/phy.jpg" width="192" height="119"></p>
        </div>
       </td>
       <td class="LoginGrd" style="padding-top: 70px; width: 63%; height: 373px;">
        &nbsp;<table width="95%" border="0" align="center" cellpadding="3" cellspacing="2"
         class="LoginTxt" style="left: 0px; position: relative; top: 22px">
         <tr>
          <td colspan="2" class="LoginTxtHd">
           Log in here.</td>
         </tr>
         <tr>
          <td width="28%" class="LoginTxt">
           User ID</td>
          <td width="72%">
           <asp:TextBox ID="txtUsername" TabIndex="1" runat="server" Width="168px" MaxLength="25"></asp:TextBox>
          </td>
         </tr>
         <tr>
          <td class="LoginTxt" style="height: 25px">
           Password</td>
          <td style="height: 25px">
           <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" TextMode="Password" Width="168px"
            MaxLength="20" OnFocus="RemoveErrorMessage();"></asp:TextBox>
          </td>
         </tr>
         <tr>
          <td nowrap class="LoginTxt" style="height: 25px">
           Remember Me</td>
          <td style="height: 25px">
           <asp:CheckBox ID="cbRememberMe" TabIndex="3" runat="server" CssClass="Rdinput" BorderWidth="0px">
           </asp:CheckBox>
          </td>
         </tr>
         <tr>
          <td nowrap class="LoginTxt">
           &nbsp;</td>
          <td>
           <asp:ImageButton ID="btnLogin" runat="server" onmouseover="this.src='./img/bu_login_over.gif'"
            Height="23px" ImageUrl="./img/bu_login.gif" OnClick="btnLogin_Click" onmouseout="this.src='./img/bu_login.gif'"
            TabIndex="4" OnClientClick="RemoveErrorMessage();" Width="102px" CssClass="Rdinput" /></td>
         </tr>
         <br />
         <br />
         <tr>
          <td nowrap class="LoginTxt">
           &nbsp;</td>
          <td>
           <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUsername"
            ErrorMessage="Please enter a User ID" Width="158px" Font-Names="Verdana" Font-Size="8pt">Please enter a User ID</asp:RequiredFieldValidator>
           <asp:Label ID="lblErrorMessage" runat="server" Width="306px" ForeColor="Red" Font-Names="Verdana"
            Font-Size="7pt"></asp:Label>
           <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
            ErrorMessage="Please enter a Password" Width="149px" Font-Names="Verdana" Font-Size="8pt">Please enter a Password</asp:RequiredFieldValidator>
          </td>
         </tr>
         <tr>
          <td nowrap class="LoginTxt">
           &nbsp;</td>
          <td>
          </td>
         </tr>
        </table>
       </td>
      </tr>      
     </table>
     <table border="0" width="100%">
    <tr valign="bottom" height="20">
    <td width="20%">&nbsp;</td>
                     <td class="CopyRight">
                        © 2008 Nuance, Inc. All rights reserved. Portions of Veriphy covered under US Patent
                        6,778,644- CSTools 3.0 (101)
                    </td>            
                </tr>
    </table>
    </td>    
   </tr>   
  </table>
 </form>
</body>
</html>
