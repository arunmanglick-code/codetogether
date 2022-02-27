<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Trial3.aspx.cs" Inherits="Trial3" Title="Untitled Page" %>

<%@ Register Src="../UserControls Cafe/UserControls/Information.ascx" TagName="Information" TagPrefix="uc1" %>
<%@ Register src="../UserControls Cafe/UserControls/SearchPagination.ascx" tagname="SearchPagination" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../JS/JScript.js" type="text/javascript"></script>

    <script src="../JS/File1.js" type="text/javascript"></script>

    <script type="text/javascript">

        

        function AlertMe() 
        {
            alert('Hello');
        }

        function ClientSideClick() {
        }


    </script>

    <script type="text/javascript">
    </script>

    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/MyTheme/Style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <table border="0" width="70%" cellpadding="1" cellspacing="3">
        <tbody>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="Hidden" align="left" colspan="3">
                    <asp:Button ID="Button1"  runat="server" CssClass="button" OnClientClick="AlertMe();" Text="Button" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBox2"  runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="Hidden" align="left" colspan="3">
                    
                </td>
            </tr>
            <tr>
                <td>
                     <asp:TextBox ID="TextBox3"  runat="server"></asp:TextBox>
                </td>
            </tr>  
            <tr>
                <td>
                    <asp:DropDownList ID="DropDownList1"  runat="server">
                        <asp:ListItem>aa</asp:ListItem>
                        <asp:ListItem>bb</asp:ListItem>
                        <asp:ListItem>cc</asp:ListItem>
                        <asp:ListItem>dd</asp:ListItem>
                        <asp:ListItem>ee</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>   
            <tr>
                <td>
                    <asp:CheckBoxList ID="CheckBoxList1" Width="200px"  runat="server">
                        <asp:ListItem>aa</asp:ListItem>
                        <asp:ListItem>bb</asp:ListItem>
                        <asp:ListItem>cc</asp:ListItem>
                        <asp:ListItem>dd</asp:ListItem>
                        <asp:ListItem>ee</asp:ListItem>
                        <asp:ListItem>ff</asp:ListItem>
                        <asp:ListItem>gg</asp:ListItem>
                        <asp:ListItem>hh</asp:ListItem>
                        <asp:ListItem>ii</asp:ListItem>
                        <asp:ListItem>jj</asp:ListItem>
                        <asp:ListItem>kk</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>  
            <tr>
                <td>
                    <asp:ListView ID="ListView1" runat="server">
                     <LayoutTemplate>
                      <table>
                       <tr runat="server" ID="itemPlaceholder"></tr>
                      </table>
                     </LayoutTemplate>
                     <ItemTemplate>
                      <tr>
                          <td>
                            <asp:CheckBox ID="CheckBox1" Text='<%# Eval("Device")%>'  runat="server" />

                          </td>
                      </tr>
                     </ItemTemplate>
                    </asp:ListView>

                    
                </td>
            </tr> 
        </tbody>
    </table>
    
     <fieldset class="fieldsetCBlue" style="width: 49.7%; height:200px;>
        <legend class="">
            <asp:Label ID="lblSection4" runat="server" Text="Hello"></asp:Label>
            
            <asp:CheckBoxList ID="CheckBoxList2" DataTextField="Device" DataValueField="Active"  runat="server">
            </asp:CheckBoxList>
        </legend>
     </fieldset>
     
     <br /><br /><br />
     
     <%--<table width="50%" border="1">
     <tr>
         <td rowspan="2">
            <fieldset style="width:90%; height:200px;">
                <legend class="">
                    <asp:Label ID="Label1" runat="server" Text="Hello"></asp:Label>
                </legend>
             </fieldset>
         </td>
         <td>
            <fieldset style="width:90%; height:95px;">
                <legend class="">
                    <asp:Label ID="Label2" runat="server" Text="Hello"></asp:Label>
                </legend>
             </fieldset>
         </td>
     </tr>
     <tr>
         <td>
            <fieldset style="width:90%; height:90px;">
                <legend class="">
                    <asp:Label ID="Label3" runat="server" Text="Hello"></asp:Label>
                    <asp:TextBox ID="TextBox4" MaxLength="3" runat="server"></asp:TextBox>
                </legend>
             </fieldset>
         </td>
     </tr>
     </table>--%>
</asp:Content>
