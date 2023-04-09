<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIC_Index.aspx.cs" Inherits="CIC.CIC_Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            &nbsp;<asp:Label ID="Name_CIC" runat="server" Text="姓名："></asp:Label>
            <input id="Text2" name="NameSerach" type="text" placeholder="請輸入姓名" aria-required="true"/>&nbsp;&nbsp;
            <asp:Button ID="btn_PreView" runat="server" Text="預覽" OnClick="btn_PreView_Click" />
            <asp:Button ID="btn_DownLoad" runat="server" Text="下載" OnClick="btn_DownLoad_Click"/>
            <br />
            <span id="errMsg" style="color:red"></span>
            </div>
        <div>
            <asp:FileUpload ID="ExcleUpload" runat="server" />
            <asp:Button ID="btn_Upload" runat="server" Text="上傳" OnClick="btn_Upload_Click" />
        </div>
        <div>
            <asp:Button ID="btn_UP" runat="server" Text="匯入" OnClick="btn_UP_Click" />
        </div>
            <div>
                <asp:Image ID="CIC_ImagePreview" runat="server" Height="272px" Width="483px" ImageUrl="~/Images/Pics.jpg"/>
            </div>
    </form>

</body>
</html>
