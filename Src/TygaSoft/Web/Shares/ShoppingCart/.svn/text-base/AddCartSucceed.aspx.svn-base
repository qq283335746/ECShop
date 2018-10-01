<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCartSucceed.aspx.cs" Inherits="TygaSoft.Web.Shares.ShoppingCart.AddCartSucceed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div style=" background:#FFFFFF; height:100% auto;">
<div class="fl" style=" width:80%; margin-right:1%;">
  <div class="cart-success">
    <b>商品已成功加入购物车！</b>
    <span id="initCart_next_go">
      <a href="ListCart.aspx" class="btn-pay">去结算</a>
      <a href="javascript:history.back();" class="btn-continue">继续购物</a>
    </span>
  </div>
</div>
<div class="fl" style=" width:19%;">
  <asp:Repeater runat="server" ID="rpCart">
      <HeaderTemplate>
        <div id="cart_detail" class="cart-detail">
          <div class="mt">
		     <h2><s></s>我的购物车</h2>
	      </div>
          <div id="cart_content" class="mc">
      </HeaderTemplate>
      <ItemTemplate>
        <%--<h3>刚加入购物车的商品</h3>
        <dl class="new">
            <dt class="p-img"><a href="http://item.jd.com/539349.html" target="_blank">
               <img src="http://img10.360buyimg.com/n5/3372/9a2815da-934d-4aaf-8be9-c57a8e9b832a.jpg" alt=""></a>
            </dt>
            <dd class="p-info">
                <div class="p-name">
                   <a href="http://item.jd.com/539349.html" target="_blank">
                     <span style="color:red"></span>苹果（APPLE）iPhone 4 8G版 3G手机（白色）WCDMA/GSM
                   </a>
                </div>
                <div class="p-price">
                   <span style="font-weight:bold;color:red">2698.00</span>
                   <em>×3</em>
                </div>
              </dd>
        </dl>
        <h3>您购物车中的其它商品</h3>--%>
        <dl class="old">
            <dt class="p-img">
               <a href='../ShowProduct.aspx?pId=<%#Eval("ProductId")%>' target="_blank">
                 <img src='<%#Eval("ImagesUrl").ToString().Replace("~","") %>' style="width:50px; height:50px;" alt="">
               </a>
            </dt>
         <dd class="p-info">
            <div class="p-name">
               <a href='../ShowProduct.aspx?pId=<%#Eval("ProductId")%>' target="_blank">
                 <span style="color:red"></span><%#Eval("ProductName")%>
               </a>
            </div>
         <div class="p-price">
            <span style="font-weight:bold;color:red"><%#Eval("Price")%></span>
              <em>×<%#Eval("Quantity")%></em>
         </div>
        </dd>
        </dl>
      </ItemTemplate>
      <FooterTemplate>
            <div class="total">
              共<strong id="skuCount"><%#GetCount()%></strong>件商品<br>
              金额总计：<strong><%#GetTotalPrice()%></strong>
           </div>
           <div class="btns">
             <a href="ListCart.aspx" class="btn-pay">去结算</a>
           </div>
         </div>
        </div>
      </FooterTemplate>
  </asp:Repeater> 
</div>
<div class="clr"></div>
</div>
</asp:Content>
