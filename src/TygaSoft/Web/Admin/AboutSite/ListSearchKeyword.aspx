<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListSearchKeyword.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.ListSearchKeyword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<asp:Repeater ID="rpData" runat="server">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="搜索关键字列表" data-options="rownumbers:true,toolbar:'#tb'" style="height:auto;">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1'">搜索名称</th>
                <th data-options="field:'f2'">累计次数</th>
                <th data-options="field:'f3'">数据个数</th>
                <th data-options="field:'f4'">最近更新时间</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%#Eval("NumberID")%></td>              
                <td><%#Eval("SearchName")%></td>
                <td><%#Eval("TotalCount")%></td>
                <td><%#Eval("DataCount")%></td>
                <td><%#Eval("LastUpdatedDate")%></td>
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
        搜索名称: <input id="txtName" runat="server" class="txt" />
        <a href="javascript:void(0)" class="easyui-linkbutton" iconCls="icon-search" onclick="javascript:onSearch()">查 询</a>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" />
<input type="hidden" id="hOp" runat="server" value="" />
<input type="hidden" id="hV" runat="server" value="" />

<script src="../../Scripts/Jeasyui.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {

        $("#btnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddProduct.aspx";
        })
        $("#abtnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddSearchKeyword.aspx";
        })

        $("#abtnEdit").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            var cblLen = cbl.length;
            if (cblLen == 1) {
                window.location = "AddSearchKeyword.aspx?nId=" + cbl[0].f0 + "";
            }
            else {
                $.messager.alert('错误提醒', '请选择一行且仅一行进行编辑', 'error');
            }
        })

        $("#abtnDel").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            var cblLen = cbl.length;
            if (cblLen == 0) {
                $.messager.alert('错误提醒', '请至少选择一行数据再进行操作', 'error');
                return false;
            }
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    var itemsAppend = "";
                    for (var i = 0; i < cbl.length; i++) {
                        itemsAppend += cbl[i].f0 + ",";
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
