using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Shares
{
    public partial class SearchProduct : System.Web.UI.Page
    {
        string orderBy;
        string categoryId;
        string keyword;
        string cIdAppend;
        decimal startPrice = -1;
        decimal endPrice = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["c"]))
            {
                categoryId = Request.QueryString["c"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["kw"]))
            {
                keyword = HttpUtility.UrlDecode(Request.QueryString["kw"]);
            }

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    hKw.Value = keyword;
                }

                WebHelper.PageHelper.LoadHeaderForProduct(Page, ltrTheme);

                //绑定产品
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
            BindCategory();

            if (!string.IsNullOrEmpty(categoryId))
            {
                if (!string.IsNullOrEmpty(cIdAppend)) categoryId = cIdAppend;
                List<Model.ProductInfo> list = WebHelper.ProductDataProxy.GetProductsByCategory(categoryId);
                AspNetPager1.RecordCount = list.Count();
                if (list != null)
                {
                    if (startPrice > -1)
                    {
                        list = list.FindAll(delegate(Model.ProductInfo m) { return m.ProductPrice >= startPrice; });
                    }
                    if (endPrice > -1)
                    {
                        list = list.FindAll(delegate(Model.ProductInfo m) { return m.ProductPrice <= endPrice; });
                    }
                    if (orderBy == "ProductPrice asc")
                    {
                        list = list.OrderBy(m => m.ProductPrice).ToList();
                    }
                    else if (orderBy == "ProductPrice desc")
                    {
                        list = list.OrderByDescending(m => m.ProductPrice).ToList();
                    }

                    rpData.DataSource = list.Skip(AspNetPager1.CurrentPageIndex-1).Take(AspNetPager1.PageSize);
                    rpData.DataBind();
                }
                else
                {
                    list = new List<Model.ProductInfo>();
                    rpData.DataSource = list;
                    rpData.DataBind();
                }
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                List<Model.ProductInfo> list = WebHelper.ProductDataProxy.GetProductsBySearch(keyword);
                int count = list.Count;
                AspNetPager1.RecordCount = count;
                if (list != null && count > 0)
                {
                    if (startPrice > -1)
                    {
                        list = list.FindAll(delegate(Model.ProductInfo m) { return m.ProductPrice >= startPrice; });
                    }
                    if (endPrice > -1)
                    {
                        list = list.FindAll(delegate(Model.ProductInfo m) { return m.ProductPrice <= endPrice; });
                    }
                    if (orderBy == "ProductPrice asc")
                    {
                        list.OrderBy(m => m.ProductPrice);
                    }
                    else if (orderBy == "ProductPrice desc")
                    {
                        list.OrderByDescending(m => m.ProductPrice);
                    }

                    rpData.DataSource = list.Skip(AspNetPager1.CurrentPageIndex-1).Take(AspNetPager1.PageSize);
                    rpData.DataBind();
                }
                else
                {
                    list = new List<Model.ProductInfo>();
                    rpData.DataSource = list;
                    rpData.DataBind();
                }

                //开启一个线程处理，将查询关键字写入到数据存储
                BLL.SearchKeyword skBll = new BLL.SearchKeyword(keyword, count);
                skBll.ThreadStart();
            }
        }

        ///// <summary>
        ///// 获取列表查询条件项,并构建查询参数集
        ///// </summary>
        //private void GetSearchItem()
        //{
        //    if (parms == null) parms = new ParamsHelper();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        string productName = keyword;
        //        hKw.Value = productName;
        //        sqlWhere += "and ProductName like @ProductName ";
        //        SqlParameter parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 256);
        //        parm.Value = "%" + productName + "%";
        //        parms.Add(parm);
        //    }
        //    string sSPrice = txtSPrice.Value.Trim();
        //    if (!string.IsNullOrEmpty(sSPrice))
        //    {
        //        decimal minPrice = 0;
        //        if (decimal.TryParse(sSPrice, out minPrice))
        //        {
        //            sqlWhere += "and ProductPrice >= @MinPrice ";
        //            SqlParameter parm = new SqlParameter("@MinPrice", SqlDbType.Decimal);
        //            parm.Value = minPrice;
        //            parms.Add(parm);
        //        }
        //    }
        //    string sEPrice = txtEPrice.Value.Trim();
        //    if (!string.IsNullOrEmpty(sEPrice))
        //    {
        //        decimal maxPrice = 0;
        //        if (decimal.TryParse(sEPrice, out maxPrice))
        //        {
        //            sqlWhere += "and ProductPrice <= @MaxPrice ";
        //            SqlParameter parm = new SqlParameter("@MaxPrice", SqlDbType.Decimal);
        //            parm.Value = maxPrice;
        //            parms.Add(parm);
        //        }
        //    }
        //}

        /// <summary>
        /// 绑定分类
        /// </summary>
        private void BindCategory()
        {
            string categoryAppend = string.Empty;
            List<Model.CategoryInfo> list = WebHelper.CategoryDataProxy.GetList();
            if (list != null && list.Count > 0)
            {
                //获取当前分类
                Model.CategoryInfo currentItem = null;
                if (!string.IsNullOrEmpty(categoryId))
                {
                    currentItem = list.Find(delegate(Model.CategoryInfo model) { return model.NumberID == categoryId; });
                }
                if ((currentItem != null))
                {
                    string navMap = currentItem.CategoryName;
                    List<Model.CategoryInfo> parentList = new List<Model.CategoryInfo>();
                    List<Model.CategoryInfo> lastChildList = new List<Model.CategoryInfo>();
                    List<Model.CategoryInfo> childList = new List<Model.CategoryInfo>();
                    GetParent(list, currentItem, ref parentList);
                    GetLastChildAll(list, currentItem, ref lastChildList);
                    GetChild(list, currentItem, ref childList);
                    int parentCount = 0;
                    if (parentList != null && parentList.Count > 0)
                    {
                        parentCount = parentList.Count;
                        int n = 0;
                        foreach (Model.CategoryInfo model in parentList)
                        {
                            if (model != null)
                            {
                                n++;
                                navMap = string.Format("<a href='/Shares/SearchProduct.aspx?c={1}'>{0}</a>", model.CategoryName, model.NumberID) + "&nbsp;&gt;&nbsp;" + navMap;
                                if (parentCount == 2)
                                {
                                    if (n == 2)
                                    {
                                        categoryAppend = string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ab\"><i class=\"aba\"></i><span class=\"abb\">{0}</span></a>", model.CategoryName, model.NumberID) + categoryAppend;
                                    }
                                    else
                                    {
                                        categoryAppend = string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ad ad-1\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></a>", model.CategoryName, model.NumberID);
                                    }
                                }
                                else
                                {
                                    categoryAppend = string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ab\"><i class=\"aba\"></i><span class=\"abb\">{0}</span></a>", model.CategoryName, model.NumberID);
                                }
                            }
                        }
                    }
                    if (parentCount == 0)
                    {
                        categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ab\"><i class=\"aba\"></i><span class=\"abb\">{0}</span></a>", currentItem.CategoryName, currentItem.NumberID);
                    }
                    else if (parentCount == 1)
                    {
                        categoryAppend += string.Format("<div class=\"ad ad-current ad-1\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></div>", currentItem.CategoryName);
                    }
                    else
                    {
                        categoryAppend += string.Format("<div class=\"ad ad-current ad-2\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></div>", currentItem.CategoryName);
                    }
                    cad.InnerHtml = navMap;

                    if (childList != null && childList.Count > 0)
                    {
                        foreach (Model.CategoryInfo model in childList)
                        {
                            if (parentCount == 0)
                            {
                                categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ad ad-1\"><i class=\"ada\"></i><span class=\"adb\">{0}<span class=\"adc\"></span></span></a>", model.CategoryName, model.NumberID);
                            }
                            else if (parentCount == 1)
                            {
                                categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ad ad-2\"><i class=\"ada\"></i><span class=\"adb\">{0}<span class=\"adc\">(19.5万)</span></span></a>", model.CategoryName, model.NumberID);
                            }
                        }
                    }

                    if (lastChildList != null && lastChildList.Count() > 0)
                    {
                        foreach (Model.CategoryInfo model in lastChildList)
                        {
                            if (model != null)
                            {
                                cIdAppend += string.Format("'{0}',", model.NumberID);
                            }
                        }
                        cIdAppend = cIdAppend.Trim(',');
                    }
                }
                else
                {
                    Model.CategoryInfo rootItem = list.Find(delegate(Model.CategoryInfo model) { return model.CategoryName == "所有分类"; });
                    if (rootItem != null)
                    {
                        cad.InnerHtml = string.Format("<a href='/Shares/SearchProduct.aspx?c={1}'>{0}</a>", rootItem.CategoryName, rootItem.NumberID);
                        categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={0}\" class=\"ab\"><i class=\"aba\"></i><span class=\"abb\">所有分类</span></a>", rootItem.NumberID);
                        List<Model.CategoryInfo> list1 = list.FindAll(delegate(Model.CategoryInfo model) { return model.ParentID == rootItem.NumberID; });
                        if (list1 != null && list1.Count > 0)
                        {
                            foreach (Model.CategoryInfo model1 in list1)
                            {
                                categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ad ad-1\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></a>", model1.CategoryName, model1.NumberID);
                                List<Model.CategoryInfo> list2 = list.FindAll(delegate(Model.CategoryInfo model2) { return model2.ParentID == model1.NumberID; });
                                if (list2 != null && list2.Count > 0)
                                {
                                    foreach (Model.CategoryInfo model2 in list2)
                                    {
                                        categoryAppend += string.Format("<a href=\"/Shares/SearchProduct.aspx?c={1}\" class=\"ad ad-2\"><i class=\"ada\"></i><span class=\"adb\">{0}</span></a>", model2.CategoryName, model2.NumberID);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ltrCategory.Text = categoryAppend;
        }

        /// <summary>
        /// 检索当前分类节点的所有父级节点，并返回检索到的项集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentModel"></param>
        /// <param name="newList"></param>
        private void GetParent(List<Model.CategoryInfo> list, Model.CategoryInfo currentModel, ref List<Model.CategoryInfo> newList)
        {
            if (list == null || currentModel == null) return;
            if (currentModel.ParentID != "0")
            {
                Model.CategoryInfo parent = list.Find(delegate(Model.CategoryInfo model) { return model.NumberID == currentModel.ParentID; });
                if (parent != null)
                {
                    newList.Add(parent);
                    GetParent(list, parent, ref newList);
                }
            }
        }

        /// <summary>
        /// 检索当前分类节点的所有子级节点，并返回检索到的项集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentModel"></param>
        /// <param name="newList"></param>
        private void GetChild(List<Model.CategoryInfo> list, Model.CategoryInfo currentModel, ref List<Model.CategoryInfo> newList)
        {
            if (list == null || currentModel == null) return;
            newList = list.FindAll(delegate(Model.CategoryInfo model) { return model.ParentID == currentModel.NumberID; });
        }

        /// <summary>
        /// 检索当前分类节点的所有子级节点，并返回检索到的项集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="currentModel"></param>
        /// <param name="newList"></param>
        private void GetLastChildAll(List<Model.CategoryInfo> list, Model.CategoryInfo currentModel, ref List<Model.CategoryInfo> newList)
        {
            if (list == null || currentModel == null) return;
            List<Model.CategoryInfo> childList = list.FindAll(delegate(Model.CategoryInfo model) { return model.ParentID == currentModel.NumberID; });
            if (childList != null && childList.Count > 0)
            {
                foreach (Model.CategoryInfo model in childList)
                {
                    GetLastChildAll(list, model, ref newList);
                }
            }
            else
            {
                newList.Add(currentModel);
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
                case "orderByPrice":
                    OnOrderByPrice();
                    break;
                case "orderByAll":
                    Bind();
                    break;
                case "searchPrice":
                    OnSearch();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据价格排序
        /// </summary>
        private void OnOrderByPrice()
        {
            string orderByName = hV.Value.Trim();
            if (!string.IsNullOrEmpty(orderByName))
            {
                switch (orderByName)
                {
                    case "asc":
                        orderBy = "ProductPrice asc";
                        Bind();
                        break;
                    case "desc":
                        orderBy = "ProductPrice desc";
                        Bind();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 筛选
        /// </summary>
        private void OnSearch()
        {
            string sStartPrice = txtSPrice.Value.Trim();
            string sEndPrice = txtEPrice.Value.Trim();

            if (!string.IsNullOrEmpty(sStartPrice))
            {
                if (!decimal.TryParse(sStartPrice, out startPrice))
                {
                    startPrice = -1;
                }
            }
            if (!string.IsNullOrEmpty(sEndPrice))
            {
                if (!decimal.TryParse(sEndPrice, out endPrice))
                {
                    endPrice = -1;
                }
            }

            Bind();
        }
    }
}