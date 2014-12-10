<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CQUTMyXueba._default" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Model" %>
<%@ Register Src="./link/head.ascx" TagPrefix="LINK" TagName="head" %>
<%@ Register Src="./link/copy.ascx" TagPrefix="LINK" TagName="copy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>计算机科学与工程学院学霸点赞系统</title>
    <link href="css/index.css" rel="stylesheet" />
    <link href="css/head.css" rel="stylesheet" />
    <link href="css/copy.css" rel="stylesheet" />
    <script src="js/fate.js"></script>
    <script src="js/load.js"></script>
    <script src="js/action.js"></script>
    <script src="js/showDetails.js" ></script>
</head>
<body>
    <div id="showInfoLeft" class="default_style_left"  onmouseover="showL()" onmouseout="hiddenL()" > 
        <iframe src="" id="iframetest" width="100%" height="100%">

        </iframe>
    </div>

    <div id="showInfoRight" class="default_style_right"  onmouseover="showR()" onmouseout="hiddenR()"> 
        <iframe src="" id="iframetest1" width="100%" height="100%">
           
        </iframe>
    </div>
    <!-- 加载-->
    <div id="LOADING" style="z-index:5000">
        <div style="position: relative; top: 200px;">
            <div class="CFCLOGO">
                <img src="img/CFCLOGO.png" />
            </div>
            <div class="loadstar">
                <img src="img/loadStar.gif" />
            </div>
        </div>
    </div>
    <!-- 提示-->
    <div id="tipS">
        <div id="tip">
            <div id="tiptitle">
                Tip
            </div>
            <div id="tipcontent">
                正在投票...
            </div>
            <div id="tipok" onclick="closeTips()">
                知道了
            </div>
        </div>
        <div id="tipback">
        </div>
    </div>

    <!-- headers -->
    <LINK:head runat="server" ID="head" />

    <!--  note -->
    <div id="note">
        <div id="notes">
            <p style="top:1px;">
                <h2>活动简介：</h2>
               &nbsp;&nbsp;&nbsp;&nbsp;为加强学校学风建设，营造良好学习氛围，学校于近期开展了2014年度“我身边的十大学霸”寻访活动，寻访活动在全校范围内挑选出10名最具代表性的学习先进典型，发挥正能量，引导广大同学热爱学习、正确学习、刻苦学习，推动优良学风的形成。
<br />&nbsp;&nbsp;&nbsp;&nbsp;凭借此次学校“十大学霸”的寻访契机，我院将在本学院的历届学生中，挑选出学习成绩优秀的学霸，寻求在国际比赛中取得卓越成绩的杰出人才，采访进入BAT等公司的专业精英。分享他们的心路历程，以及在通往成功路上披荆斩棘的人生感悟。
 <br />&nbsp;&nbsp;&nbsp;&nbsp;相信他们的故事，他们的心得，他们别样的年华，将会潜移默化地影响着我们躁动的青春，唤醒我们意气风发的少年梦，让我们在迷茫的未来中找寻属于自己的星光，找准自己的方向。
 <strong>如果你从他们的经历中有任何启发和感受，记得给他们点赞！！  </strong>         </p>
 
        </div>
    </div>
    <!-- TOP   -->
    <%  DataTable User = Model.model.getUserTable();
        List<DataRow> xueba1 = new List<DataRow>();
        List<DataRow> xueba2 = new List<DataRow>();
        int count = User.Rows.Count;
        for (var i = 0; i < count; i++)
        {
            if (int.Parse(User.Rows[i]["type"].ToString()) == 1)
            {
                xueba1.Add(User.Rows[i]);
            }
            else
            {
                xueba2.Add(User.Rows[i]);
            }
        }
        DataRow[] houxuanren = xueba1.ToArray();
        DataRow[] weilaixuebas = xueba2.ToArray();
        
        DataTable Tickts = Model.model.getUserTable();
            for (var i = 0; i < houxuanren.Count(); i = i + 2)
            {
    %>
    <div class="tr10">
        <!--tr-->
        <div class="people10">
            <div class="op" onmouseover="showRight(<%=houxuanren[i]["id"]%>)" onmouseout="hiddenRight(<%=houxuanren[i]["id"]%>)" ></div>
            <div class="student" >
                <div class="head1" style="background-image: url('/xuebapic/<%=houxuanren[i]["pic"]%>'); background-repeat: no-repeat;background-size:180px 270px;">
                </div>

            </div>
            <div class="about">
                <div class="aboutSelf">
                    &nbsp;&nbsp;&nbsp;&nbsp;<span id="username<%=houxuanren[i]["id"]%>"><%=houxuanren[i]["name"]%></span>&nbsp;<%=houxuanren[i]["intro"]%>
                </div>
                <div class="rongyu">
                    <span>获奖情况:</span><br />
                    <%=houxuanren[i]["awards"]%>
                </div>
            </div>

            <div class="comit">
                <div class="numbers" id="numbers<%=houxuanren[i]["id"]%>">
                </div>
                <div class="toupiao">
                    <div id="votecount">
<!--
    此处请修改js/action.js里面的内容，因为我不知道你那边投票之后返回的信息是什么。
    只需要修改 Jsonhttp.onreadystatechange = function () 里面的内容就行了。

    -->
                        <%=Model.model.getVoteById(houxuanren[i]["id"].ToString()) %>
                        <!--
                            此处修改为获取票数API
                            -->
                    </div>
                    <input type="button" value="" class="anu"  onclick="showTips(<%=houxuanren[i]["id"]%>)"  />
                </div>

            </div>

        </div>
        <%if (i < houxuanren.Count() - 1)
          {
        %>
    <div class="people10 td2">
            <div class="op" onmouseover="showLeft(<%=houxuanren[i+1]["id"]%>)" onmouseout="hiddenLeft(<%=houxuanren[i+1]["id"]%>)" ></div>
            <div id="stu_details<%=houxuanren[i + 1]["id"]%>" class="stu_details_default">
                <div class="details_text">
                <%=houxuanren[i]["description"]%>
                </div>
            </div>
            <div class="student">
                <div class="head1" onmouseover="show(<%=houxuanren[i]["id"]%>)" onmouseout="hidden1(<%=houxuanren[i]["id"]%>)" style="background-size:180px 270px;background-image: url('/xuebapic/<%=houxuanren[i+1]["pic"]%>'); background-repeat: no-repeat">
                </div>
            </div>
            <div class="about">
                <div class="aboutSelf">
                    &nbsp;&nbsp;&nbsp;&nbsp;<span id="username<%=houxuanren[i + 1]["id"]%>"><%=houxuanren[i + 1]["name"]%></span> <%=houxuanren[i + 1]["intro"]%>
                </div>
                <div class="rongyu">
                    <span>获奖情况:</span><br />
                    <%=houxuanren[i + 1]["awards"]%>
                </div>
            </div>
            <div class="comit">
                <div class="numbers" id="numbers<%=houxuanren[i + 1]["id"]%>">
                </div>
                <div class="toupiao">
                    <div id="votecount">
                        <%=Model.model.getVoteById(houxuanren[i+1]["id"].ToString()) %>
                    </div>
                    <input type="button" value="" class="anu"  onclick="showTips(<%=houxuanren[i+1]["id"]%>)"  />
                </div>
            </div>
        </div>
        <%
          }
          else
          {
          }%>

    </div>
    
    <!--end tr-->
    <%}   %>

    
    <!--          COPY                  -->
    <LINK:copy runat="server" ID="copy" />


</body>
</html>