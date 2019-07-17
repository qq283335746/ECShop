<%@ Page Title="添加/修改论坛内容类型" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddContentType.aspx.cs" Inherits="TygaSoft.Web.Admin.BBS.AddContentType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style="width:500px;">
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>类型名称：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtTypeName" name="txtTypeName" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" />
        </div>
        <div class="clr"></div>
    </div>
   <div class="row mtb10">
        <span class="fl rl"><b class="cr">*</b>所属类型：</span>
        <div class="fl">
            <input id="txtParent" runat="server" class="easyui-combotree" style="width:200px;" />
        </div>
        <div class="clr"></div>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" CommandName="lbtnsave" />
<input type="hidden" runat="server" id="hBackToN" value="1" />
<script type="text/javascript" src="../../Scripts/Jeasyui.js"></script>
<script type="text/javascript">
    $(function () {
        $.ajax({
            url: "/ScriptServices/AdminService.asmx/GetContentTypeJson",
            type: "post",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var jsonData = (new Function("", "return " + data.d))();
                $('[id$=txtParent]').combotree('loadData', jsonData);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    })

    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }
    function OnSave() {
        var isValid = $('#form1').form('validate');
        if (isValid) {
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }
    }
</script>

</asp:Content>
