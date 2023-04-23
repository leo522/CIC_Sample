<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Excel_Import.aspx.cs" Inherits="CIC.Excel_Import" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <asp:FileUpload ID="fileUpload" runat="server" />
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <asp:Button ID="btn_Upload" runat="server" Text="上傳" OnClick="btn_Upload_Click" />
</div>
</asp:Content>
