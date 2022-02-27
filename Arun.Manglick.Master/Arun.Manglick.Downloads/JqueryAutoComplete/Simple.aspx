<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Simple.aspx.cs" Inherits="JqueryAutoComplete.Simple" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.3.2.js" type="text/javascript"></script>
   <script type="text/javascript">
     $(document).ready(function(){
       $("a").click(function(event){
         alert("The link will no longer take you to jquery.com");
         event.preventDefault();
       });
   });

   $(document).ready(function() {
       alert("You are welcome");
   }
   );
     
   </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="http://jquery.com/">jQuery</a>

    </div>
    </form>
</body>
</html>
