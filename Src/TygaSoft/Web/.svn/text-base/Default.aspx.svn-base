<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TygaSoft.Web.Default" %>
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
        <div class="cad"><a href="javascript:void(0)" class="caa">所有分类</a></div>
    </div>
    <div class="cc" style="display:none;">
        <span class="cca"></span>
        <div id="ccb" class="ccb" style="height:56px;"></div>
        <a id="ccd" class="ccd" href="javascript:void(0)" style="display:none;"></a>
    </div>
    <div class="clr"></div>
    </div>
    <div id="goods" class="e mt10">
        <asp:Repeater runat="server" ID="rpData">
            <ItemTemplate>
                <div class="f">
                    <div class="fborder">
                        <div class="fa" style='<%#string.Format("background-image:url({0})", VirtualPathUtility.MakeRelative("~/Default.aspx", Eval("ImagesUrl").ToString()))%>'></div>
                    </div>
                    <div class="fborder-b"></div>
                    <div class="fb"></div>
                    <div class="fc">
                    <div class="fj"><a href="javascript:void(0)" onclick="addFavorite()" class="fja j-fav"><span class="fjb"></span><span class="fjc">加入收藏</span></a></div>
                    <a target="_blank" href='Shares/ShowProduct.aspx?pId=<%#Eval("ProductId") %>' class="fd" title='<%#Eval("ProductName") %>'></a>
                    <div class="fe">
                        <a target="_blank" href="Shares/ShowProduct.aspx?pId=<%#Eval("ProductId") %>" class="fea" title='<%#Eval("ProductName") %>'>
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

<script type="text/javascript" src="Scripts/JsHelper.js"></script>
<script type="text/javascript">
    $(function () {
        $(".f").hover(function () {
            $(this).addClass("f-hover");
        }, function () {
            $(this).removeClass("f-hover");
        })
    })
    
</script>

</asp:Content>
