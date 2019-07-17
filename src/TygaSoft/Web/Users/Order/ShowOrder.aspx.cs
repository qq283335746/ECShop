using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web.Users.Order
{
    public partial class ShowOrder : System.Web.UI.Page
    {
        string userId;
        string orderNum;
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
                ////动态加载css和script
                //WebHelper.PageHelper.LoadHeaderForUsers(Page, ltrTheme);

                //数据绑定
                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(orderNum))
            {
                IList<Model.OrderInfo> list = new List<Model.OrderInfo>();
                if (bll == null) bll = new BLL.Order();
                Model.OrderInfo model = bll.GetModel(orderNum, Guid.Parse(userId));
                if (model != null)
                {
                    list.Add(model);
                }

                rpData.DataSource = list;
                rpData.DataBind();
                GetProductList(model.Products);
            }
        }

        private void GetProductList(string products)
        {
            if (!string.IsNullOrEmpty(products))
            {
                BLL.Cart cart = new BLL.Cart();
                string productInfoAppend = "";
                string[] items = products.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] subItems = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (string.IsNullOrEmpty(subItems[0]))
                    {
                        continue;
                    }
                    cart.Add(subItems[0]);
                    //nIdAppend += string.Format("'{0}',", subItems[0]);
                    //Model.CartItemInfo model = new Model.CartItemInfo();
                    //model.Quantity = int.Parse(subItems[1]);
                    //model.Subtotal = decimal.Parse(subItems[2]);
                    ////productInfoAppend += string.Format("<td>{1}</td><td>{0}</td>", subItems[1], subItems[2]);
                }

                //if (!string.IsNullOrEmpty(nIdAppend))
                //{
                //    nIdAppend = nIdAppend.Trim(',');
                //    if (bll == null) bll = new BLL.OrderInfo();
                //    List<Model.Product> list = bll.GetProductInIds(nIdAppend);
                //    if (list != null)
                //    {
                //        foreach (Model.Product model in list)
                //        {
                //            productInfoAppend += string.Format("<tr><td>{1}</td><td>{0}</td>{2}</tr>",model.ProductName,model.PNum,productInfoAppend);
                //        }
                //    }
                //}

                foreach (Model.CartItemInfo model in cart.CartItems)
                {
                    productInfoAppend += string.Format("<tr><td><a href=\"../../Shares/ShowProduct.aspx?nId={3}\" target=\"_blank\">{0}</a> </td><td>{1}</td><td>{2}</td></tr>", model.ProductName, model.Subtotal, model.Quantity,model.ProductId);
                }

                if (rpData.Items.Count > 0)
                {
                    Literal ltrProducts = rpData.Items[0].FindControl("ltrProducts") as Literal;
                    if (ltrProducts != null)
                    {
                        ltrProducts.Text = productInfoAppend;
                    }
                }
            }
        }
    }
}