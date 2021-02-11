<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DynamicGrid.aspx.cs" Inherits="Institute_Management.DynamicGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div class="col-md-12">
    <asp:GridView ID="GridView1" runat="server" CssClass="table GridView1" AutoGenerateColumns="False"
        Width="100%" ShowFooter="true"
        BorderColor="Gray" BorderStyle="None" BorderWidth="1px" CellPadding="4"
        Font-Names="Calibri" Font-Size="Small" EmptyDataText="No Data Found..."
        ShowHeaderWhenEmpty="True">
        <FooterStyle BackColor="#FFFFCC" ForeColor="Black" />
        <HeaderStyle BackColor="#019645" Font-Bold="True" ForeColor="#FFFFCC" Wrap="False" />
        <PagerStyle BackColor="#FFFFCC" ForeColor="#019645" HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="black" HorizontalAlign="Left" VerticalAlign="Top" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />

        <Columns>

            <asp:BoundField DataField="sno" HeaderText="Serial No." />
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="name" runat="server" Text='<%#Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <asp:TextBox ID="phone" runat="server" Text='<%#Eval("Phone") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Button ID="addnew" runat="server" Text="Add New Row" OnClick="addnew_Click" />
                </FooterTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    </div>
</asp:Content>
