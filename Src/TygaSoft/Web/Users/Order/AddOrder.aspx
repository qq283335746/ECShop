<%@ Page Title="订单结算-天涯孤岸" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrder.aspx.cs" Inherits="TygaSoft.Web.Users.Order.AddOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>
<link href="../../Jquery/plugins/jeasyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
<link href="../../Jquery/plugins/jeasyui/themes/icon.css" rel="stylesheet" type="text/css" />
<link href="../../Styles/ProviceCity.css" rel="Stylesheet" type="text/css" />
<script src="../../Jquery/plugins/jeasyui/jquery.easyui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="pAddOrder">
<div id="checkout">
  <div class="mt">
	<h2>填写并核对订单信息</h2>
  </div>
  <div id="wizard">
      <div id="step-1" class="step">
          <div class="step-title">
            <strong>收货人信息</strong>
            <span class="step-action" id="consignee_edit_action">
              <a href="javascript:void(0)" id="editConsignee">[修改]</a>
            </span>
          </div>
          <div class="step-content">
              <div class="sbox-wrap">
                <div class="sbox">
                  <div id="" class="s-content">
                    <asp:Literal runat="server" ID="ltrHasAddress"></asp:Literal>
                  </div>
                </div>
              </div>
              <div id="consignee" style="display:none;">
                  <div class="sbox">
                    <div class="form">
                    <div id="consignee-list">
                        <asp:Literal runat="server" ID="ltrAddress"></asp:Literal>
                    </div>
                    <div id="use-new-address" class="item">
                         <input type="radio" class="hookbox" name="consignee_radio" id="rNewAddress" value="" />
                         <label for="rNewAddress">
							    使用新地址 
                         </label>
                         <span id="lbLimitMsg" class="cr" style="display:none;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 当前地址数量已达上限，若要继续添加新地址，请先删除部分收货地址。</span>
                    </div>
                    <div id="consignee-form" style="display:none;">
                        <div class="row">
                        <span class="rl"><em>*</em>收货人：</span>
                        <div class="rr">
                            <input type="text" id="txtReceiver" runat="server" class="txt" />
                            <span id="error-Receiver" class="error"></span>
                        </div>
                        <div class="clr"></div>
                        </div>
                        <div class="row">
                        <span class="rl"><em>*</em>所在地区：</span>
                        <div class="rr">
                            <input type="text" id="txtProvinceCity" runat="server" readonly="readonly" class="txt" />
                            <span id="error-Place" class="error"></span>
                            <div id="dlgArea" style="padding:10px;"></div>
                        </div>
                        <div class="clr"></div>
                        </div>
                        <div class="row">
                        <span class="rl"><em>*</em>详细地址：</span>
                        <div class="rr">
                            <input type="text" id="txtAddress" runat="server" class="txt" style="width:316px;" />
                            <span id="error-Address" class="error"></span>
                        </div>
                        <div class="clr"></div>
                        </div>
                        <div class="row">
                            <span class="rl"><em>*</em>手机号码：</span>
                            <div class="rr">
                                <input type="text" id="txtMobilephone" runat="server" class="txt" />
                                <span id="error-Mobilephone" class="ml10">手机或固定电话至少填写一个</span>
                            </div>
                            <div class="clr"></div>
                        </div>
                        <div class="row">
                            <span class="rl">固定电话：</span>
                            <div class="rr">
                                <input type="text" id="txtTelephone" runat="server" class="txt" />
                            </div>
                            <div class="clr"></div>
                        </div>
                        <div class="row">
                            <span class="rl">电子邮箱：</span>
                            <div class="rr">
                                <input type="text" id="txtEmail" runat="server" class="txt" />
                                <span id="error-Email" class="ml10">用来接收订单提醒邮件，便于您及时了解订单状态</span>
                            </div>
                            <div class="clr"></div>
                        </div>
                    </div>
                    <div class="form-btn group">
                        <a href="javascript:void(0)" onclick="OnSave()" class="btn-submit"><span>保存收货人信息</span></a>
                    </div>
                    </div>
                  </div>
              </div>
          </div>
      </div>
      <div id="step-2" class="step">
          <div class="step-title">
            <strong>支付及配送方式 </strong>
            <span class="step-action">
              <a href="javascript:void(0)" id="editPayment">[修改]</a>
            </span>
          </div>
          <div class="step-content">
              <div class="sbox-wrap">
                <div class="sbox">
                  <div class="s-content">
                    <p runat="server" id="selectPay">
	  		            在线支付
	  	              </p>
                  </div>
                </div>
                <div class="form">
                <div id="payment" class="payment" style="display:none;">
                    <h3>支付方式</h3>
                    <div style="padding-bottom:10px"></div>
                    <div class="item">
                        <input type="radio" class="hookbox" name="paymentRadio" id="rP1" value="货到付款" checked="checked" />
                        <label for="rP1"><b>货到付款</b>&nbsp; 送货上门后再收款，支持现金、POS机刷卡、支票支付&nbsp;</label>
                    </div>
                    <div class="item">
                        <input type="radio" class="hookbox" name="paymentRadio" id="rP2" value="在线支付" />
                        <label for="rP2"><b>在线支付</b>&nbsp; 即时到帐，支持绝大数银行借记卡及部分银行信用卡 &nbsp;</label>
                    </div>
                    <div class="item">
                        <input type="radio" class="hookbox" name="paymentRadio" id="rP3" value="转账汇款" />
                        <label for="rP3"><b>转账汇款</b>&nbsp; 直接到银行办理转账或汇款到我们的账户 &nbsp;</label>
                    </div>
                    <div class="form-btn group">
                    <a href="javascript:void(0)" onclick="OnSavePayment()" class="btn-submit"><span>保存支付及配送方式</span></a>
                </div>
                </div>
                </div>
              </div>
          </div>
      </div>
      <div id="step-3" class="step">
        <div class="step-title">
            <a href="../../Shares/ShoppingCart/ListCart.aspx" class="return-edit">返回修改购物车</a>
            <strong>商品清单</strong>
          </div>
          <div class="step-content">
              <div class="sbox-wrap">
                <div class="sbox" style="width:100%;">
                  <div class="s-content">
                     <div class="cart" style="background:#FFFFFF;padding:10px 20px 20px 20px;">
                      <div class="cart-inner">
                         <div class="cart-thead clearfix">
                            <div class="column t-goods">商品</div>
                            <div class="column t-price">价格</div>
                            <div class="column t-quantity" style="margin-left:50px;">数量</div>
                        </div>
    
                        <asp:Repeater runat="server" ID="rpData">
                        <HeaderTemplate>
                          <div id="product-list" class="cart-tbody">
                            <div class="item-meet meet-give">
                        </HeaderTemplate>
                          <ItemTemplate>  
                                <div class="item item_selected">
                                  <div class="item_form clearfix">
                                     <div class="cell p-goods">
                                        <div class="p-img" style="margin-left:10px;">
                                          <a href='../../ShowProduct.aspx?pId=<%#Eval("ProductId") %>' target="_blank">
                                            <img src='<%#Eval("ImagesUrl").ToString().Replace("~","") %>' alt='<%#Eval("ProductName") %>' style="width:50px; height:50px;">
                                          </a>
                                        </div>
                                        <div class="p-name">
                                          <a href="../../ShowProduct.aspx?pId=<%#Eval("ProductId") %>" target="_blank"><%#Eval("ProductName") %></a>
                                        </div>
                                     </div>
                                     <div class="cell p-price"><span class="price">¥<%#Eval("Price") %></span></div>
                                     <div class="cell p-quantity">
                                         <%#Eval("Quantity")%>
                                     </div>
                                  </div>
                                </div>
          
                          </ItemTemplate>
                          <FooterTemplate>
                           <%#rpData.Items.Count == 0 ? "<div class='tc item item_selected item-last'>暂无数据记录！</div>" : ""%>
                           </div></div>       
                            <div class="cart-toolbar clearfix">
                              <div class="total fr"><p><span id="totalSkuPrice">¥<%#GetTotalPrice()%></span>总计：</p></div>
                              <div class="amout fr"><span id="selectedCount"><%#GetCount()%></span> 件商品</div>
                            </div>
                          <div class="checkout-buttons group">
                            <div class="inner">
                              <a id="abtnCommit" href="javascript:void(0)" class="checkout-submit" onclick="OnCommit()"></a>
                              <span class="total">应付总额：<strong>¥<%#GetTotalPrice()%></strong>元</span>
                            </div>
                          </div>
                          </FooterTemplate>
                        </asp:Repeater>
                        
                      </div>
                      
                    </div>
                  </div>
                </div>
              </div>
          </div>
      </div>
  </div>
