using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.WebUserControls
{
    public partial class ShareTop : System.Web.UI.UserControl
    {
        XElement root;
        //BLL.Category cBll;
        CustomProfileCommon profile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindMenuNav();

                //绑定菜单导航
                BindCategory();
            }

            if (profile == null) profile = new CustomProfileCommon();
            lbCartCount.InnerText = profile.ShoppingCart.Count.ToString();
        }

        /// <summary>
        ///绑定菜单导航
        /// </summary>
        private void BindMenuNav()
        {
            if (root == null) root = XElement.Load(Server.MapPath("~/Web.sitemap"));
            var q = root.Descendants()
                .Where(r => (string)r.Attribute("roles") == "Everyone" || string.IsNullOrEmpty((string)r.Attribute("roles")))
                .Select(nd => new
                {
                    title = nd.Attribute("title").Value,
                    url = nd.Attribute("url").Value,
                    description = nd.Attribute("description").Value,
                });
            foreach (var item in q)
            {
                if (item.description.IndexOf("hide") == -1)
                {
                    string url = item.url;
                    MenuItem mi = new MenuItem();
                    mi.Text = item.title;
                    mi.Selectable = !string.IsNullOrEmpty(url);
                    mi.NavigateUrl = url;

                    shareMenu.Items.Add(mi);
                }
            }
        }

        /// <summary>
        /// 绑定分类
        /// </summary>
        private void BindCategory()
        {
            string sRelativePath = VirtualPathUtility.MakeRelative(Request.AppRelativeCurrentExecutionFilePath, "~/Shares/SearchProduct.aspx");
            string categoryAppend = string.Empty;
            List<Model.CategoryInfo> list = WebHelper.CategoryDataProxy.GetList();
            if (list != null && list.Count > 0)
            {
                Model.CategoryInfo rootItem = list.Find(delegate(Model.CategoryInfo model) { return model.CategoryName == "所有分类"; });
                if (rootItem != null)
                {
                    List<Model.CategoryInfo> list1 = list.FindAll(delegate(Model.CategoryInfo model) { return model.ParentID == rootItem.NumberID; });
                    if (list1 != null && list1.Count > 0)
                    {
                        int n = 0;
                        foreach (Model.CategoryInfo model1 in list1)
                        {
                            n++;
                            string shide_bt = string.Empty;
                            if (n == 1)
                            {
                                shide_bt = "hide_bt";
                            }
                            categoryAppend += string.Format("<div class=\"item " + shide_bt + "\"><span><h3><a href=\"{1}\">{0}</a></h3><s></s></span>", model1.CategoryName, sRelativePath + "?c=" + model1.NumberID + "");
                            categoryAppend += "<div class=\"i-mc\" style=\"top:3px;\">";
                            categoryAppend += "<div onclick=\"$(this).parent().parent().removeClass('hover')\" class=\"close\"></div>";
                            categoryAppend += "<div class=\"subitem\">";

                            List<Model.CategoryInfo> list2 = list.FindAll(delegate(Model.CategoryInfo model2) { return model2.ParentID == model1.NumberID; });
                            if (list2 != null && list2.Count > 0)
                            {
                                foreach (Model.CategoryInfo model2 in list2)
                                {
                                    categoryAppend += string.Format("<dl><dt><a href=\"{1}\">{0}</a></dt><dd><em><a href=\"{1}\">{0}</a></em></dd></dl>", model2.CategoryName, sRelativePath + "?c=" + model2.NumberID + "");
                                }
                            }

                            categoryAppend += "</div></div></div>";
                        }
                    }
                }
            }

            ltrCategory.Text = categoryAppend;
        }
    }
}