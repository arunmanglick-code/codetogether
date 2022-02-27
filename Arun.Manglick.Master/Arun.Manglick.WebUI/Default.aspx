<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="expires" content="3" />
    <script language="javascript" type="text/javascript">
    
        Shoot();
        function ShootOld()
        {              
            window.opener=self;
            id = window.open('Home.aspx','','fullscreen=no,menubar=no,scrollbars=yes,status=yes,titlebar=no,toolbar=no,resizable=yes,location=no,top=0,left=0');
            self.close();  // http://www.interwebby.com/blog/2006/02/04/3/
            
        }
        
        function Shoot()
        {               
            var str = "left=0,screenX=0,top=0,screenY=0";   
            if (window.screen) 
            {
              var ah = screen.availHeight - 30;
              var aw = screen.availWidth - 10;
              str += ",height=" + ah;
              str += ",innerHeight=" + ah;
              str += ",width=" + aw;
              str += ",innerWidth=" + aw;
              str += ",resizable,scrollbars=yes";
            } 
            else 
            {
              str += ",resizable"; // so the user can resize the window manually
            }
            window.opener=self;
            var win = window.open("Home.aspx", "full", str);
            window.opener.close();
            win.focus();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
