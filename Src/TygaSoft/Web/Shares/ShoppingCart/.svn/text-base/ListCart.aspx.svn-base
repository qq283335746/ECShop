<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListCart.aspx.cs" Inherits="TygaSoft.Web.Shares.ShoppingCart.ListCart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="cart" style="background:#FFFFFF;padding:10px 20px 20px 20px;">
  <div class="cart-hd group">
	 <h2>我的购物车</h2>
	 <span id="show2" class="fore"></span>
  </div>
  <div class="cart-inner">
     <div class="cart-thead clearfix">
        <div class="column t-checkbox form">
        <input type="checkbox" id="cbAll" checked="checked" />
        <label for="cbAll">全选</label>
        </div>
        <div class="column t-goods">商品</div>
        <div class="column t-price">价格</div>
        <div class="column t-quantity">数量</div>
        <div class="column t-action">操作</div>
    </div>
    
     <asp:Repeater runat="server" ID="rpData" OnItemCommand="rpData_ItemCommand">
    <HeaderTemplate>
      <div id="product-list" class="cart-tbody">
        <div class="item-meet meet-give">
    </HeaderTemplate>
      <ItemTemplate>  
            <div class="item item_selected">
              <div class="item_form clearfix">
                 <div class="cell p-checkbox">
                     <input type="checkbox" name="cbItem" id="cbItem" runat="server" value='<%#Eval("ProductId") %>' />
                 </div>
                 <div class="cell p-goods">
                    <div class="p-img">
                      <a href='../ShowProduct.aspx?pId=<%#Eval("ProductId") %>' target="_blank">
                        <img src='<%#Eval("ImagesUrl").ToString().Replace("~","") %>' alt="" style="width:50px; height:50px;">
                      </a>
                    </div>
                    <div class="p-name">
                      <a href="../ShowProduct.aspx?pId=<%#Eval("ProductId") %>" target="_blank"><%#Eval("ProductName") %></a>
                    </div>
                 </div>
                 <div class="cell p-price"><span class="price">¥<%#Eval("Price") %></span></div>
                 <div class="cell p-quantity">
                     <div class="quantity-form">
                        <a href="javascript:void(0);" class="decrement">-</a>
                        <input type="text" runat="server" id="txtQuantity" class="quantity-text" value='<%#Eval("Quantity") %>' />
                        <a href="javascript:void(0);" class="increment">+</a>
                     </div>
                 </div>
                 <div class="cell p-remove">
                   <asp:LinkButton runat="server" ID="lbtnDel" CssClass="cart-remove" CommandName="del" CommandArgument='<%#Eval("ProductId") %>' OnClientClick="return confirm('确定要删除吗？')" Text="删除"></asp:LinkButton>
                 </div>
              </div>
            </div>
          
      </ItemTemplate>
      <FooterTemplate>
       <%#rpData.Items.Count == 0 ? "<div class='tc item item_selected item-last'>暂无数据记录！</div>" : ""%>
       </div></div>       
        <div class="cart-toolbar clearfix">
          <div class="control fl">
            <span class="delete">
            <b></b>
            <asp:LinkButton ID="lbtnDelBatch" runat="server" CommandName="delBatch" Text="删除选中的商品" OnClientClick="return confirm('确定要删除吗？')" />
            </span>
          </div>
          <div class="total fr"><p><span id="totalSkuPrice">¥<%#GetTotalPrice()%></span>总计：</p></div>
          <div class="amout fr"><span id="totalCount"><%#GetCount()%></span> 件商品</div>
        </div>
        <div class="cart-total clearfix">
            <div class="total fr"><span id="finalPrice">¥<%#GetTotalPrice()%></span>总计（不含运费）：</div>
        </div>
        
      </FooterTemplate>
    </asp:Repeater>
  </div>
  <div class="cart-button clearfix">
       <a class="btn continue" href="../../Default.aspx" id="continue"><span class="btn-text">继续购物</span></a>
       <a href="javascript:void(0)" id="abtnTopay" class="checkout" onclick="OnToPay()">去结算</a>
   </div>
</div>

<asp:LinkButton runat="server" ID="lbtnToPay" OnCommand="btn_Command" CommandName="lbtnTopay"></asp:LinkButton>

<script type="text/javascript">
    $(function () {
        $("#product-list .item").filter(":last").addClass("item-last");

        var cbAll = $("#cbAll");
        var cbList = $("#product-list").find("[name$=cbItem]");
        if (cbList != undefined && cbList.length > 0) {
            cbList.attr("checked", cbAll.attr("checked"));
            cbAll.click(function () {
                cbList.attr("checked", this.checked);
            })

            cbList.click(function () {
                cbAll.attr('checked', cbList.length == cbList.filter(':checked').length);
            })

            setInterval(setPriceByQty, 300);
        }
    })

    $("#product-list .decrement").click(function () {
        var txtQuantity = $(this).next();
        var n = parseInt(txtQuantity.val());
        var r = /\d+/;
        if (!r.test(n)) {
            txtQuantity.val(1);
        }
        else {
            n = n - 1;
            if (n < 1) n = 1;
            txtQuantity.val(n);
        }
    })
    $("#product-list .increment").click(function () {
        var txtQuantity = $(this).prev();
        var n = parseInt(txtQuantity.val());
        var r = /\d+/;
        if (!r.test(n)) {
            txtQuantity.val(1);
        }
        else {
            n = n + 1;
            txtQuantity.val(n);
        }
    })

    $("#product-list .quantity-text").blur(function () {
        var txtQuantity = $(this);
        var n = parseInt(txtQuantity.val());
        var r = /\d+/;
        if (!r.test(n)) {
            txtQuantity.val(1);
        }
    })

    //    舍入处理，保留小数点后若干位
    //    f    要处理的浮点数
    //    p    小数点后数字个数
    //    return      返回处理后的字符串
    function roundDecimal(f, p) {
        var f_x = parseFloat(f);
        if (isNaN(f_x)) {
            return false;
        }
        var f_x = Math.round(f * 100) / 100;
        var s_x = f_x.toString();
        var pos_decimal = s_x.indexOf('.');
        if (pos_decimal < 0) {
            pos_decimal = s_x.length;
            s_x += '.';
        }
        while (s_x.length <= pos_decimal + p) {
            s_x += '0';
        }
        return s_x;
    }

    function setPriceByQty() {
        var cbList = $("#product-list").find("[name$=cbItem]");
        var ckItems = cbList.filter(':checked');
        var totalCount = 0;
        var totalPrice = 0;
        ckItems.each(function () {
            var currentRow = $(this).parent().parent();
            var txtQuantity = currentRow.find("[name$=txtQuantity]");
            var n = parseInt(txtQuantity.val());
            var r = /\d+/;
            if (!r.test(n)) {
                txtQuantity.val(1);
            }
            totalCount = totalCount + n;
            var objPrice = currentRow.find("[class=price]");
            var price = parseFloat(objPrice.text().substring(1));
            price = price * n;
            totalPrice = totalPrice + price;
        })
        price = roundDecimal(totalPrice, 2);
        $("#totalSkuPrice").text(totalPrice);
        $("#finalPrice").text(totalPrice);
        $("#totalCount").text(totalCount);
    }

    function OnToPay() {
        var cbList = $("#product-list").find("[name$=cbItem]");
        if (cbList.filter(':checked').length > 0) {
            __doPostBack('ctl00$cphMain$lbtnToPay', '');
        }
        else {
            alert("请选择商品后去结算");
        }
    }

</script>

</asp:Content>
