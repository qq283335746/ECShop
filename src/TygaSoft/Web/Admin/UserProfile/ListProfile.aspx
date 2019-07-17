<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListProfile.aspx.cs" Inherits="TygaSoft.Web.Admin.UserProfile.ListProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<asp:Repeater ID="rpData" runat="server">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="用户配置文件列表" data-options="rownumbers:true,toolbar:'#tb'" style="height:auto;">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1'">用户</th>
                <th data-options="field:'f2'">最近活动时间</th>
                <th data-options="field:'f3'">最近更新时间</th>
                <th data-options="field:'f4'">是否匿名用户</th>
                <th data-options="field:'f5'">大小</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%#Eval("UserName")%></td>              
                <td><%#Eval("UserName")%></td>
                <td><%#Eval("LastActivityDate")%></td>
                <td><%#Eval("LastUpdatedDate")%></td>
                <td><%#Eval("IsAnonymous").ToString() == "True"?"是":"否"%></td>
                <td><%#Eval("Size")%></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate></tbody></table>
        <%#rpData.Items.Count == 0 ? "<div class='tc m10'>暂无数据记录！</div>" : "" %>
    </FooterTemplate>
</asp:Repeater>
      
<asp:AspNetPager ID="AspNetPager1" runat="server" Width="100%" UrlPaging="false" CssClass="pages mt10" CurrentPageButtonClass="cpb"
    ShowPageIndexBox="Never" PageSize="50" OnPageChanged="AspNetPager1_PageChanged"
    EnableTheming="true" FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PrevPageText="上一页" 
    ShowCustomInfoSection="Never" CustomInfoHTML="当前第%CurrentPageIndex%页/共%PageCount%页，每页显示%PageSize%条">
</asp:AspNetPager>

 <div id="tb" style="padding:5px;height:auto">
    <div>
        <input type="checkbox" runat="server" id="cbIsAnon" name="cbIsAnon" checked="checked" />
        <label style="margin-right:10px;">匿名用户</label> 
        截止日期: <input id="txtDueDate" runat="server" class="easyui-datebox" style="width:100px" />
        <a href="javascript:void(0)" class="easyui-linkbutton" iconCls="icon-search" onclick="javascript:onSearch()">查 询</a>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" />
<input type="hidden" id="hOp" runat="server" value="" />
<input type="hidden" id="hV" runat="server" value="" />

<script type="text/javascript">
    $(function () {
        $("#abtnDel").click(function () {
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    var cbl = $('#bindT').datagrid("getSelections");
                    var itemsAppend = "";
                    for (var i = 0; i < cbl.length; i++) {
                        itemsAppend += cbl[i].f0 + "|";
                    }

                    $("[id$=hV]").val(itemsAppend);
                    $("[id$=hOp]").val("OnDel");

                    __doPostBack('ctl00$cphMain$lbtnPostBack', '');
                }
            });
        })
    })
    function onSearch() {
        $("[id$=hOp]").val("OnSearch");

        __doPostBack('ctl00$cphMain$lbtnPostBack', '');
    }
</script>

</asp:Content>