</div>
</div>

<input type="hidden" id="hProvinceCityUrl" value="../../Handlers/ProvinceCity.ashx" />
<asp:LinkButton runat="server" ID="lbtnSave" OnCommand="btn_Command" CommandName="lbtnsave" />
<input type="hidden" id="hOp" runat="server" value="saveAddress" />
<input type="hidden" id="hNId" runat="server" value="" />

<script type="text/javascript">
    var consigneeList = $("#consignee-list .item");
    var consigneeForm = $("#consignee-form");
    var consignee = $("#consignee");
    var editConsignee = $("#editConsignee");
    var hOp = $("[id$=hOp]");
    $(function () {
        $('#dlgArea').dialog({
            title: '省市区选择',
            width: 335,
            height: 350,
            href: "/Templates/HtmlPage/AreaTabsForOrder.htm",
            closed: true
        });
        $("[id$=txtProvinceCity]").click(function () {
            $('#dlgArea').window('open');

        })

        $("#product-list .item").filter(":last").addClass("item-last");
        $("#abtnToPay").parent().hide();

        if (consigneeList == undefined || consigneeList.length == 0) {
            $("#editConsignee").text("保存收货人信息");
            $("#step-1").addClass("step-current");
            consignee.show();
            consignee.prev().hide();
            $("#rNewAddress").attr("checked", "checked");
            consigneeForm.show();
        }
        else {
            consigneeList.click(function () {
                $(this).addClass("item-selected").siblings().removeClass("item-selected");
            })
            consigneeList.hover(function () {
                $(this).addClass("item-selected").siblings().removeClass("item-selected");
                $(this).find("[class^=item-action]").removeClass("hide");
            }, function () {
                $(this).find("[class^=item-action]").addClass("hide");
            })
            consigneeList.find("[name=consignee_radio]").each(function () {
                if ($(this).val() == $("[id$=hNId]").val()) {
                    $(this).attr("checked", "checked");
                }
                $(this).click(function () {
                    consigneeForm.hide();
                })
            })

            if (hOp.val() == "editAddress") {
                $("#step-1").addClass("step-current");
                $("#editConsignee").text("[关闭]");
                consignee.show();
                consigneeForm.show();
                consignee.prev().hide();
            }
            else if (hOp.val() == "delAddress") {
                consignee.show();
                consignee.prev().hide();
            }
        }

        $("#editConsignee").click(function () {
            if ($(this).text() == "保存收货人信息") {
            }
            else if ($(this).text() == "[修改]") {
                $(this).text("[关闭]");
                $("#step-1").addClass("step-current");
                consignee.show();
                consignee.prev().hide();
            }
            else if ($(this).text() == "[关闭]") {
                $(this).text("[修改]");
                $("#step-1").removeClass("step-current");
                consignee.hide();
                consignee.prev().show();
            }
        })

        $("[id^=abtnEdit]").click(function () {
            var nId = $(this).parent().parent().find("[type=radio]:first").val();
            $("[id$=hNId]").val(nId);
            $("[id$=hOp]").val("editGetAddress");
            __doPostBack('ctl00$cphMain$lbtnSave', '');
        })
        $("[id^=abtnDel]").click(function () {
            var nId = $(this).parent().parent().find("[type=radio]:first").val();
            $("[id$=hNId]").val(nId);
            $("[id$=hOp]").val("delAddress");
            __doPostBack('ctl00$cphMain$lbtnSave', '');
        })

        $("#rNewAddress").click(function () {
            $(this).attr("checked", "checked");
            $("[id$=hOp]").val("saveAddress");
            consigneeForm.find("input").val("");
            if (consigneeList != undefined && consigneeList.length >= 5) {
                $("#lbLimitMsg").css("display", "block");
                consigneeForm.hide();
            }
            else {
                consigneeForm.css("display", "block");
            }
        })

        var paymentList = $("#payment .item");
        var payOption = $("#payment").find("input[type=radio]:checked").val();
        $("[id$=selectPay]").text(payOption);
        paymentList.click(function () {
            $(this).addClass("item-selected").siblings().removeClass("item-selected");
        })
        paymentList.hover(function () {
            $(this).addClass("item-selected").siblings().removeClass("item-selected");
        }, function () {
        })

        $("#editPayment").click(function () {
            if ($(this).text() == "[修改]") {
                $(this).text("[关闭]");
                $("#step-2").addClass("step-current");
                $("#payment").show();
                $("#payment").parent().prev().hide();
            }
            else if ($(this).text() == "[关闭]") {
                $("#step-2").removeClass("step-current");
                $(this).text("[修改]");
                $("#step-2").removeClass("step-current");
                $("#payment").hide();
                $("#payment").parent().prev().show();
            }
        })

        setInterval(OnCheckState, 300);
    })

    function addressClosed() {
        editConsignee.text("[修改]");
        $("#step-1").removeClass("step-current");
        consignee.hide();
        consignee.prev().show();
    }

    function OnCheckForm() {
        var hasFinish = true;
        var txtReceiver = $("[id$=txtReceiver]");
        if ($.trim(txtReceiver.val()).length == 0) {
            $("#error-Receiver").text("请您填写收货人姓名");
            hasFinish = false;
        }
        else {
            $("#error-Receiver").text("");
        }
        var txtProvinceCity = $("[id$=txtProvinceCity]");
        var sPcity = $.trim(txtProvinceCity.val());
        if (sPcity.length == 0) {
            $("#error-Place").text("请选择所在地区");
            hasFinish = false;
        }
        else {
            $("#error-Place").text("");
        }
        var pArr = sPcity.split("-");
        if (pArr.length != 3) {
            $("#error-Place").text("请正确选择省市区");
            hasFinish = false;
        }
        else {
            $("#error-Place").text("");
        }
        var txtAddress = $("[id$=txtAddress]");
        if ($.trim(txtAddress.val()).length == 0) {
            $("#error-Address").text("请选择详细地址");
            txtAddress.focus();
            hasFinish = false;
        }
        else {
            $("#error-Address").text("");
        }
        if (!hasFinish) return false;
        var txtMobilephone = $("[id$=txtMobilephone]");
        var sMobilephone = $.trim(txtMobilephone.val());
        var txtTelephone = $("[id$=txtTelephone]");
        var sTelephone = $.trim(txtTelephone.val());
        if (sMobilephone.length == 0 && sTelephone.length == 0) {
            $("#error-Mobilephone").addClass("error");
            return false;
        }
        else {
            $("#error-Mobilephone").removeClass("error");
        }
        var reg = /^1[3|4|5|8][0-9]\d{4,8}$/;
        if (sMobilephone.length > 0) {
            if (!reg.test(sMobilephone)) {
                $("#error-Mobilephone").addClass("error").text("请正确输入手机号码格式");
                return false;
            }
            else {
                $("#error-Mobilephone").removeClass("error").text("手机或固定电话至少填写一个");
            }
        }
        if (sTelephone.length > 0) {
            reg = /(\(\d{3,4}\)|\d{3,4}-|\s)?\d{8}/;
            if (!reg.test(sTelephone)) {
                $("#error-Mobilephone").addClass("error").text("请正确输入电话号码格式");
                return false;
            }
            else {
                $("#error-Mobilephone").removeClass("error").text("手机或固定电话至少填写一个");
            }
        }
        var txtEmail = $("[id$=txtEmail]");
        var sEmail = $.trim(txtEmail.val());
        if (sEmail.length > 0) {
            reg = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
            if (!reg.test(sEmail)) {
                $("#error-Email").addClass("error").text("请正确输入电子邮箱格式");
                return false;
            }
            else {
                $("#error-Email").removeClass("error").text("用来接收订单提醒邮件，便于您及时了解订单状态");
            }
        }

        return true;
    }

    function OnSave() {
        var sNId = $("[id$=hNId]").val();
        var currentSelect = $("#consignee").find("input[type=radio]:checked");
        if (currentSelect.attr("id") == "rNewAddress") {
            if (!OnCheckForm()) {
                return false;
            }
            $("[id$=hOp]").val("saveAddress");
        }
        else {
            if (currentSelect.val() == sNId) {
                if ($("[id$=hOp]").val() != "editAddress") {
                    addressClosed();
                    return false;
                }
            }
            else {
                $("[id$=hOp]").val("changeAddress");
                $("[id$=hNId]").val(currentSelect.val());
            }
        }
        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }

    function OnSavePayment() {
        var payOption = $("#payment").find("input[type=radio]:checked").val();
        $("[id$=selectPay]").text(payOption);
        $("#payment").hide();
        $("#payment").parent().prev().show();
        $("#editPayment").text("[修改]");
        $("#step-2").removeClass("step-current");
    }

    function OnCheckState() {
        var editConsignee = $("#editConsignee");
        var editPayment = $("#editPayment");
        var abtnCommit = $("#abtnCommit");
        if ($("#step-1").hasClass("step-current")) {
            if (abtnCommit.hasClass("checkout-submit")) {
                abtnCommit.removeClass("checkout-submit").addClass("checkout-submit-disabled");
            }
        }
        else {
            if ($("#step-2").hasClass("step-current")) {
                if (abtnCommit.hasClass("checkout-submit")) {
                    abtnCommit.removeClass("checkout-submit").addClass("checkout-submit-disabled");
                }
            }
            else {
                if (abtnCommit.hasClass("checkout-submit-disabled")) {
                    abtnCommit.removeClass("checkout-submit-disabled").addClass("checkout-submit");
                }
            }
        }
    }

    function OnCommit() {
        if ($("#abtnCommit").attr("class") == "checkout-submit-disabled") {
            return false;
        }
        $("[id$=hOp]").val("commit");
        __doPostBack('ctl00$cphMain$lbtnSave', '');
    }
</script>

</asp:Content>
