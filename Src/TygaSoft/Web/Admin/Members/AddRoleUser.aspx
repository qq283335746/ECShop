<%@ Page Title="角色用户分配关系" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddRoleUser.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.AddRoleUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="角色用户关系" style=" width:500px;"> 
    <div class="clr"></div>
    <div class="easyui-panel" data-options="border:false" style=" width:300px; margin:10px; text-indent:5px; background:#E0ECFF;">
        将 “<span runat="server" id="lbTitle"></span> ” 分配给用户：
    </div>
    <div id="cbl" class="easyui-panel" data-options="border:false" style="margin:10px; text-indent:5px;">
        <asp:CheckBoxList ID="cbList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" />
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

        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
