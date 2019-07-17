using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web.Users.Order
{
    public partial class AddOrderSucceed : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(orderNum))
            {
                if (bll == null) bll = new BLL.Order();
                Model.OrderInfo model = bll.GetModel(orderNum, userId);
                if (model == null)
                {
                    ltrBank.Text = "";
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "非法操作，已终止执行","操作错误","error");
                    return;
                }

                ViewState["OrderId"] = model.OrderId;
                ViewState["TotalPrice"] = model.TotalPrice;

                string htmlAppend = "<s class=\"icon-succ02\"></s><h3 class=\"ftx-02\">订单提交成功，请您尽快付款！</h3>";
                htmlAppend += "<ul class=\"list-h\"><li class=\"fore1\">订单号："+model.OrderNum+"</li>";
                htmlAppend += "<li class=\"fore2\">应付金额：<strong class=\"ftx-01\">"+model.TotalPrice+"元</strong></li>";
                htmlAppend += "</ul><p id=\"p_show_info\">&nbsp;</p><p class=\"reminder\"><strong>立即支付<span class=\"ftx-01\">"+model.TotalPrice+"元</span>，即可完成订单。</strong>";
                htmlAppend += "请您在<span class=\"ftx-04\">24小时</span>内完成支付，否则订单会被自动取消。</p></div>";
                ltrSucceed.Text = htmlAppend;
            }
        }

        /// <summary>
        /// 按钮OnCommand事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Command(object sender, CommandEventArgs e)
        {
            string commName = hOp.Value.Trim();
            switch (commName)
            {
                case "OnPay":
                    OnPay();
                    break;
                default:
                    break;
            }
        }

        private void OnPay()
        {
            if (ViewState["OrderId"] == null || ViewState["TotalPrice"] == null)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "当前订单不存在，请检查","系统提示");
                return;
            }
            object orderId = ViewState["OrderId"];
            decimal totalPrice = 0;
            if(!decimal.TryParse(ViewState["TotalPrice"].ToString(),out totalPrice))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "付款金额转换失败，请检查","系统提示");
                return;
            }

            if (bll == null) bll = new BLL.Order();
            if (bll.PayPrice(Guid.Parse(userId), orderId, totalPrice) > 0)
            {
                Response.Redirect(string.Format("PayOrder.aspx?oN={0}", HttpUtility.UrlEncode(orderNum)), true);
            }
            else 
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "付款失败，请检查","系统提示");
            }
        }
    }
}