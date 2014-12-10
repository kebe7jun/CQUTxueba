<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="people.aspx.cs" Inherits="CQUTMyXueba.people" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Model" %>
<%@ Register Src="./link/head.ascx" TagPrefix="uc1" TagName="head" %>
<%@ Register Src="./link/copy.ascx" TagPrefix="uc1" TagName="copy" %>

<%
    try
    {
        DataTable person = Model.model.getPersonInfoById(Request.QueryString["id"]);
        DataRow data = person.Rows[0];
     %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title> 十佳大学生</title>
    <link href="css/head.css" rel="stylesheet" />
    <link href="css/copy.css" rel="stylesheet" />
</head>
<body>
    <link href="css/people.css" rel="stylesheet" />
    <div id="stuinfo_body">
        <div class="stuinfo_title">
            <%=data["title"] %>       <!--  此处更改为标题    -->
        </div>
        <hr />
        <div id="stuinfo_img">
            <img src="/xuebapic/<%=data["pic"] %>" width="280px" height="420px" />
        </div>
        <div id="stuinfo_jianjie">
            <%=data["intro"] %>
        </div>
        <div id="stuinfo_details">
        <br />
            <%=data["description"] %>
        </div>
    </div>
</body>
</html>
<%}
    catch{
        
    }
     %>