<%@ Page Title="" Language="C#" MasterPageFile="~/MyMasterPage.Master" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="SaleWorker.Calendar" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <form id="form1" runat="server">
        <div style="text-align:center">
            <h1><b>Weekly Plan</b></h1>
        </div>        
        <asp:Calendar ID="CalendarWithEvent" runat="Server"  BackColor="#FFFFCC" BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Verdana" 
                Font-Size="12pt" ForeColor="#663399" Height="200px" ShowGridLines="True" Width="100%" OnDayRender="CalendarWithEvent_DayRender" OnPreRender="CalendarWithEvent_PreRender">
                    <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                    <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                    <SelectorStyle BackColor="#FFCC66" />
                    <OtherMonthDayStyle ForeColor="#CC9966" />
                    <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                    <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                    <DayStyle VerticalAlign="Top" BorderWidth="1" HorizontalAlign="left" Height="50" />
                </asp:Calendar>
    </form>
    
</asp:Content>
