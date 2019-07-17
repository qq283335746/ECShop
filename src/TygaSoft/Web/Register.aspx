<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TygaSoft.Web.Register" %>
<%@ Register src="~/WebUserControls/top.ascx" tagname="top" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>注册</title>
    <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Register.css" rel="stylesheet" type="text/css" />
    <link href="Jquery/plugins/jeasyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="Jquery/plugins/jeasyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="Styles/jeasyui.css" rel="stylesheet" type="text/css" />
    <script src="Jquery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Jquery/plugins/jeasyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Scripts/EasyuiExtend.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            //输入用户名鼠标离开事件
            $("#tbName").blur(function () {
                var url = "Handlers/HandlerUsers.ashx";
                $.post(url, { userName: $('#tbName').val() }, function (msg) {
                    if (msg == "1") {
                        $("#msg").text("用户名已存在，请重新输入用户名");
                    }
                    else {
                        $("#msg").text("");
                    }
                })
            })
            //动态使登录框垂直居中
            var h = $(window).height();
            h = (h - 400) / 2;
            if (h > 0) {
                $("#vc").css("margin-top", "" + h + "px");
            }

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <!--header begin-->
        <div id="header">
          <uc1:Top ID="Top1" runat="server" />
        </div>
     <!--header end-->
     <!--pagemain begin-->
        <div id="pagemain">
          <div id="vc" style="margin:50px auto; width:400px;">
          <div class="easyui-panel" title="注册会员" style="width:400px;">
             <div class="row mt10">
               <span class="fl rl">用户名：</span>
               <div class="fl"><input type="text" runat="server" id="txtUserName" name="txtUserName" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" /></div>
               <div class="clr"></div>
             </div>
             <div class="row mt10">
               <span class="fl rl">设置密码：</span>
               <div class="fl"><input type="password" runat="server" id="txtPsw" name="txtPsw" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" validType="checkPsw['#txtPsw']" /></div>
               <div class="clr"></div>
             </div>
             <div class="row mt10">
               <span class="fl rl">确认密码：</span>
               <div class="fl"><input type="password" runat="server" id="txtCfmPsw" name="txtCfmPsw" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" validType="cfmPsw['#txtPsw']" /></div>
               <div class="clr"></div>
             </div>
             <div class="row mt10">
               <span class="fl rl">电子邮箱：</span>
               <div class="fl"><input type="text" runat="server" id="txtEmail" name="txtEmail" class="easyui-validatebox" data-options="required:true,validType:'email',missingMessage:'必填项',invalidMessage:'请输入正确的电子邮箱格式'" /></div>
               <div class="clr"></div>
             </div>
             <div class="row mt10">
               <span class="fl rl">验证码：</span>
               <div class="fl">
                   <input type="text" runat="server" id="txtVc" name="txtVc" style="width:50px;" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项',invalidMessage:'请输入4位验证码',validType:'length[4,4]'" />
                   <img border="0" id="imgCode" src="Handlers/ValidateCode.ashx?vcType=register" alt="看不清，单击换一张" onclick="this.src='Handlers/ValidateCode.ashx?vcType=register&abc='+Math.random()" style="vertical-align:middle;" />
               </div>
               <div class="clr"></div>
             </div>
             <div class="row mtb10">
               <span class="fl rl">&nbsp</span>
               <div class="fl"><a href="javascript:void(0)" class="easyui-linkbutton" onclick="OnCommit()">提交</a></div>
               <div class="clr"></div>
             </div>
          </div>
          </div>
        </div>
     <!--pagemain end-->
     <!--footer begin-->
        <div id="footer"></div>
     <!--footer eng-->

     <asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" CommandName="lbtnsave" />

    </form>
    <script type="text/javascript">
        function OnCommit() {
            var isValid = $('#form1').form('validate');
            if (isValid) {
                __doPostBack('lbtnPostBack', '');
            }
        }
    </script>
</body>
</html>
