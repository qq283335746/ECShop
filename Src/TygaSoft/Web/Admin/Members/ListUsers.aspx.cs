using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using TygaSoft.DBUtility;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Admin.Members
{
    public partial class ListUsers : System.Web.UI.Page
    {
        string sqlWhere = string.Empty;
        ParamsHelper parms;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void Bind()
        {
            int totalCount = 0;
            MsSqlMembershipProvider p = (MsSqlMembershipProvider)Membership.Provider;
            rpData.DataSource = p.GetAllUsers(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms != null ? parms.ToArray() : null);

            //rpData.DataSource = Membership.GetAllUsers(pageIndex, AspNetPager1.PageSize, out totalCount);
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            string userName = txtUserName.Value.Trim();
            if (!string.IsNullOrEmpty(userName))
            {
                sqlWhere += "and UserName like @UserName ";
                SqlParameter parm = new SqlParameter("@UserName", SqlDbType.NVarChar, 256);
                parm.Value = "%" + userName + "%";
                parms = new ParamsHelper();
                parms.Add(parm);
            }
        }

        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            GetSearchItem();
            Bind();
        }

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string isLockedOut = DataBinder.Eval(e.Item.DataItem, "IsLockedOut").ToString();

                if (isLockedOut == "1")
                {
                    LinkButton lbtnUnlockUser = (LinkButton)e.Item.FindControl("lbtnUnlockUser");
                    if (lbtnUnlockUser != null)
                    {
                        lbtnUnlockUser.Enabled = true;
                        lbtnUnlockUser.OnClientClick = "return confirm('确定要更改该用户的锁定状态吗？')";
                        lbtnUnlockUser.ToolTip = "点击更改锁定状态";
                    }
                }
            }
        }

        protected void rpData_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "unlock")
            {
                try
                {
                    MsSqlMembershipProvider p = (MsSqlMembershipProvider)Membership.Provider;
                    if (p.UnlockUser(e.CommandArgument.ToString()))
                    {
                        WebHelper.MessageBox.Messager(this.Page, e.Item.Controls[0], "已解除锁定","操作错误","error");

                        //从新绑定数据
                        Bind();
                    }
                    else
                    {
                        WebHelper.MessageBox.Messager(this.Page, e.Item.Controls[0], "操作失败，请检查","操作错误","error");
                    }
                }
                catch (Exception ex)
                {
                    WebHelper.MessageBox.Messager(this.Page, e.Item.Controls[0], ex.Message,"操作错误","error");
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
                case "reload":
                    Bind();
                    break;
                case "del":
                    OnDelete();
                    break;
                case "search":
                    OnSearch();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void OnSearch()
        {
            GetSearchItem();

            Bind();
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        private void OnDelete()
        {
            string userName = hV.Value.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请勾选一行数据进行操作","操作错误","error");
                return;
            }

            string errorMsg = string.Empty;
            try
            {
                if (Membership.DeleteUser(userName))
                {
                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功！");
                    GetSearchItem();
                    Bind();
                }
                else
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败，请检查！","系统提示");
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, errorMsg,"系统异常提示");
            }
        }
    }
}