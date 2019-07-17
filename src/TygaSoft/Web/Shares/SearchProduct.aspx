<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchProduct.aspx.cs" Inherits="TygaSoft.Web.Shares.SearchProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="sl">
    <div class="a">
        <h2 class="aa"><span class="aaa">分类筛选</span></h2>
        <div class="ar">

            <asp:Literal runat="server" ID="ltrCategory"></asp:Literal>

        </div>
    </div>
</div>

<div class="sr">
    <div class="c">
    <div class="ca">
        <div runat="server" id="cad" class="cad"><a href="javascript:void(0)" class="caa">所有分类</a>&nbsp;&gt;&nbsp;珠宝/钻石/翡翠/黄金</div>
    </div>
    <div class="cc" style="display:none;">
        <span class="cca">品牌：</span>
        <div id="ccb" class="ccb" style="height:56px;">
            <a href="#">钻石小鸟</a>
            <a href="#">佐卡伊</a>
            <a href="#">金大生</a>
            <a href="#">梦克拉</a>
            <a href="#">六福典雅</a>
            <a href="#">潮宏基</a>
            <a href="#">一搏千金</a>
            <a href="#">蓝色多瑙河</a>
            <a href="#">周大福</a>
            <a href="#">新金</a>
            <a href="#">钻石快线</a>
            <a href="#">梦石黛</a>
            <a href="#">戴欧妮</a>
            <a href="#">锦和珠宝</a>
            <a href="#">爱朵钻</a>
        </div>
        <a id="ccd" class="ccd" href="#"></a>
    </div>
    <div class="clr"></div>
    </div>
    <div id="d" class="d">
        <div class="fl">
            <a href="javascript:void(0)" id="orderByAll" class="da db-select">默认排序</a>
            <a href="javascript:void(0)" id="orderByPrice" class="db"><em class="dba">价格排序</em><i class="dbb"></i></a>
            <span class="dc">价格筛选</span>
            <div class="de" style="z-index:0;">
                <div class="df" id="prices">
                  <input type="text" runat="server" id="txtSPrice" value="" />-<input type="text" runat="server" id="txtEPrice" value="" />
                  <a href="javascript:void(0)" id="abtnSearchPrice" class="deb">确定</a>
                </div>
            </div>
        </div>
    </div>
    <div class="clr"></div>
    <div id="goods" class="e">
        <asp:Repeater runat="server" ID="rpData">
            <ItemTemplate>
                <div class="f">
                    <div class="fborder">
                        <div class="fa" style='<%#string.Format("background-image:url({0})", VirtualPathUtility.MakeRelative("~/Shares/SearchProduct.aspx", Eval("ImagesUrl").ToString()))%>'></div>
                    </div>
                    <div class="fborder-b"></div>
                    <div class="fb"></div>
                    <div class="fc">
                    <div class="fj"><a href="javascript:void(0)" onclick="addFavorite()" class="fja j-fav"><span class="fjb"></span><span class="fjc">加入收藏</span></a></div>
                    <a target="_blank" href='ShowProduct.aspx?pId=<%#Eval("ProductId") %>' class="fd" title='<%#Eval("ProductName") %>'></a>
                    <div class="fe">
                        <a target="_blank" href="ShowProduct.aspx?pId=<%#Eval("ProductId") %>" class="fea" title='<%#Eval("ProductName") %>'>
                            <%#Eval("ProductName") %>
                        </a>
                    </div>
                    <div class="ff"><span class="ffa">￥<%#Eval("ProductPrice")%></span></div>
                    <div class="fg">免邮费</div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:AspNetPager ID="AspNetPager1" runat="server" Width="100%" UrlPaging="false" CssClass="pages mt10" CurrentPageButtonClass="cpb"
            ShowPageIndexBox="Never" PageSize="36" OnPageChanged="AspNetPager1_PageChanged"
            EnableTheming="true" FirstPageText="首页" LastPageText="末页" NextPageText="下一页" PrevPageText="上一页" 
            ShowCustomInfoSection="Never" CustomInfoHTML="当前第%CurrentPageIndex%页/共%PageCount%页，每页显示%PageSize%条">
        </asp:AspNetPager>

    </div>
</div>

<input type="hidden" runat="server" id="hKw" value="" />
<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command"/>
<input type="hidden" runat="server" id="hOp" value="" />
<input type="hidden" runat="server" id="hV" value="" />

<script type="text/javascript" src="../Scripts/JsHelper.js"></script>
<script type="text/javascript">
    $(function () {
        $("#category").show();

        var hKw = $("[id$=hKw]");
        var hOp = $("[id$=hOp]");
        var hV = $("[id$=hV]");
        var $_orderByPrice = $("#orderByPrice");
        var $_orderByAll = $("#orderByAll");
        if ($.trim(hKw.val()).length > 0) {
            $("#txtKeyword").val(hKw.val());
        }

        $(".f").hover(function () {
            $(this).addClass("f-hover");
        }, function () {
            $(this).removeClass("f-hover");
        })

        if (hOp.val() == "orderByPrice") {
            $_orderByPrice.addClass("db-select");
            $_orderByAll.removeClass("db-select");
            if (hV.val() == "asc") {
                $_orderByPrice.find("i").addClass("dbb-up");
            }
            else if (hV.val() == "desc") {
                $_orderByPrice.find("i").addClass("dbb-down");
            }
        }
        else if (hOp.val() == "orderByAll") {
            $_orderByAll.addClass("db-select");
        }
        $_orderByAll.click(function () {
            if ($(this).hasClass("db-select")) {
                return false;
            }
            hOp.val("orderByAll");
            hV.val("");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })
        $_orderByPrice.click(function () {
            $("[id$=hOp]").val("orderByPrice");
            var objI = $(this).find("i");
            if (objI.hasClass("dbb-up")) {
                $("[id$=hV]").val("desc");
            }
            else if (objI.hasClass("dbb-down")) {
                $("[id$=hV]").val("asc");
            }
            else {
                $("[id$=hV]").val("asc");
            }
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })
        $("#abtnSearchPrice").click(function () {
            var sSPrice = $.trim($("[id$=txtSPrice]").val());
            var sEPrice = $.trim($("[id$=txtEPrice]").val());
            if (sSPrice.length == 0 && sEPrice.length == 0) {
                alert("请输入价格区间");
                return;
            }
            var reg = /^(\d*\.)?\d+$/;
            if (!reg.test(sSPrice) || !reg.test(sEPrice)) {
                alert("请正确输入价格区间的数值");
                return;
            }
            if (parseFloat(sSPrice) > parseFloat(sEPrice)) {
                alert("价格区间中的起始值不能大于结束值");
                return;
            }
            $("[id$=hOp]").val("searchPrice");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        })
    })
</script>

</asp:Content>
