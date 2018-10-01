using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web.Users.Order
{
    public partial class PayOrder : System.Web.UI.Page
    {
        string orderNum;
        string userId;
        BLL.Order bll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["oN"]))
            {
                orderNum = HttpUtility.UrlDecode(Request.QueryString["oN"]);
            }
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        ///创建验证的票据
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        string[] datas = userData.Split(',');
                        if (datas.Length >= 0) userId = datas[0];
                    }
                }
            }
            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForProduct(Page, ltrTheme);

                Bind();
            }
        }

        private void Bind()
        {
            if (string.IsNullOrEmpty(orderNum))
            {
                WebHelper.MessageBox.Messager(this.Page, Page.Controls[0],"非法进入，已终止执行","操作错误","error");
                return;
            }
            if (bll == null) bll = new BLL.Order();
            Model.OrderInfo model = bll.GetModel(orderNum, userId);
            if (model == null)
            {
                WebHelper.MessageBox.Messager(this.Page, Page.Controls[0], "非法操作，已终止执行", "操作错误", "error");
                return;
            }

            string htmlAppend = "<s class=\"icon-succ02\"></s><h3 class=\"ftx-02\">付款成功，我们将尽快处理，请耐心等待！</h3>";
            htmlAppend += "<ul class=\"list-h\"><li class=\"fore1\">订单号：" + model.OrderNum + "</li>";
            htmlAppend += "<li class=\"fore2\">已付金额：<strong class=\"ftx-01\">" + model.TotalPrice + "元</strong></li>";
            htmlAppend += "</ul></div>";
            ltrSucceed.Text = htmlAppend;
        }
    }
}