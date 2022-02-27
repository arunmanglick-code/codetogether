<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UseMasterSimple.aspx.cs" Inherits="AJAXPostImplmentation_Master_Pages_UseMasterSimple"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
function SelectAll()
	{
		var name="ctl00_ContentPlaceHolder1_GridView1_ctl";
		var x=0;
		var i=2;
		var cons =0;
		do
		{
			if(i <= 9)
			{
			    oElement = document.getElementById(name + cons + i + "_CheckBox1");
			}
			else
			{
			    oElement = document.getElementById(name + i + "_CheckBox1");
			}
			
			if (oElement == null)
				{x=1;}
			else
			{oElement.checked=document.getElementById("ctl00_ContentPlaceHolder1_GridView1_ctl01_CheckBoxAll").checked;}
			
			i++;
		}while(x<1);
	}

function Check()
{
    if(CheckDelete())
    {
        return window.confirm('Are u Sure');        
    }
    else
    {
        window.alert('Please Select Checkbox');
        return false;
    }
    
}	
function CheckDelete()
{
        var name="ctl00_ContentPlaceHolder1_GridView1_ctl";
        var x=0;
		var i=2;
		var cons =0;
		do
		{
			if(i <= 9)
			{
			    oElement = document.getElementById(name + cons + i + "_CheckBox1");
			}
			else
			{
			    oElement = document.getElementById(name + i + "_CheckBox1");
			}
			
			if (oElement == null)
			{
				x=1;				
			}
			else
			{
			   if(oElement.checked)
			   {
			        return true;
			   }
			}
			
			i++;
		}while(x<1);
		
		return false;
}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset>
                <legend>Single Update Panel</legend>Last refresh
                <%=DateTime.Now.ToString() %><br />
                <asp:Button ID="Button1" runat="server" Text="Show Grid" OnClick="Button1_Click" />
                <asp:Button ID="Button2" runat="server" Text="Delete Selected" OnClientClick="return Check();" OnClick="Button1_Click" />
                <br />
                <br />
                <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CC9966"
                    BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="False"
                    AllowSorting="True">
                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                    <RowStyle BackColor="White" ForeColor="#330099" />
                    <Columns>                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckBoxAll" onclick="SelectAll();" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1"  runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="CourseId" SortExpression="CourseId" HeaderText="CourseId" />
                        <asp:BoundField DataField="Sequence" HeaderText="Sequence" />
                        <asp:BoundField DataField="Institution" HeaderText="Institution" />
                        <asp:BoundField DataField="Course" HeaderText="Course" />
                        <asp:TemplateField HeaderText="Average on Label">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Average") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                </asp:GridView>
                
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
