<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="WeeklyPlanAdd.aspx.cs" Inherits="SaleWorker.WeeklyPlanAdd" ClientIDMode="Static" %>

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
    <form runat="server" class="form-horizontal" id="frm1">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Date :</label>
                <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="tbDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="tbDate">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-6">
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <asp:DropDownList ID="ddlTypeCust" runat="server" CssClass="form-control" placeholder="Customer Code">
                    <asp:ListItem Value="1">Exist Customer</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">New Customer</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-6">
                <button class="btn btn-primary" data-toggle="modal" data-target="#modalNewcustomer" id="btNewCustt">
                    Create New Customer</button>
            </div>


            <!-- Modal -->
            <div class="modal fade" id="modalNewcustomer" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="myModalLabel">Create New Customer</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="col-sm-10">
                                    <label class="control-label">Customer Name :</label>
                                    <asp:TextBox ID="tbNewCustName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <label class="control-label">จังหวัด :</label>
                                    <asp:DropDownList ID="ddlNewProvince" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlNewProvince_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-sm-6">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <label class="control-label">อำเภอ :</label>
                                            <asp:DropDownList ID="ddlNewDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlNewProvince" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>                          
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btNewCust" runat="server" Text="Create New Customer" CssClass="btn btn-info" OnClick="btNewCust_Click" />
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->

        </div>

        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Customer ID :</label>
                <asp:TextBox ID="tbCustId" runat="server" CssClass="form-control" placeholder="Customer ID"></asp:TextBox>
                <asp:Button ID="btSearchCust" runat="server" Text="Search Customer" CssClass="btn btn-info" OnClick="btSearchCust_Click" />
            </div>
            <div class="col-sm-6">
                <br />
                <br />
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lbCustomer" runat="server" Text="Customer Desc" CssClass="control-label"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btSearchCust" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">จังหวัด :</label>
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
            </div>
            <div class="col-sm-6">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <label class="control-label">อำเภอ :</label>
                        <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlProvince" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-2">
                <label class="control-label">หมายเหตุ :</label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10"></asp:TextBox>
            </div>
        </div>
        <br />
        <div style="text-align: center">
            <asp:Button ID="btAdd" runat="server" Text="Add Data" CssClass="btn btn-info" OnClick="btAdd_Click" />
            <button type="button" class="btn btn-default" id="btClear">
                <span class="glyphicon glyphicon-remove"></span> Clear</button>            
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {           

            $("#ddlTypeCust").change(function () {
                if ($("#ddlTypeCust").val() == 1) {
                    $("#ddlProvince").prop("disabled", false);
                    $("#ddlDistrict").prop("disabled", false);
                    $("#btSearchCust").prop("disabled", false);
                    $("#tbCustId").prop("disabled", false);
                    $("#btSearchCust").prop("disabled", false);
                    $("#btNewCustt").hide();

                } else if ($("#ddlTypeCust").val() == 2) {
                    $("#btNewCustt").show();
                    $("#ddlProvince").prop("disabled", true);
                    $("#ddlProvince").prop("selectedIndex", 0);
                    $("#ddlDistrict").prop("disabled", true);
                    $("#ddlProvince").prop("selectedIndex", 0);
                    $("#tbCustId").prop("disabled", true);
                    $("#lbCustomer").html("Customer Desc");
                    $("#btSearchCust").prop("disabled", true);
                    $("#tbCustId").val("");
                }
                //alert(typeval);
            });
            $("#btSearchCust").click(function () {
                $("#ddlProvince").prop("disabled", true);
                $("#ddlDistrict").prop("disabled", true);
            });
            $("#btClear").click(function () {
                $("#tbDate").val("");
                $("#ddlTypeCust").val("2");
                $("#btNewCustt").show();

                $("#ddlProvince").prop("disabled", true);
                $("#ddlProvince").prop("selectedIndex", 0);
                $("#ddlDistrict").prop("disabled", true);
                $("#ddlDistrict").prop("selectedIndex", 0);
                $("#btSearchCust").prop("disabled", true);
                $("#tbCustId").prop("disabled", true);
                $("#tbCustId").val("");
                $("#lbCustomer").html("Customer Desc");
                $("#lbCustomer").css('color', "black");
                $("#tbRemark").val("");
            });
            
        });
    </script>
</asp:Content>
