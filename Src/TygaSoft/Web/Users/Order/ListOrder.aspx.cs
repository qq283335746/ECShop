using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Users.Order
{
    public partial class ListOrder : System.Web.UI.Page
    {
        string userId;
        BLL.Order bll;
        string sqlWhere;
        ParamsHelper parms;

        protected void Page_Load(object sender, EventArgs e)
        {
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
            //查询条件
            GetSearchItem();

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

            sqlWhere += "and UserId = @UserId";
            parms.Add(new SqlParameter("@UserId",userId));

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
            Bind();
        }

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string payStatus = (string)DataBinder.Eval(e.Item.DataItem, "PayStatus");
                switch (payStatus)
                {
                    case "未支付":
                        HtmlAnchor aPay = (HtmlAnchor)e.Item.FindControl("aPay");
                        if (aPay != null) aPay.Visible = true;
                        break;
                    default:
                        break;
                }
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
                        Bind();
                    }
                }
            }
        }
    }
}