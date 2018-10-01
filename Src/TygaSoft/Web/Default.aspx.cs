using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForShare(Page, ltrTheme);

                //绑定分类
                BindCategory();

                Bind();
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// 绑定产品列表
        /// </summary>
        private void Bind()
        {
            int totalCount = 0;
            rpData.DataSource = WebHelper.ProductDataProxy.GetProducts(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount);
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
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
                        foreach (Model.CategoryInfo model1 in list1)
                        {
                            categoryAppend += string.Format("<a href=\""+sRelativePath+"?c={1}\" class=\"ad ad-1\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></a>", model1.CategoryName,model1.NumberID);
                            List<Model.CategoryInfo> list2 = list.FindAll(delegate(Model.CategoryInfo model2) { return model2.ParentID == model1.NumberID; });
                            if (list2 != null && list2.Count > 0)
                            {
                                foreach (Model.CategoryInfo model2 in list2)
                                {
                                    categoryAppend += string.Format("<a href=\""+sRelativePath+"?c={1}\" class=\"ad ad-2\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></a>", model2.CategoryName, model2.NumberID);
                                }
                            }
                        }
                    }
                }
            }

            ltrCategory.Text = categoryAppend;
        }
    }
}