<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShareTop.ascx.cs" Inherits="TygaSoft.Web.WebUserControls.ShareTop" %>

<div class="vt">
<div class="w">
    <ul>
        <asp:LoginView ID="lvUser" runat="server">
        <AnonymousTemplate> 
          <li class="fl" style="color:#CCCCCC;"> 您好，欢迎光临某某某珠宝！</li>
          <li class="fl ml10"><span style="color:#CCCCCC;">|</span> 
              <a href="/Login.aspx" class="ml10">登录</a>
          </li>
          <li class="fl ml10"><span style="color:#CCCCCC;">|</span>
              <a href="/Register.aspx" class="ml10">注册</a>
          </li>
        </AnonymousTemplate>
        <LoggedInTemplate>
        <li class="fl ml10">
            <asp:LoginName ID="lnUser" runat="server" FormatString="您好，{0}" ForeColor="#CCCCCC" />
            <span style="color:#CCCCCC;">|</span>
            <asp:LoginStatus ID="lsUser" runat="server" LogoutText="[退出]" CssClass="ml10" />
        </li>
            
        </LoggedInTemplate>
    </asp:LoginView>     
    </ul>
    <ul class="fr">
        <li class="fl"><a href="/Users/Default.aspx">会员中心</a></li>
        <li class="fl ml10"><span style="color:#CCCCCC;">|</span> <a href="javascript:void(0)" class="ml10">全国服务热线：400-0755-1234</a></li>
    </ul>
</div>
</div>
<div class="bg_header_main">
<div class="w">
    <div class="logo_wrapper">
    <div class="logo">
        <a href="javascript:void(0)" title="天涯孤岸珠宝网">
        <img src="/Images/logo.png" alt="天涯孤岸珠宝网" />
        </a>
    </div>
    <div class="goodness">
        <img src="/Images/head_7.png" width="142px" height="46px" alt="" />
        <img src="/Images/head_8.png" width="142px" height="46px" alt="" />
    </div>
    </div>
    <div class="clr"></div>
    <div id="nav">
    <div id="category" style="display:none;">
        <div class="mt ld">
        <h2><a href="javascript:void(0)">全部商品分类<b></b></a></h2>
        </div>
        <div class="mc">
            <asp:Literal runat="server" ID="ltrCategory"></asp:Literal>

        </div>
    </div>
    <asp:Menu runat="server" ID="shareMenu" Orientation="Horizontal" 
        StaticEnableDefaultPopOutImage="False" DisappearAfter="-1" StaticDisplayLevels="1" 
        MaximumDynamicDisplayLevels="1" DynamicHorizontalOffset="0" ClientIDMode="Static">
    </asp:Menu>
    </div>
    <div class="search_cart">
      <div class="fl">
         
          <input type="text" id="txtKeyword" class="placeholder" autocomplete="off" onkeydown="javascript:if(event.keyCode==13) onSearch('txtKeyword');" />
          <a href="javascript:void(0)" id="abtnSearch" class="abtn" onclick="onSearch('txtKeyword')">搜 索</a>
          <p id="hotKeyword">
            <span>热门：</span>
            </p>
      </div>
      <div class="cart">
        <p>购物车中有<span runat="server" id="lbCartCount">0</span>件商品</p>
        <a rel="nofollow" href="javascript:void(0)" id="abtnToPay">去结算</a>
        </div>
    </div>
</div>
</div>

<script type="text/javascript" src="/Jquery/js/jquery-ui-1.10.3.custom.min.js"></script>
<script type="text/javascript">
    $(function () {
        var $_SitePaths = $("#SitePaths");
        var $_SitePathsLastSpan = $_SitePaths.find("span:last");
        $_SitePaths.find("a:first").remove();
        $("#shareMenu").prev("a").remove();
        var aList = $("#shareMenu>ul>li>a");
        aList.each(function () {
            var $_this = $(this);
            if ($_SitePaths.text().indexOf($_this.text()) > -1) {
                $_this.addClass("current").parent().siblings("li").children("a").removeClass("current");
            }
        })
        $("#shareMenu ul:gt(5)").css("width", "200px");

        $("#shareMenu>ul>li>a").hover(function () {
        }, function () {
            $(this).removeClass("highlighted");
        })

        $("#abtnToPay").click(function () {
            if (parseInt($("[id$=lbCartCount]").text()) > 0) {
                window.location = "/Shares/ShoppingCart/ListCart.aspx";
            }
        })

        $("#txtKeyword").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/ScriptServices/KeywordService.asmx/GetKeywords",
                    type: "post",
                    contentType: "application/json; charset=utf-8",
                    data: "{prefixText:'" + request.term + "',count:0}",
                    dataType: "json",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (event, ui) {
                $(this).val(ui.item.value);
            },
            minLength: 1,
            autoFocus: true,
            delay: 300
        })

        //热门
        var objHotKeyword = $("#hotKeyword");
        $.ajax({
            url: "/ScriptServices/KeywordService.asmx/GetHotKeywords",
            type: "post",
            contentType: "application/json; charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (data) {
                $.map(data.d, function (item) {
                    objHotKeyword.append("<a href='javascript:void(0)' onclick='onHotKeyword(this)'>" + item + "</a>");
                })
            }
        });
    })

    function onHotKeyword(h) {
        $("#txtKeyword").val($(h).text());
        onSearch("txtKeyword");
        return false;
    }

    function onSearch(h) {
        var o = document.getElementById(h);
        var g = o.value;
        g = g.replace(/^\s*(.*?)\s*$/, "$1");
        if (g.length > 0) {
            setTimeout(function () {
                window.location.href = "/Shares/SearchProduct.aspx?kw=" + encodeURIComponent(g) + "";
            })
        }
    }
</script>