<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="DailyVisitImp.aspx.cs" Inherits="SaleWorker.DailyVisitImp" %>

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
    <form runat="server" id="frm1" class="form-horizontal">
        <div class="row" style="text-align: center">
            <h1>รายการทั้งหมดที่ต้องทำ Customer Visit</h1>
        </div>
        <br />
        <div class="form-group">
            <div class="col-sm-12">
                <asp:GridView ID="gvitem" runat="server" CssClass="table table-responsive table-hover table-bordered"
                    DataKeyNames="id" PageSize="10" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="gvitem_PageIndexChanging" OnRowCommand="gvitem_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="docno" HeaderText="docno" />
                        <asp:BoundField DataField="plandate" HeaderText="plandate"  DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="saleid" HeaderText="saleid" />
                        <asp:BoundField DataField="salename" HeaderText="salename" />
                        <asp:BoundField DataField="customerid" HeaderText="customerid" />
                        <asp:BoundField DataField="customername" HeaderText="customername" />
                        <asp:TemplateField HeaderText="Select Data">
                            <ItemTemplate>
                                <asp:Button ID="btSelectData" runat="server" Text="Select" CommandName="select" class="btn btn-info" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</asp:Content>
