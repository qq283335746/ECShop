<%@ Page Title="角色列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListRoles.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.ListRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div id="toolbar">
      角色名：<input type="text" runat="server" id="txtRoleName" maxlength="50" class="txt" />
      <a id="btnSearch" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'">查 询</a>
    </div>

<asp:Repeater ID="rpData" runat="server" OnItemDataBound="rpData_ItemDataBound">
<HeaderTemplate>
    <table id="bindT" class="easyui-datagrid" title="角色列表" data-options="rownumbers:true,toolbar:'#toolbar'" style="height:auto;">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f1'">
                    角色名称
                </th>
                <th data-options="field:'f2'">
                    操作
                </th>
            </tr>
        </thead>
</HeaderTemplate>
<ItemTemplate>
    <tr>
        <td><%#Eval("RoleName") %> </td>
        <td>
            <%#Eval("RoleName") %>
        </td>
        <td>
            <a href='AddRoleUser.aspx?rName=<%#HttpUtility.UrlEncode(Eval("RoleName").ToString()) %>'>分配用户</a>
        </td>
    </tr>
</ItemTemplate>
<FooterTemplate>
    </table>
    <%#rpData.Items.Count == 0 ? "<div class='tc mt10'>暂无数据记录！</div>" : "" %>
</FooterTemplate>
</asp:Repeater>

<asp:AspNetPager ID="AspNetPager1" runat="server" Width="100%" UrlPaging="false" CssClass="pages"
CurrentPageButtonClass="cpb" ShowPageIndexBox="Never" PageSize="50" OnPageChanged="AspNetPager1_PageChanged"
EnableTheming="true" FirstPageText="首页" LastPageText="末页" NextPageText="下一页"
PrevPageText="上一页" ShowCustomInfoSection="Never" CustomInfoHTML="当前第%CurrentPageIndex%页/共%PageCount%页，每页显示%PageSize%条">
</asp:AspNetPager>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command"/>
<input type="hidden" runat="server" id="hOp" value="" />
<input type="hidden" runat="server" id="hV" value="" />
<input type="hidden" id="hIsUpdate" value="0" />
    
<script type="text/javascript">
    $(function () {
        if ($("#hIsUpdate").val() == 1) {
            $("[id$=hOp]").val("reload");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }
        $("#btnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddRole.aspx";
        })
        $("#abtnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddRole.aspx";
        })

        $("#btnSearch").click(function () {
            $("[id$=hV]").val($("[id$=txtUserName]").val());
            $("[id$=hOp]").val("search");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })
        $("#abtnEdit").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            var cblLen = cbl.length;
            if (cblLen == 1) {
                window.location = "AddCategory.aspx?cId=" + cbl[0].f0 + "";
            }
            else {
                alert("请选择一行且仅一行进行编辑");
            }
        })
        $("#abtnDel").click(function () {
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    var cbl = $('#bindT').datagrid("getSelections");

                    var itemsAppend = "";
                    for (var i = 0; i < cbl.length; i++) {
                        itemsAppend += cbl[i].f0 + ",";
                    }
                    $("[id$=hV]").val(itemsAppend);
                    $("[id$=hOp]").val("del");
                    __doPostBack('ctl00$cphMain$lbtnPostBack', '');
                }
            });
        })
    })
</script>
</asp:Content>
