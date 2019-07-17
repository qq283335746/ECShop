<%@ Page Title="添加角色" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddRole.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.AddRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style=" width:500px;">
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px; margin-top:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>角色名称：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtRolename" class="txt" maxlength="50" />
            <span id="error-Rolename" class="cr"></span>
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
        var txtRolename = $("[id$=txtRolename]");
        if ($.trim(txtRolename.val()).length == 0) {
            txtRolename.focus();
            $("#error-Rolename").text("角色名为必填项");
            return false;
        }
        else {
            $("#error-Rolename").text("");
        }
        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
