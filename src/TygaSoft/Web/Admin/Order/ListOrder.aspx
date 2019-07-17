<%@ Page Title="订单管理" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListOrder.aspx.cs" Inherits="TygaSoft.Web.Admin.Order.ListOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="toolbar">
    <div>
      <ul class="horizontal">
          <li>订单号： <input id="txtOrderNum" runat="server" class="txt" /></li>
          <li>订单状态: 
             <select id="cbbStatus" runat="server" class="easyui-combobox" name="cbbStatus" style="width:100px;">
                <option value="-1">全部</option>
                <option value="0">未处理</option>
                <option value="1">正在出库</option>
                <option value="10">已完成</option>
                <option value="11">已取消</option>
             </select>
          </li>
          <li>付款状态:
             <select id="cbbPayStatus" runat="server" class="easyui-combobox" name="cbbPayStatus" style="width:100px;">
                <option value="-1">全部</option>
                <option value="0">未付款</option>
                <option value="1">已付款</option>
             </select>
          </li>
          <li>
              <a href="javascript:void(0)" class="easyui-linkbutton" iconCls="icon-search" onclick="javascript:onSearch()">查 询</a>
          </li>
      </ul>
      <div class="clr"></div>
    </div>
</div>
<asp:Repeater ID="rpData" runat="server">
    <HeaderTemplate>
        <table id="bindT" class="easyui-datagrid" title="订单列表" data-options="rownumbers:true,toolbar:'#toolbar'" style="height:auto;">
        <thead>
            <tr>
                <th data-options="field:'f0',checkbox:true"></th>
                <th data-options="field:'OrderNum'">订单号</th>
                <th data-options="field:'TotalPrice'">商品总金额</th>
                <th data-options="field:'PayPrice'">应支付金额</th>
                <th data-options="field:'PayOption'">支付方式</th>
                <th data-options="field:'PayStatus'">支付状态</th>
                <th data-options="field:'PayDate'">付款时间</th>
                <th data-options="field:'Status'">订单状态</th>
                <th data-options="field:'LastUpdatedDate'">最后更新日期</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>           
                <td><%#Eval("OrderId")%></td>
                <td><%#Eval("OrderNum")%></td>
                <td><%#Eval("TotalPrice")%></td>
                <td><%#Eval("PayPrice")%></td>
                <td><%#Eval("PayOption")%></td> 
                <td><%#Eval("PayStatus")%></td> 
                <td><%#Eval("PayDate").ToString().Replace("1904/10/19 0:00:00","未付款")%></td>            
                <td><%#Eval("Status")%></td>
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
