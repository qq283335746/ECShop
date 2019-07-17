<%@ Page Title="成员管理" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListUsers.aspx.cs" Inherits="TygaSoft.Web.Admin.Members.ListUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar">
    用户名：<input type="text" runat="server" id="txtUserName" maxlength="50" class="txt" />
    <a id="btnSearch" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'">查 询</a>
</div>

<asp:Repeater ID="rpData" runat="server" OnItemDataBound="rpData_ItemDataBound" OnItemCommand="rpData_ItemCommand">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="用户列表" data-options="rownumbers:true,singleSelect:true,toolbar:'#toolbar'" style="height:auto;">
        <thead>
            <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1'">用户名</th>
            <th data-options="field:'f2'">电子邮箱</th>
            <th data-options="field:'f3'">创建日期</th>
            <th data-options="field:'f4'">最后一次登录时间</th>
            <th data-options="field:'f5'">是否锁定</th>
            <th data-options="field:'f6'">角色</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#Eval("UserName")%></td>
            <td><%#Eval("UserName")%></td><td><%#Eval("Email")%></td><td><%#Eval("CreationDate")%></td>
            <td><%#Eval("LastLoginDate")%></td>
            <td><asp:LinkButton ID="lbtnUnlockUser" runat="server" Enabled="false" CommandName="unlock" CommandArgument='<%#Eval("UserName") %>' Text='<%#Eval("IsLockedOut").ToString() == "1" ? "已锁定":"正常" %>' /></td>
            <td><a href='AddUserRole.aspx?uName=<%#HttpUtility.UrlEncode(Eval("UserName").ToString()) %>'>查看</a></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody></table>
        <%#rpData.Items.Count == 0 ? "<div class='tc m10'>暂无数据记录！</div>" : "" %>
    </FooterTemplate>
</asp:Repeater>
      
<asp:AspNetPager ID="AspNetPager1" runat="server" Width="100%" UrlPaging="false" CssClass="pages mt10" CurrentPageButtonClass="cpb"
    ShowPageIndexBox="Never" PageSize="50" OnPageChanged="AspNetPager1_PageChanged"
    EnableTheming="true" FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PrevPageText="上一页" 
    ShowCustomInfoSection="Never" CustomInfoHTML="当前第%CurrentPageIndex%页/共%PageCount%页，每页显示%PageSize%条">
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
            window.location = "AddUser.aspx";
        })
        $("#abtnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddUser.aspx";
        })
        $("#btnSearch").click(function () {
            $("[id$=hV]").val($("[id$=txtUserName]").val());
            $("[id$=hOp]").val("search");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })
        $("#abtnDel").click(function () {
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
                    var cbl = $('#bindT').datagrid("getSelections");
                    var cblLen = cbl.length;
                    if (cblLen == 1) {
                        $("[id$=hV]").val(cbl[0].f0);
                        $("[id$=hOp]").val("del");
                        __doPostBack('ctl00$cphMain$lbtnPostBack', '');
                    }
                    else {
                        alert("请选择一行且仅一行数据进行操作");
                    }
                }
            });
        })
    })
</script>

</asp:Content>
