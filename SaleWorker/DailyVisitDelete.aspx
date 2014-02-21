<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="DailyVisitDelete.aspx.cs" Inherits="SaleWorker.DailyVisitDelete" %>

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
    <form class="form-horizontal" runat="server" role="form" id="form1">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class="form-group">
            <div class="col-sm-6">
                <label class="control-label">Date From :</label>
                <asp:TextBox ID="tbDateFrom" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="tbDateFrom_CalendarExtender" runat="server" Enabled="True" TargetControlID="tbDateFrom" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
            <div class="col-sm-6">
                <label class="control-label">Date To :</label>
                <asp:TextBox ID="tbDateTo" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="tbDateTo_CalendarExtender" runat="server" Enabled="True" TargetControlID="tbDateTo" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </div>
        </div>
        <div class="form-group">
            <div style="text-align: center">
                <asp:Button ID="btSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="btSearch_Click" />
            </div>
        </div>
        <br />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">List DialyVisit for delete</legend>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="col-sm-10 maincontent">
                        <asp:GridView ID="gvActual" runat="server" CssClass="table table-bordered table-hover table-responsive table-condensed"
                            DataKeyNames="id" PageSize="10" AutoGenerateColumns="false"
                            HeaderStyle-BackColor="green" AllowPaging="true" OnRowCommand="gvActual_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server"
                                            onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server"
                                            onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>                              
                                <asp:BoundField DataField="docno"
                                    HeaderText="docno" />
                                <asp:BoundField DataField="plandate"
                                    HeaderText="date" />
                                <asp:BoundField DataField="SaleId"
                                    HeaderText="SaleId" />
                                <asp:BoundField DataField="SaleName"
                                    HeaderText="SaleName" />
                                <asp:BoundField DataField="customerid"
                                    HeaderText="customerid" />
                                <asp:BoundField DataField="customername"
                                    HeaderText="customername" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnDetail" runat="server" Text="Detail" CssClass="btn btn-info" CommandName="detail" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btSearch" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <div style="text-align: center">
                <asp:Button ID="btDelete" runat="server" Text="Delete" CssClass="btn btn-warning" OnClientClick="return ConfirmDelete();" OnClick="btDelete_Click" />
            </div>
        </fieldset>
        <div class="modal fade" id="modalDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel1">Detail Data</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">ประเภทกิจกรรม</legend>
                                    <ul class="nav nav-tabs">
                                        <li><a href="#customer" data-toggle="tab">ลูกค้า</a></li>
                                        <li><a href="#item" data-toggle="tab">สินค้า</a></li>
                                        <li><a href="#other" data-toggle="tab">อื่นๆ</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="customer">
                                            <br />
                                            <div class="form-group">
                                                <div class="col-sm-12">
                                                    <asp:CheckBoxList ID="cblCustomer" runat="server" Enabled="False">
                                                        <asp:ListItem Value="customer1">เยี่ยมลูกค้า --- แนะนำตัว</asp:ListItem>
                                                        <asp:ListItem Value="customer2">เยี่ยมลูกค้า --- สร้างความสัมพันธ์ / ให้ของขวัญ</asp:ListItem>
                                                        <asp:ListItem Value="customer3">เยี่ยมลูกค้า --- วางบิล / รับเช็ค</asp:ListItem>
                                                        <asp:ListItem Value="customer4">เยี่ยมลูกค้า --- ซื้อซอง / ยื่นซองงานประมูล</asp:ListItem>
                                                        <asp:ListItem Value="customer5">เยี่ยมลูกค้า --- ส่งสินค้าฉุกเฉิน</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="item">
                                            <br />
                                            <div class="form-group">
                                                <asp:GridView ID="gvItem" runat="server" CssClass="table table-bordered table-hover table-responsive" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="ITEMNO">
                                                    <Columns>
                                                        <asp:BoundField DataField="ITEMNO" HeaderText="ITEMNO" />
                                                        <asp:BoundField DataField="desc" HeaderText="desc" />
                                                        <asp:BoundField DataField="qty" HeaderText="unit" />
                                                        <asp:BoundField DataField="stockunit" HeaderText="stockunit" />
                                                        <asp:BoundField DataField="TypPresentItem" HeaderText="ประเภทการนำเสนอ" />
                                                        <asp:BoundField DataField="ReactionCus" HeaderText="ผลการตอบรับ" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="other">
                                            <br />
                                            <div class="form-group">
                                                <div class="col-sm-12">
                                                    <asp:CheckBoxList ID="cblOther" runat="server" Enabled="False">
                                                        <asp:ListItem Value="other1">ประชุม / อบรม / สัมนา</asp:ListItem>
                                                        <asp:ListItem Value="other2">กิจกรรมบริษัท ออกงาน Exhibition</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>                       
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfCount" runat="server" Value="0" />
    </form>
    <script type="text/javascript">
        //$(document).ready(function () {
        //    $("#btSearch").click(function () {
        //        var tbSearch = $("#tbsearch");
        //        if (tbSearch.val() == "") {
        //            alert("please to");
        //        }                
        //    });
        //});
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "#C2D69B";
                }
                else {
                    row.style.backgroundColor = "white";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
        function ConfirmDelete() {
            var count = document.getElementById("<%=hfCount.ClientID %>").value;
            var gv = document.getElementById("<%=gvActual.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].checked && chk[i].id.indexOf("chkAll") == -1) {
                    count++;
                }
            }
            if (count == 0) {
                alert("No records to delete.");
                return false;
            }
            else {
                return confirm("Do you want to delete " + count + " records.");
            }
        }
    </script>
</asp:Content>
