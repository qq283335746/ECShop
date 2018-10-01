<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TygaSoft.Web.Login" %>

<%@ Register src="WebUserControls/Top.ascx" tagname="Top" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录</title>
    <link href="Styles/Default.css" rel="stylesheet" type="text/css" />
    <link href="Styles/PageMain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Jquery/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //动态使登录框垂直居中
            var h = $(window).height();
            h = h - 31;
            $("#pagemain").css("height", "" + h + "px");

            //设置输入框初始化焦点
            $("#tbName").focus();
        })
    </script>
   
</head>
<body class="loginBody">
    <form id="form1" runat="server">
    <!--header begin-->
    <div id="header">
        <uc1:Top ID="Top1" runat="server" />
    </div>
    <!--header end-->
    <!--pagemain begin-->
    <div id="login">
    <div id="pagemain">
         <div class="w" style="width: 1000px;">
           <div id="loginBoxTop"></div>
           <div id="loginBoxRow">
             <div id="loginBox">
               <div id="loginInfoTop">
                 <div id="righthide"></div>
               </div>
               <span class="clr"></span>
               
               <div id="loginInfo">
                 <ul>
                   <li class="fl">
                       <ul>
                         <li style="height:31px; line-height:31px;">
                             <span class="fl lbUserName"></span>
                             <div class="fl"><asp:TextBox ID="tbName" runat="server" MaxLength="50" CssClass="txt"></asp:TextBox></div>
                             <span class="clr"></span>
                         </li>
                         <li style="height:31px; line-height:31px;">
                             <span class="fl lbPsw"></span>
                             <div class="fl"><asp:TextBox ID="tbPsw" runat="server" TextMode="Password" MaxLength="50" CssClass="txt"></asp:TextBox></div>
                             <span class="clr"></span>
                         </li>
                       </ul>
                   </li>
                   <li class="fl ml10">
                     <asp:Button ID="btnCommit" runat="server" CssClass="btnLogin" onclick="btnCommit_Click" />
                   </li>
                  </ul>
                 <span class="clr"></span>
                
               </div>

               <div id="loginMsg" style="margin-left:133px;">
                  <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="用户名为必填项" CssClass="cr" ControlToValidate="tbName" Display="Dynamic"></asp:RequiredFieldValidator><br />
                  <asp:RequiredFieldValidator ID="rfvPsw" runat="server" ErrorMessage="密码为必填项" CssClass="cr" ControlToValidate="tbPsw" Display="Dynamic"></asp:RequiredFieldValidator>
               </div>
             </div>
           </div>
           <div id="loginBoxBottom"></div>
         </div>
      </div>
    </div>
    <!--pagemain end-->
    <!--footer begin-->
    <div id="footer"></div>
    <!--footer eng-->
    </form>
</body>
</html>
