using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class ListSearchKeyword : System.Web.UI.Page
    {
        BLL.SearchKeyword bll;
        string sqlWhere;
        ParamsHelper parms;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //数据绑定
                Bind();
            }
        }

        private void Bind()
        {
            if (bll == null) bll = new BLL.SearchKeyword();
            int totalCount = 0;

            rpData.DataSource = bll.GetList(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            if (parms == null) parms = new ParamsHelper();
            string sSearchName = txtName.Value.Trim();
            if (!string.IsNullOrEmpty(sSearchName))
            {
                sqlWhere += " and SearchName like @SearchName";
                SqlParameter parm = new SqlParameter("@SearchName", SqlDbType.NVarChar, 256);
                parm.Value = "%" + sSearchName + "%";
                parms.Add(parm);
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //查询条件
            GetSearchItem();
            Bind();
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
                case "OnSearch":
                    OnSearch();
                    break;
                case "OnDel":
                    OnDelete();
                    break;
                default:
                    break;
            }
        }

        private void OnSearch()
        {
            //查询条件
            GetSearchItem();
            Bind();
        }

        private void OnDelete()
        {
            string sAppend = hV.Value.Trim();
            if (!string.IsNullOrEmpty(sAppend))
            {
                string[] orderIds = sAppend.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (orderIds.Length > 0)
                {
                    if (bll == null) bll = new BLL.SearchKeyword();
                    if (bll.DeleteBatch(orderIds.ToList<string>()))
                    {
                        GetSearchItem();
                        Bind();
                    }
                }
            }
        }
    }
}