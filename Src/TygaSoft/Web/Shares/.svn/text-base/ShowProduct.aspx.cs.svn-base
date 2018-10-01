using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Shares
{
    public partial class ShowProduct : System.Web.UI.Page
    {
        BLL.Product pBll;
        BLL.SystemProfile syspBll;
        List<Model.SystemProfileInfo> list;
        string sqlWhere;
        ParamsHelper parms;
        string productId;
        CustomProfileCommon profile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["pId"]))
            {
                productId = HttpUtility.UrlDecode(Request.QueryString["pId"]);
            }

            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForProduct(Page, ltrTheme);

                Bind();
            }
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            if (pBll == null) pBll = new BLL.Product();
            int totalCount = 0;
            rpData.DataSource = pBll.GetDataSet(1, 1, out totalCount, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            if (!string.IsNullOrEmpty(productId))
            {
                sqlWhere += "and ProductId = @ProductId ";
                SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.VarChar, 40);
                parm.Value = productId;
                parms.Add(parm);
            }
        }

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltrAboutUs = (Literal)e.Item.FindControl("ltrAboutUs");
                Literal ltrProductService = (Literal)e.Item.FindControl("ltrProductService");
                Literal ltrQuestions = (Literal)e.Item.FindControl("ltrQuestions");
                if (ltrAboutUs != null)
                {
                    BindSystemProfile(ltrAboutUs,"关于本站");
                }
                if (ltrProductService != null)
                {
                    BindSystemProfile(ltrProductService, "产品服务");
                }
                if (ltrQuestions != null)
                {
                    BindSystemProfile(ltrQuestions, "常见问题");
                }
            }
        }

        protected void rpData_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            string commName = e.CommandName;
            switch (commName)
            {
                case "addCart":
                    AddCart((string)e.CommandArgument);
                    break;
                case "toBuy":
                    ToBuy((string)e.CommandArgument);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="productId"></param>
        private void AddCart(string productId)
        {
            if (profile == null) profile = new CustomProfileCommon();
            profile.ShoppingCart.Add(productId);
            profile.Save();
            Response.Redirect("ShoppingCart/AddCartSucceed.aspx", true);
        }

        /// <summary>
        /// 直接购买
        /// </summary>
        /// <param name="productId"></param>
        private void ToBuy(string productId)
        {
            if (profile == null) profile = new CustomProfileCommon();
            profile.ShoppingCart.Add(productId);
            profile.Save();
            Response.Redirect("ShoppingCart/ListCart.aspx", true);
        }

        /// <summary>
        /// 分割获取产品小、中、大图
        /// </summary>
        /// <param name="sSImagesUrl"></param>
        /// <param name="sMImagesUrl"></param>
        /// <param name="sLImagesUrl"></param>
        /// <returns></returns>
        protected string GetProductImages(string sSImagesUrl, string sMImagesUrl, string sLImagesUrl)
        {
            if (string.IsNullOrEmpty(sSImagesUrl)) return string.Empty;
            string htmlAppend = string.Empty;
            string[] sSImagesUrlArr = sSImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] sMImagesUrlArr = sMImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] sLImagesUrlArr = sLImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int n = 0;
            foreach (string item in sSImagesUrlArr)
            {
                if (n == 0)
                {
                    htmlAppend += string.Format("<li><img src=\"{0}\" width=\"50\" height=\"50\" alt=\"\" class=\"img-hover\"/><span style=\"display:none;\">{1},{2}</span></li>", item.Replace("~", ""), sMImagesUrlArr[n].Replace("~", ""), sLImagesUrlArr[n].Replace("~", ""));
                }
                else
                {
                    htmlAppend += string.Format("<li><img src=\"{0}\" width=\"50\" height=\"50\" alt=\"\" /><span style=\"display:none;\">{1},{2}</span></li>", item.Replace("~", ""), sMImagesUrlArr[n].Replace("~", ""), sLImagesUrlArr[n].Replace("~", ""));
                }
                
                n++;
            }

            return htmlAppend;
        }

        /// <summary>
        /// 分割获取自定义属性
        /// </summary>
        /// <param name="sCustomAttrs"></param>
        /// <returns></returns>
        protected string GetCustomAttrs(string sCustomAttrs)
        {
            if (string.IsNullOrEmpty(sCustomAttrs)) return string.Empty;
            string htmlAppend = string.Empty;
            string[] items = sCustomAttrs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int itemsLen = items.Length;
            int n = 0;
            foreach (string item in items)
            {
                string[] itemArr = item.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                if (itemArr[0].Contains("标题"))
                {
                    if (n > 0)
                    {
                        htmlAppend += "</ul>";
                    }
                    htmlAppend += string.Format("<div class=\"biaoti\">{0}</div><ul>", itemArr[1]);
                }
                else
                {
                    htmlAppend += string.Format("<li><span class=\"span_1\">{0}：</span><span class=\"span_2\">{1}</span></li>",itemArr[0], itemArr[1]);
                }

                if (n == itemsLen)
                {
                    htmlAppend += "</ul>";
                }

                n++;
            }
            return htmlAppend;
        }

        /// <summary>
        /// 获取系统预设，并绑定到页面显示
        /// </summary>
        /// <param name="productId"></param>
        private void BindSystemProfile(Literal ltr,string title)
        {
            if (!string.IsNullOrEmpty(productId))
            {
                if (list == null)
                {
                    if (syspBll == null) syspBll = new BLL.SystemProfile();
                    list = syspBll.GetModelInTitle("'关于本站','产品服务','常见问题'");
                }
                if (list != null)
                {
                    Model.SystemProfileInfo model = list.Find(delegate(Model.SystemProfileInfo m) { return m.Title == title; });
                    if (model != null)
                    {
                        ltr.Text = HttpUtility.UrlDecode(model.ContentText);
                    }
                }
            }
        }
    }
}