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
            &nbsp;<asp:Label ID="Label2" runat="server" Text="職別："></asp:Label>
            <input id="Text2" name="Text2" type="text" placeholder="請輸入職別" aria-required="true"/>&nbsp;&nbsp;
            <asp:Button ID="btn_DownLoad" runat="server" Text="下載" OnClick="btn_DownLoad_Click"/>
            <br />
            <div>
                <asp:Image ID="Image1" runat="server" Height="272px" Width="483px" ImageUrl="~/Images/Pics.jpg"/>
            </div>
            
        </div>
    </form>

</body>
</html>
