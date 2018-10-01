<%@ Page Title="新建用户" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style=" width:500px;">
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px; margin-top:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>用户名：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtUsername" class="txt" maxlength="20" />
            <span id="error-Username" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;"><b class="cr">*</b>设置密码：</span>
        <div class="fl">
            <input type="password" runat="server" id="txtPswset" class="txt" maxlength="20" />
            <span id="error-Pswset" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>确认密码：</span>
        <div class="fl">
            <input type="password" runat="server" id="txtPsw" class="txt" maxlength="20" />
            <span id="error-Psw" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;"><b class="cr">*</b>电子邮箱：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtEmail" class="txt" maxlength="20" />
            <span id="error-Email" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>验证码：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtVc" class="txt" maxlength="20" style="width:50px;" />
            <img border="0" id="imgCode" src="../../Handlers/ValidateCode.ashx?vcType=3" alt="看不清，单击换一张" onclick="this.src='../../Handlers/ValidateCode.ashx?vcType=3&abc='+Math.random()" style="vertical-align:middle;" />
            <span id="error-Vc" class="cr"></span>
        </div>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />

<script type="text/javascript" src="../../Scripts/Jeasyui.js"></script>
 <script type="text/javascript">
     $(function () {

         //输入用户名鼠标离开事件
         $("[id$=txtUsername]").blur(function () {
             var url = "../../Handlers/HandlerUsers.ashx";
             $.post(url, { userName: $("[id$=txtUsername]").val() }, function (msg) {
                 if (msg == "1") {
                     $("#msg").text("用户名已存在，请重新输入用户名");
                 }
                 else {
                     $("#msg").text("");
                 }
             })
         })
     })
    </script>

<input type="hidden" runat="server" id="hBackToN" value="1" />
<script type="text/javascript">
    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {
        var txtUsername = $("[id$=txtUsername]");
        if ($.trim(txtUsername.val()).length == 0) {
            txtUsername.focus();
            $("#error-Username").text("用户名为必填项");
            return false;
        }
        else {
            $("#error-Username").text("");
        }
        var txtPswset = $("[id$=txtPswset]");
        if ($.trim(txtPswset.val()).length == 0) {
            txtPswset.focus();
            $("#error-Pswset").text("密码为必填项");
            return false;
        }
        else {
            $("#error-Pswset").text("");
        }
        var txtPsw = $("[id$=txtPsw]");
        if ($.trim(txtPsw.val()).length == 0) {
            txtPsw.focus();
            $("#error-Psw").text("确认密码为必填项");
            return false;
        }
        else {
            $("#error-Psw").text("");
        }
        if ($.trim(txtPsw.val()) != $.trim(txtPswset.val())) {
            txtPsw.focus();
            $("#error-Psw").text("前后输入密码不相等，请检查");
            return false;
        }
        else {
            $("#error-Psw").text("");
        }
        var txtEmail = $("[id$=txtEmail]");
        if ($.trim(txtEmail.val()).length == 0) {
            txtEmail.focus();
            $("#error-Email").text("电子邮箱为必填项");
            return false;
        }
        else {
            $("#error-Email").text("");
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

        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
