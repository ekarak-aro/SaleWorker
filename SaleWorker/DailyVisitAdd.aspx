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

        .scrollToTop {
            width: 125px;
            height: 130px;
            padding: 10px;
            text-align: center;
            background: whiteSmoke;
            font-weight: bold;
            color: aqua;
            text-decoration: none;
            position: fixed;
            top: 75px;
            right: 40px;
            display: none;
            background: url('arrow_up.png') no-repeat 0px 20px;
        }

            .scrollToTop:hover {
                text-decoration: none;
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
        <br />
        <hr />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">ประเภทกิจกรรม </legend>

            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li><a href="#customer" data-toggle="tab">ลูกค้า</a></li>
                <li><a href="#item" data-toggle="tab">สินค้า</a></li>
                <li><a href="#other" data-toggle="tab">อื่นๆ</a></li>
            </ul>

            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active" id="customer">
                    <br />
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:CheckBoxList ID="cblCustomer" runat="server">
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
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddlTypeItem" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">กรุณาเลือก</asp:ListItem>
                                        <asp:ListItem Value="1">แนะนำสินค้า</asp:ListItem>
                                        <asp:ListItem Value="2">สาธิตการใช้งาน</asp:ListItem>
                                        <asp:ListItem Value="3">แก้ไขปัญหาสินค้า</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- Modal -->
                    <div class="modal fade" id="modalItem" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title" id="myModalLabel">ค้นหารายการสินค้า</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <div class="col-sm-6">
                                            <asp:DropDownList ID="ddlItemSearchType" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">ItemCode</asp:ListItem>
                                                <asp:ListItem Value="1">ItemDesc</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:TextBox ID="tbSearchItem" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div style="text-align: center">
                                            <asp:Button ID="btSearchItem" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btSearchItem_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvSearchItem" runat="server" CssClass="table table-bordered table-hover table-responsive" AllowPaging="true" AutoGenerateColumns="false"
                                                    PageSize="10" OnPageIndexChanging="gvSearchItem_PageIndexChanging" OnRowCommand="gvSearchItem_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="ITEMNO" HeaderText="ITEMNO" />
                                                        <asp:BoundField DataField="desc" HeaderText="desc" />
                                                        <asp:BoundField DataField="STOCKUNIT" HeaderText="STOCKUNIT" />
                                                        <asp:TemplateField HeaderText="Select Data">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btSelectDataItem" runat="server" Text="Select" CommandName="select" class="btn btn-info" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btSearchItem" />
                                                <asp:AsyncPostBackTrigger ControlID="gvSearchItem" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-6">
                            <label class="control-label">รหัสสินค้า :</label>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="tbItemCode" runat="server" CssClass="form-control" disabled="disabled"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <button class="btn btn-primary" data-toggle="modal" data-target="#modalItem">
                                ค้นหารหัสสินค้า
                            </button>
                        </div>
                        <div class="col-sm-6">
                            <br />
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lbItemDesc" runat="server" Text="ItemDesc"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-6">
                                    <label class="control-label">จำนวนคาดว่าที่ต้องใช้ :</label>
                                    <asp:TextBox ID="tbUnit" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tbUnit" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-sm-6">
                                    <br />
                                    <br />
                                    <asp:Label ID="lbUom" runat="server" Text="UOM"></asp:Label>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <div class="col-sm-6">
                                    <label class="control-label">ผลการตอบรับลูกค้า :</label>
                                    <asp:DropDownList ID="ddlReactionCus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">กรุณาเลือก</asp:ListItem>
                                        <asp:ListItem Value="1">รอลูกค้าตัดสินใจ</asp:ListItem>
                                        <asp:ListItem Value="2">อนุมัติแล้วรอ PO</asp:ListItem>
                                        <asp:ListItem Value="3">ออก PO เรียบร้อย</asp:ListItem>
                                        <asp:ListItem Value="4">จัดการคำขอเรียบร้อย</asp:ListItem>
                                        <asp:ListItem Value="5">ลูกค้าปฏิเสธ --- ไม่มีสินค้า/ไม่เพียงพอ</asp:ListItem>
                                        <asp:ListItem Value="6">ลูกค้าปฏิเสธ --- ราคา</asp:ListItem>
                                        <asp:ListItem Value="7">ลูกค้าปฏิเสธ --- คุณภาพ</asp:ListItem>
                                        <asp:ListItem Value="8">ลูกค้าปฏิเสธ --- Lead Time นานเกินไป</asp:ListItem>
                                        <asp:ListItem Value="9">ลูกค้าขอใบเสนอราคา</asp:ListItem>
                                        <asp:ListItem Value="10">ลูกค้าขอดูตัวอย่าง</asp:ListItem>
                                        <asp:ListItem Value="11">ลูกค้าขอสาธิตการใช้งาน</asp:ListItem>
                                        <asp:ListItem Value="12">ลูกค้าขอเอกสารเพิ่มเติม</asp:ListItem>
                                        <asp:ListItem Value="13">ลูกค้าขอ test สินค้า</asp:ListItem>
                                        <asp:ListItem Value="14">ลูกค้าขอยกเลิก PO</asp:ListItem>
                                        <asp:ListItem Value="15">ลูกค้าขอคืนสินค้า</asp:ListItem>
                                        <asp:ListItem Value="16">ลูกค้าขอเปลี่ยนสินค้า</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-6"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group">
                        <div style="text-align: center">
                            <asp:Button ID="btAddItem" runat="server" Text="AddItem" CssClass="btn btn-primary" OnClick="btAddItem_Click" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvItem" runat="server" CssClass="table table-bordered table-hover table-responsive" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="gvSearchItem_PageIndexChanging" DataKeyNames="ITEMNO" OnRowCommand="gvItem_RowCommand" OnRowDeleting="gvItem_RowDeleting">
                                    <Columns>
                                        <asp:BoundField DataField="ITEMNO" HeaderText="ITEMNO" />
                                        <asp:BoundField DataField="desc" HeaderText="desc" />
                                        <asp:BoundField DataField="unit" HeaderText="unit" />
                                        <asp:BoundField DataField="stockunit" HeaderText="stockunit" />
                                        <asp:BoundField DataField="TypPresentItem" HeaderText="ประเภทการนำเสนอ" />
                                        <asp:BoundField DataField="ReactionCus" HeaderText="ผลการตอบรับ" />
                                        <asp:TemplateField HeaderText="Delete Data">
                                            <ItemTemplate>
                                                <asp:Button ID="btDeleteDataItem" runat="server" Text="Delete" CommandName="delete" class="btn btn-info" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btAddItem" />
                                <asp:AsyncPostBackTrigger ControlID="gvItem" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="tab-pane" id="other">
                    <br />
                    <div class="form-group">
                        <div class="col-sm-12">
                            <asp:CheckBoxList ID="cblOther" runat="server">
                                <asp:ListItem Value="other1">ประชุม / อบรม / สัมนา</asp:ListItem>
                                <asp:ListItem Value="other2">กิจกรรมบริษัท ออกงาน Exhibition</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="form-group">
            <div class="col-sm-2">
                <label class="control-label">หมายเหตุ : </label>
            </div>
            <div class="col-sm-10">
                <asp:TextBox ID="tbRemark" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div style="text-align: center">
                <asp:Button ID="btSave" runat="server" Text="Save Data" CssClass="btn btn-info btn-lg" />
            </div>
        </div>

        <div class="hidden-xs hidden-sm">
            <a href="#" class="scrollToTop">Scroll To Top</a>
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tbWeeklyPlanNo").prop("disabled", true);
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
            $("#tbItemCode").prop("disabled", true);

            $("#tbUnit").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(event.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
                    // Allow: Ctrl+A
                    (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });

            //Check to see if the window is top if not then display button
            $(window).scroll(function () {
                if ($(this).scrollTop() > 100) {
                    $('.scrollToTop').fadeIn();
                } else {
                    $('.scrollToTop').fadeOut();
                }
            });

            //Click event to scroll to top
            $('.scrollToTop').click(function () {
                $('html, body').animate({ scrollTop: 0 }, 800);
                return false;
            });
        });
    </script>
</asp:Content>
