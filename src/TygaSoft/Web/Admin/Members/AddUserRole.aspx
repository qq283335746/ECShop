<%@ Page Title="用户角色分配" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddUserRole.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.AddUserRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="用户分配角色" style=" width:500px;"> 
    <div class="clr"></div>
    <div class="easyui-panel" data-options="border:false" style=" width:300px; margin:10px; text-indent:5px; background:#E0ECFF;">
        将 “<span runat="server" id="lbTitle"></span> ” 添加到角色：
    </div>
    <div id="cbl" class="easyui-panel" data-options="border:false" style="margin:10px; text-indent:5px;">
        <asp:Label ID="lbMsg" runat="server"></asp:Label>
        <asp:CheckBoxList ID="cbList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" />
        <input type="hidden" id="hOldId" runat="server" />
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
        if ($("[id$=hOldId]").val().length == 0) {
            var c = $("#cbl").find(":checkbox:checked");
            if (c == undefined) {
                alert("没有找到任何角色，请检查");
                return false;
            }
            if (c.length == 0) {
                alert("请至少勾选一个角色，请检查");
                return false;
            }
        }

        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
