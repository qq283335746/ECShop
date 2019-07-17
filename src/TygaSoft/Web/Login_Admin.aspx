<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_Admin.aspx.cs" Inherits="TygaSoft.Web.Login_Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>后台登录</title>
    <asp:Literal runat="server" ID="ltrTheme"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
    <div id="login">
    <div class="easyui-layout" style="width:500px;height:285px; margin:0 auto;">
        <div data-options="region:'center',title:'后台登录'">
            <div style="margin:50px auto;">
                <div class="easyui-panel" data-options="border:false" style="margin-bottom:10px;">
                    <span class="fl" style="width:100px; text-align:right;"> 用户名：</span>
                    <div class="fl">
                        <input type="text" runat="server" id="txtUsername" class="txt" maxlength="20" />
                        <span id="error-Username" class="cr"></span>
                    </div>
                </div>
                <div class="easyui-panel" data-options="border:false" style="margin-bottom:10px;">
                    <span class="fl" style="width:100px; text-align:right;"> 密 码：</span>
                    <div class="fl">
                        <input type="password" runat="server" id="txtPsw" class="txt" maxlength="50" />
                        <span id="error-Psw" class="cr"></span>
                    </div>
                </div>
                <div class="easyui-panel" data-options="border:false" style="margin-bottom:10px;">
                    <span class="fl" style="width:100px; text-align:right;">验证码：</span>
                    <div class="fl">
                        <input type="text" runat="server" id="txtVc" class="txt" maxlength="6" style="width:50px;" />
                        <img border="0" id="imgCode" src="Handlers/ValidateCode.ashx?vcType=login" alt="看不清，单击换一张" onclick="this.src='Handlers/ValidateCode.ashx?vcType=login&abc='+Math.random()" style="vertical-align:middle;" />
                        <span id="error-Vc" class="cr"></span>
                    </div>
                </div>
                <div class="easyui-panel" data-options="border:false" style="margin-bottom:10px;">
                    <span class="fl" style="width:100px; text-align:right;">&nbsp;</span>
                    <div class="fl">
                        <a id="abtnLogin" href="javascript:void(0)" onclick="OnLogin()" class="easyui-linkbutton">登 录</a>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
    </div>

    <asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />

    </form>

    <script type="text/javascript">
        $(function () {
            //动态使登录框垂直居中
            var h = $(window).height();
            h = (h - 300) / 2;
            $("#login").css("margin-top", "" + h + "px");

        })

        function OnLogin() {
            var txtUsername = $("[id$=txtUsername]");
            if ($.trim(txtUsername.val()).length == 0) {
                txtUsername.focus();
                $("#error-Username").text("用户名为必填项");
                return false;
            }
            else {
                $("#error-Username").text("");
            }
            var txtPsw = $("[id$=txtPsw]");
            if ($.trim(txtPsw.val()).length == 0) {
                txtPsw.focus();
                $("#error-Psw").text("密码为必填项");
                return false;
            }
            else {
                $("#error-Psw").text("");
            }
            var txtVc = $("[id$=txtVc]");
            if ($.trim(txtVc.val()).length == 0) {
                txtVc.focus();
                $("#error-Vc").text("验证码为必填项");
                return false;
            }
            else {
                $("#error-Vc").text("");
            }

            alert("aa");
            __doPostBack('lbtnSave', '');
        }
    </script>

</body>
</html>
