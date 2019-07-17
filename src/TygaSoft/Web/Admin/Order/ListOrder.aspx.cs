using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.Order
{
    public partial class ListOrder : System.Web.UI.Page
    {
        BLL.Order bll;
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
            if (bll == null) bll = new BLL.Order();
            int totalCount = 0;

            rpData.DataSource = bll.GetDsForOrderInfo(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms == null ? null : parms.ToArray());
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            if (parms == null) parms = new ParamsHelper();
            string sOrderNum = txtOrderNum.Value.Trim();
            if (!string.IsNullOrEmpty(sOrderNum))
            {
                sqlWhere += " and OrderNum like @OrderNum";
                SqlParameter parm = new SqlParameter("@OrderNum", SqlDbType.VarChar, 30);
                parm.Value = "%" + sOrderNum + "%";
                parms.Add(parm);
            }
            if (cbbStatus.SelectedIndex > 0)
            {
                short status = 0;
                if (short.TryParse(cbbStatus.Value, out status))
                {
                    sqlWhere += " and Status = @Status";
                    SqlParameter parm = new SqlParameter("@Status", SqlDbType.TinyInt);
                    parm.Value = status;
                    parms.Add(parm);
                }
            }
            if (cbbPayStatus.SelectedIndex > 0)
            {
                short payStatus = 0;
                if (short.TryParse(cbbPayStatus.Value, out payStatus))
                {
                    sqlWhere += " and PayStatus = @PayStatus";
                    SqlParameter parm = new SqlParameter("@PayStatus", SqlDbType.TinyInt);
                    parm.Value = payStatus;
                    parms.Add(parm);
                }
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
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
                    if (bll == null) bll = new BLL.Order();
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