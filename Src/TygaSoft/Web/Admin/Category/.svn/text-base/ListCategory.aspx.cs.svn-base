using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;

namespace TygaSoft.Web.Admin.Category
{
    public partial class ListCategory : System.Web.UI.Page
    {
        string sqlWhere;
        ParamsHelper parms;
        BLL.Category bll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //InitCategoryList(ddlCategory);

                //数据绑定
                Bind();
            }
        }

        private void Bind()
        {
            GetSearchItem();

            int totalCount = 0;
            if (bll == null) bll = new BLL.Category();
           
            rpData.DataSource = bll.GetList(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms == null ? null : parms.ToArray()); ;
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            string sCategoryName = txtCategoryName.Value.Trim();
            string parentId = cbtCategory.Value.Trim();

            if (!string.IsNullOrEmpty(sCategoryName))
            {
                sqlWhere += " and CategoryName like @CategoryName";
                SqlParameter parm = new SqlParameter("@CategoryName", SqlDbType.VarChar, 50);
                parm.Value = "%" + sCategoryName + "%";
                parms.Add(parm);

                if (parentId == Guid.Empty.ToString()) parentId = string.Empty;
            }

            if (!string.IsNullOrEmpty(parentId))
            {
                sqlWhere += " and ParentID = @parentId";
                parms.Add(new SqlParameter("@parentId", parentId));
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            GetSearchItem();
            Bind();
        }

        /// <summary>
        /// 绑定父级分类
        /// </summary>
        /// <param name="listControl"></param>
        private void InitCategoryList(ListControl listControl)
        {
            if (listControl.Items.Count > 0) listControl.Items.Clear();

            listControl.Items.Add(new ListItem("请选择", "0"));

            if (bll == null) bll = new BLL.Category();
            Dictionary<string, string> dic = bll.GetKeyValue(" and ParentID='0' ");

            foreach (KeyValuePair<string, string> kvp in dic)
            {
                string name = kvp.Value;
                string value = kvp.Key;
                listControl.Items.Add(new ListItem(name, value));
                CreateSubNode(listControl, value, name);
            }
        }

        /// <summary>
        /// 绑定子级分类
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="parentValue"></param>
        /// <param name="parentName"></param>
        private void CreateSubNode(ListControl listControl, string parentValue, string parentName)
        {
            if (bll == null) bll = new BLL.Category();
            Dictionary<string, string> dic = bll.GetKeyValueByParentId(parentValue);

            foreach (KeyValuePair<string, string> kvp in dic)
            {
                string name = parentName + "-/-" + kvp.Value;
                string value = kvp.Key;
                listControl.Items.Add(new ListItem(name, value));
                CreateSubNode(listControl, value, name);
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
                case "search":
                    OnSearch();
                    break;
                case "del":
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

        /// <summary>
        /// 批量删除数据
        /// </summary>
        private void OnDelete()
        {
            string itemsAppend = hV.Value.Trim();
            if (string.IsNullOrEmpty(itemsAppend))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请至少勾选一行进行操作","操作错误","error");
                return;
            }
            
            if (bll == null) bll = new BLL.Category();
            string[] itemsAppendArr = itemsAppend.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();
            foreach (string item in itemsAppendArr)
            {
                string[] itemArr = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> dic = bll.GetKeyValueByParentId(itemArr[0]);
                if (dic != null && dic.Count > 0)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "不能删除，原因：分类名称为“" + itemArr[1] + "”存在子级，请先删除所有子级");
                    return;
                }

                list.Add(itemArr[0]);
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