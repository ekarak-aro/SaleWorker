<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="AddLoneItem.aspx.cs" Inherits="SaleWorker.AddLoneItem" EnableEventValidation="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
        }

        .modal-body {
            max-height: 500px;
            overflow: auto;
        }
    </style>

    <form runat="server" role="form" class="form-horizontal" id="form1">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class="page-header text-center">
            <h1 id="Header">โปรแกรมแจ้งยืมสินค้าที่มีอยู่ใน Stock</h1>
        </div>
        <div class="row">
            <asp:Label ID="Label1" runat="server" Text="ชื่อบริษัท : " class="col-sm-2 control-label"></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="col-sm-2 col-lg-2 col-md-2">
                        <asp:TextBox ID="tbCusCode" runat="server" class="form-control" placeholder="Customer Code" required="required"></asp:TextBox>
                    </div>
                    <asp:Button ID="btSearchCust" runat="server" Text="Search Customer" class="btn btn-default btn-default col-sm-2" OnClick="Onclick" />
                    <div class="col-sm-3 col-lg-3 col-md-3">
                        <asp:Label ID="lbCustName" runat="server" Text="Customer Des" class="control-label"></asp:Label>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btSearchCust" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="row">
            <asp:Label ID="Label2" runat="server" Text="ผู้ขอยืมสินค้า : " class="col-sm-2 control-label"></asp:Label>
            <div class="col-lg-4 col-md-4 col-sm-4">
                <asp:DropDownList ID="ddlSale" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <br />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Item </legend>
            <button class="btn btn-primary btn-lg" data-toggle="modal" data-target="#modalItem">
                Add Item
            </button>

            <!-- Modal -->
            <div class="modal fade" id="modalItem" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content ">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="myModalLabel">Search Item</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlSearchTypeItem" runat="server" CssClass="form-control">
                                        <asp:ListItem Selected="True">ItemCode</asp:ListItem>
                                        <asp:ListItem>ItemDesc</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="tbItemSearch" runat="server" CssClass="form-control col-lg-5 col-md-5 col-sm-5"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-sm-4">
                                    <label class="control-label">Location :</label>
                                </div>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                                        <asp:ListItem Selected="True">HO</asp:ListItem>
                                        <asp:ListItem>RAYONG</asp:ListItem>
                                        <asp:ListItem>CHIANG</asp:ListItem>
                                        <asp:ListItem>PHUKET</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="text-align: center">
                                <asp:Button ID="btSearchItem" runat="server" Text="Search Item" CssClass="btn btn-primary btn-sm" OnClick="btSearchItem_Click" />
                            </div>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <asp:GridView ID="gvSearchItem" runat="server" class="table table-bordered table-hover table-condensed table-responsive"
                                            AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gvSearchItem_RowCommand" OnPageIndexChanging="gvSearchItem_PageIndexChanging"
                                            PagerSettings-Position="TopAndBottom" PagerSettings-Mode="NumericFirstLast">
                                            <Columns>
                                                <asp:BoundField DataField="itemno" HeaderText="Item No" />
                                                <asp:BoundField DataField="descrip" HeaderText="Item Description" ItemStyle-Width="10%" ItemStyle-Wrap="true"/>
                                                <asp:BoundField DataField="uom" HeaderText="Uom" ItemStyle-Width="30%"/>
                                                <asp:BoundField DataField="price" HeaderText="price" />
                                                <asp:BoundField DataField="stockUnit" HeaderText="stockUnit" DataFormatString="{0:F0}" />
                                                <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" />
                                                <asp:TemplateField HeaderText="Select Data">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btSelectData" runat="server" Text="Select" CommandName="select" class="btn btn-info" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NextPrevious" Position="TopAndBottom" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btSearchItem" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btCloseItem" runat="server" Text="Close" CssClass="btn btn-default" OnClick="btCloseItem_Click" />
                            <!--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>-->
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
            <br />
            <h2></h2>

            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-12 col-lg-12 col-md-12">
                            <asp:GridView ID="gvItem" runat="server" CssClass="table table-striped table-bordered table-condensed table-responsive" PageSize="5" AutoGenerateColumns="False"
                                ShowFooter="True" ForeColor="#333333" OnRowDeleting="gvItem_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Col1" HeaderText="Item Code" />
                                    <asp:BoundField DataField="Col2" HeaderText="Item Desc" ItemStyle-Width="30%" ItemStyle-Wrap="true"/>
                                    <asp:TemplateField HeaderText="UOM" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-lg-6 col-md-6 col-sm-6">
                                                    <asp:TextBox ID="tbGridViewUom" runat="server"  Text='<%#Eval("UOM") %>'></asp:TextBox>
                                                </div>
                                            </div>                                                                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Col3" HeaderText="Stock Unit" />
                                    <asp:BoundField DataField="Col4" HeaderText="Price" />
                                    <asp:BoundField DataField="LOCATION" HeaderText="LOCATION" />
                                    <asp:TemplateField HeaderText="TotalPrice">
                                        <FooterTemplate>
                                            <asp:Button ID="btCalculate" runat="server" Text="Calculate" OnClick="btCalculate_Click" CssClass="btn btn-warning" />
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbGridViewTotalPrice" runat="server" Text='<%#Eval("TotalPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" ButtonType="Button" ControlStyle-CssClass="btn btn-info">
                                        <ControlStyle CssClass="btn btn-info" />
                                    </asp:CommandField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="#C2D69B" />
                                <HeaderStyle BackColor="Green" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>

            </asp:UpdatePanel>
        </fieldset>
        <div class="row" style="text-align: center">
            <asp:Button ID="btSendData" runat="server" Text="Send Data" class="btn btn-success" OnClick="btSendData_Click" />
        </div>
        <br />
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <p class="lead">
                        You Credit :
                        <asp:Label ID="lbCredit" runat="server" Text="0"></asp:Label>
                    </p>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btSearchCust" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div class="row">
                    <p class="lead">
                        Total Item :
                    <asp:Label ID="Label4" runat="server" Text="0"></asp:Label>
                    </p>
                </div>
                <br />
                <div class="row">
                    <p class="lead">
                        Total Price :
                    <asp:Label ID="Label3" runat="server" Text="0"></asp:Label>
                    </p>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvItem" />
            </Triggers>
        </asp:UpdatePanel>



    </form>

    <script>
        $(document).ready(function () {
            $('#' + '<%= tbCusCode.ClientID %>').css('border', '3px solid blue');
        });
    </script>
</asp:Content>
