﻿<%@ Page Title="Purchase" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Purchase.aspx.cs" Inherits="Institute_Management.Purchase" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">



    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
                        <p class="text-bold text-primary">Select Product</p>
                    </div>
                    <div class="card-body">

                        <div class="form-group">
                            <label for="text">Category:</label>
                            <asp:DropDownList ID="DropDownList1" runat="server"
                                AutoPostBack="true"
                                CssClass="form-control select2 DropDownList1" required="required"
                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="text">SubCategory:</label>
                            <asp:DropDownList ID="DropDownList2" runat="server"
                                AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="form-control select2 DropDownList2" required="required"
                                OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="text">MainCategory:</label>
                            <asp:DropDownList ID="DropDownList3" runat="server"
                                AppendDataBoundItems="true"
                                CssClass="form-control select2" required="required">
                            </asp:DropDownList>
                        </div>


                        <div class="form-group">
                            <label for="text">Product:</label>
                            <asp:TextBox ID="TextBox1" runat="server"
                                CssClass="form-control" ToolTip="Product" placeholder="Input Product Name"
                                required="required"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="text">Price:</label>
                            <asp:TextBox ID="TextBox3" runat="server"
                                CssClass="form-control" ToolTip="Price" placeholder="Input Price"
                                required="required"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="text">Quantity:</label>
                            <asp:TextBox ID="TextBox4" runat="server"
                                CssClass="form-control" ToolTip="Quantity" placeholder="Select Quantiy"
                                required="required"></asp:TextBox>
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

                            <span class="pull-left col-md-2" style="width: 100%">Product List
                            </span>
                            <span class="col-md-2">
                                <asp:DropDownList ID="DropDownList4" AppendDataBoundItems="true"
                                    CssClass="form-control select2 DropDownList4" Width="100%" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DropDownList4_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>

                            <span class="col-md-2">
                                <asp:DropDownList ID="DropDownList5" AppendDataBoundItems="true"
                                    CssClass="form-control select2 DropDownList5" Width="100%" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DropDownList5_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>

                            <span class="col-md-2">
                                <asp:DropDownList ID="DropDownList6" AppendDataBoundItems="true"
                                    CssClass="form-control select2 DropDownList6" Width="100%" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DropDownList6_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                            <span class="pull-right col-md-4">
                                <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="true"
                                    CssClass="form-control TextBox2" Width="100%" placeholder="Search by code or Product"
                                    OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
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
                                    <asp:LinkButton ID="LinkButton1" CssClass="form-control btn btn-block btn-primary LinkButton1" runat="server" OnClick="LinkButton1_Click">
                                Search...
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="card-body">

                            <asp:GridView ID="GridView1" runat="server" CssClass="table GridView1" AutoGenerateColumns="False"
                                Width="100%" ShowFooter="true"
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
                                    <asp:TemplateField HeaderText="&nbsp;&nbsp; Action &nbsp;&nbsp;">

                                        <ItemTemplate>
                                            <asp:LinkButton ID="BtnEdit" CssClass="insert-btn" CommandArgument='<%# Eval("id") %>' runat="server" OnClick="BtnEdit_Click">
                                            <span class="btn btn-xs btn-warning"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="BtnDelete" CommandArgument='<%# Eval("id") %>' runat="server" OnClick="BtnDelete_Click" OnClientClick="if ( ! UserDeleteConfirmation()) return false;">
                                            <span class="btn btn-xs btn-danger"><i class="fa fa-trash-o" aria-hidden="true"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" Text="Total Price"></asp:Label>
                                            <asp:TextBox ID="tprice" runat="server" Text="" /><br />
                                            <asp:Label runat="server" Text="Total Quantity"></asp:Label>
                                            <asp:TextBox ID="tquantity" runat="server" Text="" />
                                        </FooterTemplate>

                                    </asp:TemplateField>

                                    <asp:BoundField DataField="code" HeaderText="Code" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" />
                                    <asp:BoundField DataField="SubCategory" HeaderText="SubCategory" />
                                    <asp:BoundField DataField="MainCategory" HeaderText="MainCategory" />
                                    <asp:BoundField DataField="name" HeaderText="Product" />
                                    <asp:BoundField DataField="created_at" HeaderText="Date" />
                                    <asp:BoundField DataField="" HeaderText="Quantity" />
                                       
                                    <asp:BoundField DataField="" HeaderText="Price" />

                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>



        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DropDownList1" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DropDownList2" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DropDownList4" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DropDownList5" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="DropDownList6" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="LinkButton1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TextBox2" EventName="TextChanged" />




        </Triggers>

    </asp:UpdatePanel>

    <script type="text/javascript">

        //System.Application.add_load(initdropdown);
        $(function () {
            SelectAndDatapicker();
            Click();
            textchange();
        })
        function SelectAndDatapicker() {
            // select 2 code
            $('.select2').select2();
            $('.select3').select2();
            $('.select4').select2();
            $('.select5').select2();
            $('.select6').select2();

            // date pikcker 

            $('.datepicker').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true
            });

            $('.datepicker1').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true
            });

            $('.datepicker2').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true
            });

            $('.datepicker3').datepicker({
                language: 'en',
                format: 'yyyy-mm-dd',
                autoclose: true
            });

            $('#datepicker4').datepicker({
                format: 'yyyy-mm-dd hh:mm',
                autoclose: true
            });
        }
        function Click() {
            $('.LinkButton1').click(function () {
                $('.loading').show();
                return true;
            });
        }
        function textchange() {
            $('.TextBox2').change(function () {
                $('.loading').show();
                return;
            });
        }
    </script>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(".loading").hide();
            SelectAndDatapicker();
            Click();
            textchange();

            $('.DropDownList1').change(function () {
                $(".loading").show();
                return;
            });

            $('.DropDownList2').change(function () {
                $(".loading").show();
                return;
            });

            $('.DropDownList4').change(function () {
                $(".loading").show();
            });

            $('.DropDownList5').change(function () {
                $(".loading").show();
            });

            $('.DropDownList6').change(function () {
                $(".loading").show();
            });

            $(".insert-btn").click(function () {
                $(".loading").show();
                return;
            });

        }
    </script>
</asp:Content>

