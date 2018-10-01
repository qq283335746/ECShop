<%@ Page Title="新建系统预设" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSystemProfile.aspx.cs" Inherits="TygaSoft.Web.Admin.SystemSet.AddSystemProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <script src="/kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="/kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息">
    <div class="easyui-panel" data-options="border:false" style="margin:10px;">
        <span class="fl" style="width:100px; text-align:right;"><b class="cr">*</b>标题：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtTitle" class="txtl" />
            <span id="error-Title" class="cr"></span>
        </div>
    </div>

    <div class="easyui-panel" data-options="border:false" style="margin:10px;">
        <span class="fl" style="width:100px; text-align:right;"><b class="cr">*</b>内容：</span>
        <div class="fl">
            <textarea id="editor1" cols="100" rows="8" style="width:800px;height:400px;visibility:hidden;"></textarea>
        </div>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />
<input type="hidden" runat="server" id="hBackToN" value="1" />
<input type="hidden" runat="server" id="hEditor1" value="" />

<script src="../../Scripts/Jeasyui.js" type="text/javascript"></script>
<script type="text/javascript">
    var editor1;
    KindEditor.ready(function (K) {
        editor1 = K.create('#editor1', {
            cssPath: '../../kindeditor/plugins/code/prettify.css',
            uploadJson: '../../Handlers/KindeditorFilesUpload.ashx',
            fileManagerJson: '../../Handlers/KindeditorFiles.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                K.ctrl(document, 13, function () {
                    //                        self.sync();
                    //                        K('form[name=form1]')[0].submit();
                });
                K.ctrl(self.edit.doc, 13, function () {
                    //                        self.sync();
                    //                        K('form[name=form1]')[0].submit();
                });
            }
        });
        prettyPrint();

    });
    </script>
<script type="text/javascript">
    $(function () {
        var hEditor1 = $("[id$=hEditor1]");
        if (hEditor1.val().length > 0) {
            $("#editor1").html(decodeURIComponent(hEditor1.val()));
        }
    })

    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {

        var txtTitle = $("[id$=txtTitle]");
        if ($.trim(txtTitle.val()).length == 0) {
            txtProductName.focus();
            $("#error-Title").text("标题为必填项");
            return false;
        }
        else {
            $("#error-Title").text("");
        }
        $("[id$=hEditor1]").val(encodeURIComponent(editor1.html()));

        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
