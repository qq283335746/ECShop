<%@ Page Title="收货地址管理" Language="C#" MasterPageFile="~/Users/Users.Master" AutoEventWireup="true" CodeBehind="ListAddress.aspx.cs" Inherits="TygaSoft.Web.Users.ListAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar">
    <a href="javascript:void(0)" onclick="OnSetDefatlt()" class="easyui-linkbutton" data-options="iconCls:'icon-save'">设为默认</a>
</div>
<asp:Repeater ID="rpData" runat="server">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="用户列表" data-options="rownumbers:true,toolbar:'#toolbar'" style="height:auto;">
        <thead>
            <tr>
            <th data-options="field:'f0',checkbox:true"></th>
            <th data-options="field:'f1'">收货人</th>
            <th data-options="field:'f2'">收货地址</th>
            <th data-options="field:'f3'">联系方式</th>
            <th data-options="field:'f4'">电子邮箱</th>
            <th data-options="field:'f5'">是否默认</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#Eval("NumberID")%></td>
            <td><%#Eval("Receiver")%></td>
            <td><%#Eval("ProvinceCity")%> <%#Eval("Address")%></td>
            <td><%#Eval("Mobilephone")%>  <%#Eval("Telephone")%></td>
            <td><%#Eval("Email")%></td>
            <td><%#(bool)Eval("IsDefault") == true?"是":"否"%></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody></table>
        <%#rpData.Items.Count == 0 ? "<div class='tc m10'>暂无数据记录！</div>" : "" %>
    </FooterTemplate>
</asp:Repeater>

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
            window.location = "AddAddress.aspx";
        })
        $("#abtnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddAddress.aspx";
        })
        $("#abtnEdit").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            var cblLen = cbl.length;
            if (cblLen == 1) {
                window.location = "AddAddress.aspx?nId=" + cbl[0].f0 + "";
            }
            else {
                alert("请选择一行且仅一行进行编辑");
            }
        })
        $("#abtnDel").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            if (cbl.length == 0) {
                alert("请勾选一行或多行数据再进行操作");
                return false;
            }
            $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
                if (r) {
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

    function OnSetDefatlt() {
        var cbl = $('#bindT').datagrid("getSelections");
        var cblLen = cbl.length;
        if (cblLen == 1) {
            if (($("[id$=hOp]").val() == "setDefault") && ($("[id$=hV]").val() == cbl[0].f0)) {
                return false;
            }
            $("[id$=hV]").val(cbl[0].f0);
            $("[id$=hOp]").val("setDefault");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }
        else {
            alert("请选择一行且仅一行数据进行操作");
        }
    }

</script>

</asp:Content>
