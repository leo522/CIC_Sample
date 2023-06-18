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
            <asp:Label ID="Source" runat="server" Text="申請資料來源："></asp:Label>
            <asp:RadioButtonList ID="RBSource" runat="server"  RepeatDirection="Horizontal">
                <asp:ListItem Text="學校" Value="學校" />
                <asp:ListItem Text="醫院" Value="醫院" />
                <asp:ListItem Text="使用者" Value="使用者" />
            </asp:RadioButtonList>
        </div><hr />

        <asp:Label ID="IdenUser" runat="server" Text="功能">
            <asp:RadioButtonList ID="RBUser" runat="server">
                <asp:ListItem Text="使用者輸入" Value="使用者輸入" />
                <asp:ListItem Text="管理者匯入" Value="管理者匯入" />
            </asp:RadioButtonList>
            <asp:Label ID="ReviewStatusLabel" runat="server" Text="">進度：</asp:Label><br />
            <label for="">出席者</label>
            <input type="text" name="Attendees"/>
        </asp:Label><hr />

        <asp:Panel ID="Panel1" runat="server">傑出資料申請/上傳項目</asp:Panel>
        <div>
            <label for="">競賽主題：</label>
            <input type="text" name="Competition"/>
        </div><hr />

        <div>
            <label for="">團隊參賽主題：</label>
            <input type="text" name="TeamCompetition"/>
        </div><hr />

        <div>
            <label for="">活動名稱：</label>
            <input name="Item" type="text" placeholder="實證醫學"/><hr />

            <Label for="ItemName">獎項名稱/得獎名次：</Label>
            <input name="ItemName" type="text" placeholder="金獎"/><hr />

            <Label for="Support">主辦單位：</Label>
            <input name="Organization" type="text" placeholder="醫策會"/><hr />

            <Label for="ActiveDate">發生日期：</Label>
            <input name="DateDay" type="date"/><hr />

            <Label for="TypeItem">類型：</Label>
            <asp:RadioButtonList ID="RBType" runat="server"  RepeatDirection="Horizontal">
                <asp:ListItem Text="教學類(如EBM、擬真等)" Value="Teaching" />
                <asp:ListItem Text="研究類(如口頭報告、海報發表等)" Value="Researching" />
                <asp:ListItem Text="其它" Value="Other" />
            </asp:RadioButtonList>
           
            <asp:Label ID="StatusMessage" runat="server" Text=""></asp:Label>
            <hr />

            <asp:Label ID="SupportingData" runat="server" Text="佐證資料(圖片或文件檔)：" style="display: block;"></asp:Label>
            <asp:FileUpload ID="FileUploads" runat="server" Multiple="true" />
            <asp:Button ID="UploadBtn" runat="server" Text="上傳文件圖片檔案" OnClick="UploadBtn_Click" />
            <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
            <asp:Button ID="btnSave" runat="server" Text="存檔"  OnClick="btnSave_Click" />
        </div><hr />

        <div>
            <label for="ReviewResultLabel">審查結果：</label>
            <asp:Label ID="ReviewStatus" runat="server" Text=""></asp:Label><br />
            <label for="Reason">原因：</label>
            <input name="ReasonInput" type="text"/>
        </div><hr />

        <div>
            <asp:Label ID="Pic" runat="server" Text="圖片展示"></asp:Label>
            <asp:TextBox ID="ImageIdTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="btn_Pic" runat="server" Text="讀取圖片" OnClick="btn_Pic_Click" OnClientClick="showReadConfirmation();"/>     
            <asp:Label ID="StatusLabelPic" runat="server" Text=""></asp:Label>
            <asp:Label ID="HaveRead" runat="server" Text=""></asp:Label>
        </div><hr />

        <div>
            <asp:Image ID="ImageExhibit" runat="server" />
        </div>

    </form>

    <script type="text/javascript">
    function showReadConfirmation() {
        alert("已讀");
    }
    </script>
</body>
</html>
