<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TygaSoft.Web.Shares.AboutSite.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>网站帮助中心</title>
    <link href="/Jquery/plugins/jeasyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/Jquery/plugins/jeasyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/jeasyui.css" rel="stylesheet" type="text/css" />
    <script src="/Jquery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="/Jquery/plugins/jeasyui/jquery.easyui.min.js" type="text/javascript"></script>
</head>
<body class="easyui-layout">
<form id="form1" runat="server">
    <div data-options="region:'north',title:'',split:true" style="height:30px; border-top:none; background-color:#E0ECFF; padding-top:5px;">
       <ul>
           <li class="fl"><a href="/Default.aspx">首页</a></li>
           <li class="fl ml10">|</li>
           <li class="fl ml10">天涯孤岸软件--帮助中心</li>
       </ul>
    </div>
    <div data-options="region:'south',title:'',split:true" style="height:30px; background-color:#E0ECFF; padding-top:5px;">
        <div class="tc">Copyright (C) 2013-2013 天涯孤岸版权所有</div>
    </div>
    <div data-options="region:'west',title:'功能菜单',split:true" style="width:180px;">
        <div id="menuNav"></div>
    </div>
    <div id="PageMain" data-options="region:'center',title:'center title'" style="padding:5px;">
        <div>
            <asp:Literal runat="server" ID="ltrContent"></asp:Literal>
        </div>
    </div>
</form>

<script type="text/javascript">
    $(function () {
        //所属类型
        $.ajax({
            url: "/ScriptServices/SharesService.asmx/GetSiteHelper",
            type: "post",
            contentType: "application/json; charset=utf-8",
            success: function (html) {
                $("#menuNav").html(html.d);
                $("#menuNav").accordion({
                    fit: true,
                    border: false
                });
                var hoverA = $("#menuNav").find("a[class=hover]");
                $(".layout-panel-center>.panel-header>.panel-title").text(hoverA.parent().prev().find("[class=panel-title]").text() + ">" + hoverA.text());
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    })
    function OnMenuSelect(h) {
        $(h).addClass("hover").siblings().removeClass("hover").parent().siblings().find("a").removeClass("hover");
        $(".layout-panel-center>.panel-header>.panel-title").text($(h).parent().prev().find("[class=panel-title]").text() + ">" + $(h).text());
        $.ajax({
            url: "/ScriptServices/SharesService.asmx/GetSiteHelper",
            type: "post",
            contentType: "application/json; charset=utf-8",
            success: function (html) {
                $("#menuNav").html(html.d);
                $("#menuNav").accordion({
                    fit: true,
                    border: false
                });
                var hoverA = $("#menuNav").find("a[class=hover]");
                $(".layout-panel-center>.panel-header>panel-title").text(hoverA.parent().prev().find("[class=panel-title]").text() + ">" + hoverA.text());
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
        return false;
    }
</script>
    
</body>
</html>
