using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using TygaSoft.CustomProviders;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.Members
{
    public partial class ListRoles : System.Web.UI.Page
    {
        string sqlWhere;
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
            //获取列表查询条件项,并构建查询参数集
            GetSearchItem();

            MsSqlRoleProvider r = (MsSqlRoleProvider)Roles.Provider;
            int totalCount = 0;

            rpData.DataSource = r.GetAllRoles(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms != null ? parms.ToArray() : null);
            rpData.DataBind();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //   
            //}
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            string sRoleName = txtRoleName.Value.Trim();

            if (!string.IsNullOrEmpty(sRoleName))
            {
                sqlWhere += "and RoleName like @RoleName ";
                SqlParameter parm = new SqlParameter("@RoleName", SqlDbType.NVarChar, 50);
                parm.Value = "%" + sRoleName + "%";
                parms.Add(parm);
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
            string RoleAppend = hV.Value.Trim();
            if (string.IsNullOrEmpty(RoleAppend))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请至少勾选一行数据再进行操作","操作错误","error");
                return;
            }

            string[] items = RoleAppend.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            IList<string> list = new List<string>();
            foreach(string item in items)
            {
                list.Add(item);
            }
            if(list.Count > 0)
            {
                try
                {
                    MsSqlRoleProvider p = (MsSqlRoleProvider)Roles.Provider;
                    if (p.DeleteBatch(list))
                    {
                        WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功！");
                        GetSearchItem();
                        Bind();
                    }
                    else
                    {
                        WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败，请检查！","系统提醒");
                    }
                }
                catch (Exception ex)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, ex.Message,"系统异常提醒");
                }
            }
        }
    }
}