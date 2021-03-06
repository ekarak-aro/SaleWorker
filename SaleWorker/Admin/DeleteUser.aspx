﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DeleteUser.aspx.cs" Inherits="SaleWorker.Admin.DeleteUser" ClientIDMode="Static" %>

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
    <form runat="server" role="form" class="form-horizontal" id="form1">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <div class=" form-group">
            <div class="col-sm-4">
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Name</asp:ListItem>
                    <asp:ListItem Value="2">email</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control" required="required"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <div style="text-align: center">
                <asp:Button ID="btSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="btSearch_Click" />
            </div>
        </div>
        <br />
        <fieldset class="scheduler-border">
            <legend class="scheduler-border">List user for delete</legend>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="col-sm-10 maincontent">
                        <asp:GridView ID="gvUser" runat="server" CssClass="table table-bordered table-hover table-responsive table-condensed"
                            DataKeyNames="userid" PageSize="10" AutoGenerateColumns="false" OnPageIndexChanging="gvUser_PageIndexChanging"
                            HeaderStyle-BackColor="green" AllowPaging="true">
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
                                <asp:BoundField DataField="title"
                                    HeaderText="title" />
                                <asp:BoundField DataField="fname"
                                    HeaderText="first name" />
                                <asp:BoundField DataField="lname"
                                    HeaderText="last name" />
                                <asp:BoundField DataField="email"
                                    HeaderText="email" />
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
                <asp:Button ID="btDelete" runat="server" Text="Delete" CssClass="btn btn-warning" OnClick="btDelete_Click" OnClientClick="return ConfirmDelete();" />
            </div>
        </fieldset>
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
            var gv = document.getElementById("<%=gvUser.ClientID%>");
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
