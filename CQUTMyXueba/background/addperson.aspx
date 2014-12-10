<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addperson.aspx.cs" Inherits="CQUTMyXueba.background.addperson" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>人员添加-上传照片</title>
</head>
<body>
    <form runat="server">
        请先上传照片，正确上传以后会自动重定向到信息添加页面：<br />
        <input type="file" name="pic" id="pic" runat="server" placeholder="选择一张照片"/>
        <asp:Button id="uploadpic" runat="server" Text="上传图片" OnClick="uploadpic_Click"/>
    </form>
</body>
</html>