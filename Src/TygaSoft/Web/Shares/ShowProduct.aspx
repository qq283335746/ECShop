<%@ Page Title="产品详情" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowProduct.aspx.cs" Inherits="TygaSoft.Web.Shares.ShowProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<asp:Repeater runat="server" ID="rpData" OnItemDataBound="rpData_ItemDataBound" OnItemCommand="rpData_ItemCommand">
    <ItemTemplate>
        <div style="background:#FFFFFF;">
            <div id="product-intro">
            <div id="productName" class="pt10">
	            <h1><%#Eval("ProductName")%></h1>
	            <strong><%#Eval("Subtitle")%></strong>
            </div>
            <div class="clearfix">
            <ul id="summary">
                <li id="summary-market">
                <div class="dt">商品编号：</div>
                <div class="dd"><span><%#Eval("PNum")%></span></div>
                </li>
                <li id="summary-price">
		            <div class="dt">本&nbsp;站&nbsp;价：</div>
		            <div class="dd">
			            <strong class="p-price" id="jd-price"><%#string.Format("￥{0}", Eval("ProductPrice"))%></strong>
		            </div>
	            </li>
                <li id="summary-grade">
	                <div class="dt">商品评分：</div>
	                <div class="dd">
		                <span class="star sa5"></span>
	                </div>
                </li>
                <li id="summary-sto">
	                <div class="dt">库存：</div>
	                <div class="dd">
		                <span><%#(int)Eval("StockNum") > 0 ? "有货":"无货"%></span>
	                </div>
                </li>
                <li id="summary-pay">
                    <div class="dt">支付方式：</div>
	                <div class="dd">
		                <span><%#Eval("PayOptions")%></span>
	                </div>
                </li>
                <li class="btnBox">
                    <div class="fl" style="width:154px; padding:4px 0 4px 10px;">
                      <asp:LinkButton runat="server" ID="lbtnBuy" CommandName="toBuy" CommandArgument='<%#Eval("ProductId") %>' CssClass="btnBuy"></asp:LinkButton>
                    </div>
                    <div class="fl" style="width:154px; padding:4px 10px 4px 0;">
                      <asp:LinkButton runat="server" ID="lbtnCart" CommandName="addCart" CommandArgument='<%#Eval("ProductId") %>' CssClass="btnCart"></asp:LinkButton>
                    </div>
                    <div class="clr"></div>
                </li>
                    
            </ul>
            </div>
            <div id="preview" class="mt10 ml10">
            <div id="spec-n1" class="jqzoom">
	            <img src="" width="350" height="350" alt="" id="currentShowImg" title="" />
            </div>

            <div id="spec-list">
            <a onclick="onPrevNext(0);return false;" class="spec-control disabled" id="spec-forward"></a>
            <a onclick="onPrevNext(1);return false;" class="spec-control disabled" id="spec-backward"></a>
            <div class="spec-items" style="position: absolute; width: 310px; height: 54px; overflow: hidden;">
                <ul id="productImgs" class="lh" style="position: absolute; left: 0px; top: 0px; width: 682px;">
                    <%#GetProductImages(Eval("SImagesUrl").ToString(), Eval("MImagesUrl").ToString(), Eval("LImagesUrl").ToString())%>
                </ul>
            </div>
            </div>
            </div>
            </div>
            <div class="clr"></div>
            <div id="product-details">
            <ul class="num">
            <li class="hover"><a href="javascript:void(0)">产品档案</a></li>
            <li><a href="javascript:void(0)">关于本站</a></li>
            <li><a href="javascript:void(0)">产品服务</a></li>
            <li><a href="javascript:void(0)">常见问题</a></li>
            </ul>
            <div class="details_item" style="display:block;">
            <div class="dangan">
                <div class="content">
                    <%#GetCustomAttrs(Eval("CustomAttrs").ToString())%>
                   <div class="clr"></div>
                </div>
            </div>
            <div class="title_box"><div class="zhongwen">产品描述</div><div class="yingwen">Product Description</div></div>
            <div class="text_box">
                <%#HttpUtility.UrlDecode(Eval("Descr").ToString())%>
            </div>
            </div>
            <div class="details_item" style="display:none;">
                <div class="title_box" style="margin-top:0px;">
                    <div class="zhongwen">关于本站</div><div class="yingwen">About Us</div>
                </div>
                <asp:Literal runat="server" ID="ltrAboutUs"></asp:Literal>
            </div>
            <div class="details_item" style="display:none;">
                <asp:Literal runat="server" ID="ltrProductService"></asp:Literal>
            </div>
            <div class="details_item" style="display:none;">
                <div class="title_box" style="margin-top:0px;">
                    <div class="zhongwen">常见问题</div><div class="yingwen">Frequently Asked Questions</div>
                </div>
                <asp:Literal runat="server" ID="ltrQuestions"></asp:Literal>
            </div>

            <div class="clr"></div>
            </div>
            </div>
    </ItemTemplate>
</asp:Repeater>

<script type="text/javascript">
    $(function () {
        $(".jqzoom").jqueryzoom({
            xzoom: 400,
            yzoom: 400,
            offset: 10,
            position: "right",
            preload: 1,
            lens: 1
        });

        $("#product-details>ul>li").click(function () {
            $(this).addClass("hover").siblings().removeClass("hover");
            var i = $(this).index();
            var di = $(this).parent().siblings(".details_item");
            di.eq(i).show().siblings(".details_item").hide();
        })

    })
    </script>

</asp:Content>
