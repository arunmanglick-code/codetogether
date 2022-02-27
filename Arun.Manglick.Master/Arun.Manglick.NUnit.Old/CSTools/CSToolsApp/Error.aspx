<%@ Page Language="C#" MasterPageFile="~/cs_tool.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Vocada.CSTools.Error" Title="CSTools: Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                                                    <asp:Label ID="lblDirectoryListHeader" runat="server" CssClass="UserCenterTitle">Error</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" width="98%" style="margin-left: 0px; margin-top: 0px;" border="0"
                                            cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" style="font-size:x-small;font-weight:bold;color:Red">An Error has occured while accessing this web page. <br /> <br />
                                                For technical support contact support@vocada.com  
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
</asp:Content>

