<%@ Page Title="修改密码" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UpdatePsw.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.UpdatePsw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style=" width:500px;">
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px; margin-top:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>当前密码：</span>
        <div class="fl">
            <input type="password" runat="server" id="txtPswpast" class="txt" maxlength="20" />
            <span id="error-Pswpast" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;"><b class="cr">*</b>新密码：</span>
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
</div>

<asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />

<script type="text/javascript" src="../../Scripts/Jeasyui.js"></script>

<input type="hidden" runat="server" id="hBackToN" value="1" />
<script type="text/javascript">
    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {
        var txtPswpast = $("[id$=txtPswpast]");
        if ($.trim(txtPswpast.val()).length == 0) {
            txtPswpast.focus();
            $("#error-Pswpast").text("当前密码为必填项");
            return false;
        }
        else {
            $("#error-Pswpast").text("");
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

        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
