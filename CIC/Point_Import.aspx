﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Point_Import.aspx.cs" Inherits="CIC.Point_Import" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel_Main" runat="server">
        <div>
            <h3 style="text-align: left;">Step1 - 項目</h3>
            <asp:Label ID="LabelStep1" runat="server" Text="單位:" style="margin-right: 10px;"></asp:Label>
            <asp:DropDownList ID="DropDownList" runat="server">
                <asp:ListItem Value="Item1">項目1</asp:ListItem>
                <asp:ListItem Value="Item2">項目2</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div>
            <hr />
            <h3 style="text-align: left;">Step2 - 下載匯入格式範例</h3>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ExcelFile/教學點數申請表.xls" Text="下載範例檔案"></asp:HyperLink>
        </div>

        <div>
            <hr />
            <h3 style="text-align: left;">Step3 - 匯入編輯後檔案</h3>
            <asp:Label ID="UploadFileData" runat="server" Text="上傳檔案:" style="vertical-align: middle;"></asp:Label>
            <asp:FileUpload ID="FileUpload" runat="server" style="vertical-align: middle;"/>
            <asp:Button ID="btn_Upload" runat="server" Text="上傳" OnClick="btn_Upload_Click" style="vertical-align: middle;"/>
        </div>
    </asp:Panel>
</asp:Content>

