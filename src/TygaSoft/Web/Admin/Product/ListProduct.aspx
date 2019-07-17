<%@ Page Title="产品列表" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListProduct.aspx.cs" Inherits="TygaSoft.Web.Admin.Product.ListProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar">
  <ul>
      <li class="fl">产品名称：<input type="text" runat="server" id="txtProductName" maxlength="256" class="txt" /></li>
      <li class="fl ml10">
          <a id="abtnSearch" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'">查 询</a>
      </li>
  </ul>
  <div class="clr"></div>  
</div>

<asp:Repeater ID="rpData" runat="server" OnItemCommand="rpData_ItemCommand">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="产品列表" data-options="rownumbers:true,toolbar:'#toolbar'" style="height:auto;">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'f5'">产品图片</th>
                <th data-options="field:'f1'">产品名称</th>
                <th data-options="field:'f2'">所属分类</th>
                <th data-options="field:'f3'">副标题</th>
                <th data-options="field:'f4'">价格(￥)</th>
                <th data-options="field:'f6'">库存</th>
                <th data-options="field:'f7'">支付方式</th>
                <th data-options="field:'f8'">创建日期</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td><%#Eval("ProductId")%></td>
                <td><img src='<%#VirtualPathUtility.MakeRelative("~/Admin/Product/ListProduct.aspx", Eval("ImagesUrl").ToString())%>' alt="" style=" width:50px; height:50px;" /> </td>
                <td><%#Eval("ProductName").ToString().Length > 25?Eval("ProductName").ToString().Substring(0,25)+"...":Eval("ProductName".ToString())%></td>
                <td><%#Eval("CategoryName")%></td>
                <td><%#Eval("Subtitle").ToString().Length > 15 ? Eval("Subtitle").ToString().Substring(0, 15) + "..." : Eval("Subtitle".ToString())%></td>
                <td><%#Eval("ProductPrice")%></td>
                <td><%#Eval("StockNum")%></td>
                <td><%#Eval("PayOptions")%></td>
                <td><%#Eval("CreateDate")%></td>
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

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command"/>
<input type="hidden" runat="server" id="hOp" value="" />
<input type="hidden" runat="server" id="hV" value="" />
<input type="hidden" id="hIsUpdate" value="0" />

<script src="/Scripts/Jeasyui.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        if ($("#hIsUpdate").val() == 1) {
            $("[id$=hOp]").val("reload");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }

        $("#btnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddProduct.aspx";
        })
        $("#abtnNew").click(function () {
            $("#hIsUpdate").val("1");
            window.location = "AddProduct.aspx";
        })

        $("#abtnEdit").click(function () {
            var cbl = $('#bindT').datagrid("getSelections");
            var cblLen = cbl.length;
            if (cblLen == 1) {
                window.location = "AddProduct.aspx?nId=" + cbl[0].f0 + "";
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
                    $("[id$=hOp]").val("del");
                    __doPostBack('ctl00$cphMain$lbtnPostBack', '');
                }
            });
        })

        $("#abtnSearch").click(function () {
            $("[id$=hOp]").val("search");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })

    }) 
</script>

</asp:Content>
