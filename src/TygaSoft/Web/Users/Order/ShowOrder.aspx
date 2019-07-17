<%@ Page Title="订单详情" Language="C#" MasterPageFile="~/Users/Users.Master" AutoEventWireup="true" CodeBehind="ShowOrder.aspx.cs" Inherits="TygaSoft.Web.Users.Order.ShowOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<asp:Repeater runat="server" ID="rpData">
  <ItemTemplate>

<div class="easyui-panel" style="border-left:none; border-right:none; border-top:none;">
  <span style="color:#cc0000;">订单状态：<%#Eval("StatusName")%></span> 
</div>
<div class="mtb10">商品信息</div>
<table id="bindT" class="easyui-datagrid" data-options="rownumbers:false" style="height:auto;">
   <thead>
      <tr>
        <th data-options="field:'f1'">商品名称</th>
        <th data-options="field:'f2'">价格</th>
        <th data-options="field:'f3'">商品数量</th>
      </tr>
   </thead>
    <tbody>
      <asp:Literal runat="server" ID="ltrProducts"></asp:Literal>
    </tbody>
</table>
<div class="mt10"></div>
<div class="easyui-panel" title="订单信息">
  <div class="row mt10">
     <span class="fl rl">订单编号：</span>
     <div class="fl"><%#Eval("OrderNum") %></div>
     <div class="clr"></div>
  </div>
  <div class="row mt10">
     <span class="fl rl">支付方式：</span>
     <div class="fl"><%#Eval("PayOption") %></div>
     <div class="clr"></div>
  </div>
  <div class="row mt10 mb10">
     <span class="fl rl">取消时间：</span>
     <div class="fl"><%#Eval("LastUpdatedDate") %></div>
     <div class="clr"></div>
  </div>
  <div class="mt10 p10" style="border-top:solid 1px #95B8E7;">
      <span style="margin-left:5px;">收货人信息</span> 
      <div class="row mt10">
         <span class="fl rl">收货人姓名：</span>
         <div class="fl"><%#Eval("Receiver")%></div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl">地址：</span>
         <div class="fl"><%#Eval("ProviceCity")%> <%#Eval("Address")%></div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl">手机号码：</span>
         <div class="fl"><%#Eval("Mobilephone")%> </div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl">固定电话：</span>
         <div class="fl"><%#Eval("Telephone")%></div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl">电子邮件：</span>
         <div class="fl"><%#Eval("Email")%> </div>
         <div class="clr"></div>
      </div>
  </div>

</div>

  </ItemTemplate>
</asp:Repeater>

</asp:Content>
