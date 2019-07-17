<%@ Page Title="添加/修改分类" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCategory.aspx.cs" Inherits="TygaSoft.Web.Admin.Category.AddCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style=" width:500px;">
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px; margin-top:20px;">
        <span class="fl" style="width:100px; text-align:right;"> <b class="cr">*</b>分类名称：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtCategoryname" class="txt" maxlength="50" style="width:340px;" />
            <span id="error-Categoryname" class="cr"></span>
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;">所属分类：</span>
        <div class="fl">
            <asp:DropDownList ID="ddlCategory" runat="server" />
        </div>
    </div>
    <div class="easyui-panel" data-options="border:false" style="margin-bottom:20px;">
        <span class="fl" style="width:100px; text-align:right;">备注：</span>
        <div class="fl">
            <textarea runat="server" id="txtRemark" cols="80" rows="3" style="width:340px; height:100px;"></textarea>
        </div>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />
<input type="hidden" runat="server" id="hBackToN" value="1" />

<script type="text/javascript" src="../../Scripts/Jeasyui.js"></script>
<script type="text/javascript">
    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {
        var txtCategoryname = $("[id$=txtCategoryname]");
        if ($.trim(txtCategoryname.val()).length == 0) {
            txtCategoryname.focus();
            $("#error-Categoryname").text("分类名称为必填项");
            return false;
        }
        else {
            $("#error-Categoryname").text("");
        }
        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
