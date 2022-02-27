<%@ Page Language="c#" Inherits="Vocada.CSTools.VCPopUP" CodeFile="VCPopUP.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>CSTools</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <script language="javascript">

            function OnPopUpLoad()
			{		  
			    var vUrl;	
				vUrl = window.dialogArguments.URL;				
				document.getElementById("idFrame").src = vUrl;		
			    if(vUrl.search(/actual_message/)!=-1)
				    window.document.title="CSTools: Notification Message";							
				else if(vUrl.search(/message_center/)!=-1)
				    window.document.title="CSTools: Search";		
				else
				  window.document.title= "CSTools";
			}
    </script>

</head>
<body onload="OnPopUpLoad()">
    <iframe id="idFrame" src="frmVC.aspx" width="100%" height="100%" frameborder="yes"
        marginheight="0" marginwidth="0" scrolling="no" bordercolor="red"></iframe>
</body>
</html>
