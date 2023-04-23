<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Excel_DataTable.aspx.cs" Inherits="CIC.Excel_DataTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload" runat="server" />
    <asp:Button ID="btn_Up" runat="server" Text="上傳" OnClick="btn_Up_Click" />
    <asp:Label ID="lblMessage" runat="server" ></asp:Label>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="true"></asp:GridView>
</asp:Content>

