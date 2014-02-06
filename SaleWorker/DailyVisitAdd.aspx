<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="DailyVisitAdd.aspx.cs" Inherits="SaleWorker.DailyVisitAdd" ClientIDMode="Static" %>

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
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Weeklyplan No :</label>
                <asp:TextBox ID="tbWeeklyPlanNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <label class="control-label">Plan Date :</label>
                <asp:TextBox ID="tbPlanDate" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Sale ID :</label>
                <asp:TextBox ID="tbSaleId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <label class="control-label">Sale Name :</label>
                <asp:TextBox ID="tbSaleName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Type Customer :</label>
                <asp:TextBox ID="tbTypeCustomer" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Customer ID :</label>
                <asp:TextBox ID="tbCustId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <label class="control-label">Customer Name :</label>
                <asp:TextBox ID="tbCustName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">District ID :</label>
                <asp:TextBox ID="tbDistrictId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <label class="control-label">District Name :</label>
                <asp:TextBox ID="tbDistrictName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Province ID :</label>
                <asp:TextBox ID="tbProvinceId" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <label class="control-label">Province Name :</label>
                <asp:TextBox ID="tbProvinceName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlProvince").prop("disabled", true);
            $("#tbPlanDate").prop("disabled", true);
            $("#tbSaleId").prop("disabled", true);
            $("#tbSaleName").prop("disabled", true);
            $("#tbTypeCustomer").prop("disabled", true);
            $("#tbCustId").prop("disabled", true);
            $("#tbCustName").prop("disabled", true);
            $("#tbDistrictId").prop("disabled", true);
            $("#tbDistrictName").prop("disabled", true);
            $("#tbProvinceId").prop("disabled", true);
            $("#tbProvinceName").prop("disabled", true);
        });
    </script>
</asp:Content>
