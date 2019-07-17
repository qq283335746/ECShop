<%@ Page Title="站点内容类型集" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListContentType.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.ListContentType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script src="/Scripts/Jeasyui.js" type="text/javascript"></script>
<script src="/Scripts/TreeFun.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    
<div class="mb5" style="display:none;">
   <a href="javascript:;" class="easyui-linkbutton" data-options="iconCls:'icon-add'" onclick="treeFun.add('treeCt')">新建</a>
</div>
<div class="clr"></div>
<div class="easyui-panel" title="站点内容类型" style="padding:5px;">
    <ul id="treeCt" class="easyui-tree"></ul>
    <div id="mm" class="easyui-menu" style="width:120px;">
        <div onclick="treeFun.add('treeCt')" data-options="iconCls:'icon-add'">添加</div>
        <div onclick="treeFun.edit('treeCt')" data-options="iconCls:'icon-edit'">编辑</div>
        <div onclick="treeFun.del('treeCt')" data-options="iconCls:'icon-remove'">删除</div>
    </div>
</div>
<div class="clr"></div>
<div id="dlgAdd" style="width:500px; height:200px;" href="../Html/AddContentType.htm">

</div>

<script type="text/javascript">
    $(function () {
        treeFun.init('treeCt');
    })

    function OnAddDlgLoad() {
        treeFun.url = "/ScriptServices/AdminService.asmx/InsertContentType";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (node) {
            $('#lbParent').text(node.text);
            $('#dlgForm').form('load', {
                hParentId: node.id,
                hId:""
            });
        }
        else {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
        }
    }

    function onEditDlgLoad() {
        treeFun.url = "/ScriptServices/AdminService.asmx/UpdateContentType";
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (node) {
            $('#lbParent').text(node.attributes.parentName);
            $('#dlgForm').form('load', {
                hId: node.id,
                hParentId: node.attributes.parentId,
                txtName: node.text
            });
        }
        else {
            $.messager.alert('错误提示', '请选中一个节点再进行操作', 'error');
        }
    }

    function OnDlgSave() {
        var isValid = $('#dlgForm').form('validate');
        if (!isValid) {
            return false;
        }
        $.ajax({
            type: "POST",
            url: treeFun.url,
            contentType: "application/json; charset=utf-8",
            data: "{name: '" + $("#txtName").val() + "', parentId: '" + $("#hParentId").val() + "',nId:'" + $("#hId").val() + "'}",
            success: function (data) {
                var msg = data.d;
                jeasyuiFun.topCenter("温馨提醒",msg);
                if (msg.indexOf("成功") > -1) {
                    treeFun.load("treeCt");
                    $('#dlgAdd').window('close');
                }
            }
        })
    }

    function OnDel() {
        var t = $("#treeCt");
        var node = t.tree('getSelected');
        if (node) {
            $.messager.confirm('温馨提醒', '确定要删除吗?', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "/ScriptServices/AdminService.asmx/DelContentType",
                        contentType: "application/json; charset=utf-8",
                        data: "{nId:'" + node.id + "'}",
                        success: function (data) {
                            var msg = data.d;
                            jeasyuiFun.topCenter("温馨提醒", msg);
                            if (msg.indexOf("成功") > -1) {
                                treeFun.load("treeCt");
                            }
                        }
                    })
                }
            });
        }
        else {
            $.messager.alert('温馨提醒', "没有选中任何数据", 'error'); 
        }
    }
</script>

</asp:Content>
