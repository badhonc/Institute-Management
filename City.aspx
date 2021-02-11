<%@ Page Title="City" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="City.aspx.cs" Inherits="Institute_Management.City" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <%--Message Area--%>
        <div class="col-md-12 mt-1">
            <%--message area--%>

            <div class="alert alert-success alert-dismissible" id="success_area" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <strong>Success!</strong>
                <asp:Label ID="success_msg" runat="server"></asp:Label>
            </div>

            <div class="alert alert-danger alert-dismissible" id="error_area" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <strong>Danger!</strong>
                <asp:Label ID="error_msg" runat="server"></asp:Label>
            </div>
            <%--message area--%>
        </div>

    <%--Message Area--%>

<div class="col-md-12">
    <div class="card">
        <div class="card-header">
            <p class="text-bold text-primary">Add City</p>
        </div>
        <div class="card-body">
            
            <div class="form-group">
                <label for="text">Country:</label>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    AppendDataBoundItems="true"
                    CssClass="form-control select2" required="required"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="text">City:</label>
               <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Input City Name"
                   ToolTip="City" required="required"></asp:TextBox>
            </div>


            <asp:Button ID="Button1" CssClass="btn btn-default form-control insert-btn" BackColor="#019645" ForeColor="White" runat="server" Text="Add..." OnClick="Button1_Click" />
            <asp:Button ID="Button2" CssClass="btn btn-default insert-btn form-control" BackColor="#019645" ForeColor="White" runat="server" Text="Update..." OnClick="Button2_Click" />
        </div>
    </div>
</div>

<div class="col-md-12">
    <div class="card">
        <div class="card-header">
            <p class="text-bold text-primary form-inline mb-3">

                <span class="pull-left col-md-4" style="width:100%">
                    City List
                </span>
                <span class="col-md-4">
                    <asp:DropDownList ID="DropDownList2" AppendDataBoundItems="true"
                    CssClass="form-control select2" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"></asp:DropDownList>
                </span>
                <span class="pull-right col-md-4">
                    <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="true"
                        CssClass="form-control" Width="100%" placeholder="Search by code or city" OnTextChanged="TextBox2_TextChanged"
                        ></asp:TextBox>
                </span>
            </p>
            <br />
            <br />

            <div class="row">
                <div class="col-md-5">
                    <div class="form-group date">
                        <input name="fromdate" style="z-index: 0" runat="server" 
                            placeholder="YYYY-MM-DD" value="" type="text" 
                            class="form-control datepicker2" readonly="readonly"
                            required="required" id="fromdate" autocomplete="off">
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form-group date">
                        <input name="todate" style="z-index: 0" runat="server" 
                            placeholder="YYYY-MM-DD" value="" type="text" 
                            class="form-control datepicker3" readonly="readonly"
                            required="required" id="todate" autocomplete="off">
                    </div>
                </div>

                <div class="col-md-2">
                    <div class="form-group">
                        <asp:LinkButton ID="LinkButton1" CssClass="form-control btn btn-block btn-primary" runat="server" OnClick="LinkButton1_Click">
                            Search...
                        </asp:LinkButton>
                    </div>
                </div>


            </div>
            
            

            
        <div class="card-body">
            <asp:GridView ID="GridView1" runat="server" CssClass="table GridView1" AutoGenerateColumns="False"
                            Width="100%"
                            BorderColor="Gray" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="id"
                            Font-Names="Calibri" Font-Size="Small" EmptyDataText="No Data Found..."
                            ShowHeaderWhenEmpty="True">
                            <FooterStyle BackColor="#FFFFCC" ForeColor="Black" />
                            <HeaderStyle BackColor="#019645" Font-Bold="True" ForeColor="#FFFFCC" Wrap="False" />
                            <PagerStyle BackColor="#FFFFCC" ForeColor="#019645" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="black" HorizontalAlign="Left" VerticalAlign="Top" />
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />

                <Columns>




                    <asp:BoundField DataField="row_number" HeaderText="#" />
                    <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp; Action &nbsp;&nbsp;&nbsp;&nbsp;">

                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEdit" CssClass="insert-btn" CommandArgument='<%# Eval("id") %>' runat="server" OnClick="BtnEdit_Click">
                                            <span class="btn btn-xs btn-warning"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="BtnDelete" CommandArgument='<%# Eval("id") %>' runat="server" OnClick="BtnDelete_Click" OnClientClick="if ( ! UserDeleteConfirmation()) return false;">
                                            <span class="btn btn-xs btn-danger"><i class="fa fa-trash-o" aria-hidden="true"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </asp:TemplateField>

                    <asp:BoundField DataField="code" HeaderText="Code" />
                    <asp:BoundField DataField="name" HeaderText="City" />
                    <asp:BoundField DataField="country" HeaderText="Country" />
                    <asp:BoundField DataField="created_at" HeaderText="Date" />

                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</div>

</asp:Content>
