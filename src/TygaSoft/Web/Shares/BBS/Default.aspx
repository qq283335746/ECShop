<%@ Page Title="" Language="C#" MasterPageFile="~/Shares/BBS/Bbs.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TygaSoft.Web.Shares.BBS.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
<link href="/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
<script type="text/javascript" src="/kindeditor/kindeditor.js"></script>
<script type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
<script type="text/javascript" src="/kindeditor/plugins/code/prettify.js"></script>
<script type="text/javascript" src="/Scripts/Bbs/BbsDefault.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar">
    <a class="easyui-linkbutton" data-options="iconCls:'icon-add',split:false,plain:true">发帖</a>
    <a class="easyui-linkbutton" data-options="iconCls:'icon-edit',split:false,plain:true">回复</a>
</div>

<div style="border:solid 1px #95B8E7;">
<asp:Repeater ID="rpData" runat="server">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" data-options="fit:true, rownumbers:true,border:false, toolbar:'#toolbar'">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1',sortable:true,width:100">标题</th>
                <th data-options="field:'f2',sortable:true,width:120">最近更新时间</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%#Eval("NumberID")%></td>
                <td><a href='AddContent.aspx?nId=<%#Eval("NumberID")%>'><%#Eval("Title")%></a> </td>
                <td><%#Eval("LastUpdatedDate")%></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate></tbody></table>
        <%#rpData.Items.Count == 0 ? "<div class='tc m10'>暂无数据记录！</div>" : "" %>
    </FooterTemplate>
</asp:Repeater>
</div>
      
<asp:AspNetPager ID="AspNetPager1" runat="server" Width="100%" UrlPaging="false" CssClass="pages mt10" CurrentPageButtonClass="cpb"
    ShowPageIndexBox="Never" PageSize="50" OnPageChanged="AspNetPager1_PageChanged"
    EnableTheming="true" FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PrevPageText="上一页" 
    ShowCustomInfoSection="Never" CustomInfoHTML="当前第%CurrentPageIndex%页/共%PageCount%页，每页显示%PageSize%条">
</asp:AspNetPager>

<div class="mt10">
<div class="easyui-panel" title="快速发帖">
    <div class="row mt10">
        <span class="fl rl"><b class="cr">*</b>标题：</span>
        <div class="fl">
            <input type="text" runat="server" id="txtTitle" class="easyui-validatebox txtl" data-options="required:true,missingMessage:'必填项',validType:'length[2,3]'" />
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mt10">
        <span class="fl rl">说说几句：</span>
        <div class="fl">
            <textarea id="txtContent" cols="100" rows="8" data-options="required:true,missingMessage:'必填项',validType:'length[1,1000]'" style="height:200px;visibility:hidden;"></textarea>
        </div>
        <div class="clr"></div>
    </div>
    <div class="row mtb10">
        <span class="fl rl">&nbsp</span>
        <div class="fl">
           <a href="javascript:;" class="easyui-linkbutton" data-options="iconCls:'icon-ok',split:false">提 交</a> 
        </div>
        <div class="clr"></div>
    </div>
</div>
</div>

<script type="text/javascript">
    var editor1;
    KindEditor.ready(function (K) {
        editor1 = K.create('#txtContent', {
            cssPath: '/kindeditor/plugins/code/prettify.css',
            uploadJson: '/Handlers/KindeditorFilesUpload.ashx',
            fileManagerJson: '/Handlers/KindeditorFiles.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                K.ctrl(document, 13, function () {
                });
                K.ctrl(self.edit.doc, 13, function () {
                });
            }
        });
        prettyPrint();

    });

    $(function () {
        BbsDefault.init();
    })
</script>

</asp:Content>
