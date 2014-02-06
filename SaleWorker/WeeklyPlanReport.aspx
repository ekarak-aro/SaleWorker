<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="WeeklyPlanReport.aspx.cs" Inherits="SaleWorker.WeeklyPlanReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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

        .maincontent {
            width: 100%;
            height: 100%;
            overflow: auto;
            position: relative;
        }
    </style>
    <form id="form1" runat="server" class="form-horizontal">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Date :</label>
                <asp:TextBox ID="tbDateFrom" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" required="required"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="tbDateFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="tbDateFrom">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-6">
                <label class="control-label">To :</label>
                <asp:TextBox ID="tbDateTo" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" required="required"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="tbDateTo_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="tbDateTo">
                </ajaxToolkit:CalendarExtender>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Sale :</label>
                <asp:DropDownList ID="ddlSale" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="row" style="text-align: center">
            <asp:Button ID="btSearchData" runat="server" Text="Search Data" class="btn btn-success" OnClick="btSearchData_Click" />
        </div>
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">Report </legend>

            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li><a href="#Excel" data-toggle="tab">Excel</a></li>
                <li><a href="#ReportViewer" data-toggle="tab">ReportViewer</a></li>
            </ul>

            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active" id="Excel">
                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-6 maincontent">
                                    <asp:GridView ID="gvItem" runat="server" Width="100%" CssClass="table table-bordered table-hover table-responsive table-condensed" AllowPaging="True" OnPageIndexChanging="gvItem_PageIndexChanging" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="docno" HeaderText="docno" />
                                            <asp:BoundField DataField="plandate" HeaderText="plandate" />
                                            <asp:BoundField DataField="saleid" HeaderText="saleid" />
                                            <asp:BoundField DataField="salename" HeaderText="salenamen" />
                                            <asp:BoundField DataField="customername" HeaderText="customername" />
                                            <asp:BoundField DataField="typecustomer" HeaderText="TypeCustomer" />
                                            <asp:BoundField DataField="customerid" HeaderText="customerid" />
                                            <asp:BoundField DataField="customername" HeaderText="customername" />
                                            <asp:BoundField DataField="districtname" HeaderText="districtname" />
                                            <asp:BoundField DataField="provincename" HeaderText="provincename" />
                                            <asp:BoundField DataField="remark" HeaderText="remark" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btSearchData" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                    <div class="row" style="text-align: center">
                        <asp:Button ID="btExcel" runat="server" Text="Excel" class="btn btn-info" OnClick="btExcel_Click" />
                    </div>
                </div>
                <div class="tab-pane" id="ReportViewer">
                    <div class="col-sm-12">
                        <div class="maincontent">
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="100%" Style="width: 100% ; overflow:hidden;">
                            </rsweb:ReportViewer>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </form>
</asp:Content>
