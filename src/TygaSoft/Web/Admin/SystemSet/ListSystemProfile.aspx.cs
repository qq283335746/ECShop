using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.SystemSet
{
    public partial class ListSystemProfile : System.Web.UI.Page
    {
        string sqlWhere;
        ParamsHelper parms;
        BLL.SystemProfile bll;

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
            int totalCount = 0;
            if (bll == null) bll = new BLL.SystemProfile();

            rpData.DataSource = bll.GetDataSet(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms == null ? null : parms.ToArray()); ;
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            string sTitle = txtTitle.Value.Trim();
            if (!string.IsNullOrEmpty(sTitle))
            {
                sqlWhere += "and Title like @Title ";
                SqlParameter parm = new SqlParameter("@Title", SqlDbType.NVarChar, 256);
                parm.Value = "%" + sTitle + "%";
                parms = new ParamsHelper();
                parms.Add(parm);
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
        /// 查询事件
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
            string itemsAppend = hV.Value.Trim();
            if (string.IsNullOrEmpty(itemsAppend))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请至少勾选一行再进行操作","操作错误","error");
                return;
            }

            if (bll == null) bll = new BLL.SystemProfile();
            string[] itemsAppendArr = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();
            foreach (string item in itemsAppendArr)
            {
                list.Add(item);
            }

            if (list.Count > 0)
            {
                if (bll.DeleteBatch(list))
                {
                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功");
                    GetSearchItem();
                    Bind();
                }
                else
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败，请检查","系统提示");
                }
            }
        }
    }
}