<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminTop.ascx.cs" Inherits="TygaSoft.Web.WebUserControls.AdminTop" %>
<div id="vt">
    <div class="w">
        <ul class="fl lh">
            <li class="fore1">
            <asp:HyperLink ID="hlMainPage" runat="server" NavigateUrl="~/Default.aspx" Text="首 页" /></li>
        </ul>
        <ul class="fr lh">
            <li id="loginbar" class="fore1 ld"> 
            <asp:LoginView ID="lvUser" runat="server">
                <AnonymousTemplate> 您好，请先<asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/Login.aspx" CssClass="alkc">[登录]</asp:HyperLink>
                </AnonymousTemplate>
                <LoggedInTemplate><asp:LoginName ID="lnUser" runat="server" FormatString="您好，{0}" /><asp:LoginStatus ID="lsUser" runat="server" LogoutText="[退出]" CssClass="alkc ml10" /></LoggedInTemplate>
            </asp:LoginView> 
             </li>
        </ul>
        <span class="clr"></span>
    </div>
</div>
<div class="w">
  <div id="logobg" style="text-align:center;"> 
      <div id="logo">
        <span style="color:#FFF; font-size:36px;">测试系统</span> 
      </div>
  </div>
  <div id="nav">
      <asp:Menu runat="server" ID="menu1" Orientation="Horizontal" 
          StaticEnableDefaultPopOutImage="False" DisappearAfter="-1" StaticDisplayLevels="1" 
          MaximumDynamicDisplayLevels="1" DynamicHorizontalOffset="0">

    </asp:Menu>

  </div>
  <div id="subNav" class="mb10"></div>
</div>
<script type="text/javascript">
    $(function () {
        var $_SitePaths = $("#SitePaths");
        var $_SitePathsLastSpan = $_SitePaths.find("span:last");
        $_SitePaths.find("a:first").remove();
        $("#nav>a:first").remove();
        var aList = $("#Top1_menu1>ul>li>a");
        aList.each(function () {
            var $_this = $(this);
            if ($_SitePaths.text().indexOf($_this.text()) > -1) {
                $_this.addClass("highlighted");
                $_this.siblings("ul").css("display", "block");
                var ulaList = $_this.siblings("ul").find("a");
                ulaList.each(function () {
                    if ($(this).text() == $_SitePathsLastSpan.text()) {
                        $(this).addClass("highlighted");
                    }
                })
            }
        })
        $("#Top1_menu1 ul:gt(7)").css("width", "200px");

        $("#Top1_menu1>ul>li>a").hover(function () {
            var $_a = $(this).parent().siblings().find("a:first");
            $_a.each(function () {
                if ($(this).hasClass("highlighted")) {
                    $(this).next().css("display","none");
                    $(this).removeClass("highlighted");
                }
            })
        })
    })
</script>