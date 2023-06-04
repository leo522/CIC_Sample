<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Outstanding.aspx.cs" Inherits="CIC.Outstanding" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>傑出表現</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Source" runat="server" Text="來源："></asp:Label>
            <asp:RadioButtonList ID="RBSource" runat="server"  RepeatDirection="Horizontal">
                <asp:ListItem Text="學校" Value="學校" />
                <asp:ListItem Text="醫院" Value="醫院" />
                <asp:ListItem Text="使用者" Value="使用者" />
            </asp:RadioButtonList>
        </div><hr />

        <div>
            <asp:Panel ID="Panel1" runat="server">申請/上傳項目</asp:Panel>
            <asp:Label ID="ItemName" runat="server" Text="活動名稱-獎項："></asp:Label>
            <input name="Item" type="text" placeholder="實證醫學-金獎"/><hr />

            <asp:Label ID="Support" runat="server" Text="主辦單位："></asp:Label>
            <input name="SupportItem" type="text" placeholder="醫策會"/><hr />

            <asp:Label ID="ActiveDate" runat="server" Text="發生日期："></asp:Label>
            <input name="DateDay" type="date"/><hr />

            <asp:Label ID="TypeItem" runat="server" Text="類型："></asp:Label>
            <asp:RadioButtonList ID="RBType" runat="server"  RepeatDirection="Horizontal">
                <asp:ListItem Text="教學類(如EBM、擬真等)" Value="Teaching" />
                <asp:ListItem Text="研究類(如口頭報告、海報發表等)" Value="Researching" />
                <asp:ListItem Text="其它" Value="Other" />
            </asp:RadioButtonList>
            <asp:Button ID="btnSave" runat="server" Text="存檔" OnClick="btnSave_Click" />
            <asp:Label ID="StatusMessage" runat="server" Text=""></asp:Label>
            <hr />

            <asp:Label ID="SupportingData" runat="server" Text="佐證資料(圖片或文件檔)：" style="display: block;"></asp:Label>
            <asp:FileUpload ID="FileUploads" runat="server" Multiple="true" />
            <asp:Button ID="UploadBtn" runat="server" Text="上傳文件檔案" OnClick="UploadBtn_Click" />
            <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
        </div><hr />

        <div>
            <asp:Label ID="Pic" runat="server" Text="圖片展示"></asp:Label>
            <asp:TextBox ID="ImageIdTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="btn_Pic" runat="server" Text="讀取圖片" OnClick="btn_Pic_Click" />     
            <asp:Label ID="StatusLabelPic" runat="server" Text=""></asp:Label>
        </div><hr />
        
        <div>
            <asp:Image ID="ImageExhibit" runat="server" />
        </div>
    </form>
</body>
</html>
