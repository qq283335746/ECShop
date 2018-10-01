<%@ Page Title="天涯孤岸软件-OA办公软件" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListJadeite.aspx.cs" Inherits="TygaSoft.Web.Shares.ListJadeite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="sl">
    <div class="a">
        <h2 class="aa"><span class="aaa">分类筛选</span></h2>
        <div class="ar">
            <a href="javascript:void(0)" class="ab"><i class="aba"></i><span class="abb">所有分类</span></a>

            <asp:Literal runat="server" ID="ltrCategory"></asp:Literal>

        </div>
    </div>
</div>

<div class="sr">
    <div class="c">
    <div class="ca">
        <div class="cad"><a href="#" class="caa">所有分类</a>&nbsp;&gt;&nbsp;翡翠</div>
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
    <div id="goods" class="e" style="margin-top:17px;">
        <asp:Repeater runat="server" ID="rpData">
            <ItemTemplate>
                <div class="f">
                    <div class="fborder">
                        <div class="fa" style='<%#string.Format("background-image:url({0})", VirtualPathUtility.MakeRelative("~/Shares/ListJadeite.aspx", Eval("ImagesUrl").ToString()))%>'></div>
                    </div>
                    <div class="fborder-b"></div>
                    <div class="fb"></div>
                    <div class="fc">
                    <div class="fj"><a href="#" class="fja j-fav"><span class="fjb"></span><span class="fjc">加入收藏</span></a></div>
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

</asp:Content>
