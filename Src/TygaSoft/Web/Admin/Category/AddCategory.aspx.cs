using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.Admin.Category
{
    public partial class AddCategory : System.Web.UI.Page
    {
        string cId;
        BLL.Category cBll;
        string parentName;
        string title;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cId"]))
            {
                cId = Request.QueryString["cId"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["title"]))
            {
                title = HttpUtility.UrlDecode(Request.QueryString["title"]);
            }
            else title = "All";
            if (!string.IsNullOrEmpty(Request.QueryString["parentName"]))
            {
                parentName = HttpUtility.UrlDecode(Request.QueryString["parentName"]);
            }

            if (!Page.IsPostBack)
            {
                //绑定分类
                InitCategoryList(ddlCategory);

                //绑定数据
                Bind();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void Bind()
        {
            //如果当前是编辑进入页面，择执行修改操作
            if (!string.IsNullOrEmpty(cId))
            {
                if (cBll == null) cBll = new BLL.Category();
                Model.CategoryInfo model = cBll.GetModel(cId);
                ListItem li = null;
                if (model != null)
                {
                    txtCategoryname.Value = model.CategoryName;
                    li = ddlCategory.Items.FindByValue(model.ParentID);
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                    txtRemark.Value = model.Remark;
                }
            }
        }

        /// <summary>
        /// 绑定父级分类
        /// </summary>
        /// <param name="listControl"></param>
        private void InitCategoryList(ListControl listControl)
        {
            if (listControl.Items.Count > 0) listControl.Items.Clear();

            listControl.Items.Add(new ListItem("请选择", "0"));

            Dictionary<string, string> dic = null;
            if (cBll == null) cBll = new BLL.Category();

            if (!string.IsNullOrEmpty(parentName))
            {
                dic = cBll.GetKeyValueOnParentName(parentName);
                //只显示当前父级名称及其子节点的选择
                if (dic != null && dic.Count > 0)
                {
                    listControl.Items.Clear();
                    dic.Distinct();
                }
            }
            else
            {
                dic = cBll.GetKeyValue("and ParentID='0' ");
            }

            foreach (KeyValuePair<string, string> kvp in dic)
            {
                ListItem li = null;
                li = ddlCategory.Items.FindByValue(kvp.Key);
                if (li == null)
                {
                    string name = kvp.Value;
                    string value = kvp.Key;
                    listControl.Items.Add(new ListItem(name, value));
                    CreateSubNode(listControl, value, name);
                }
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
            if (cBll == null) cBll = new BLL.Category();
            Dictionary<string, string> dic = cBll.GetKeyValueByParentId(parentValue);

            if (dic != null)
            {
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    string name = parentName + "-/-" + kvp.Value;
                    string value = kvp.Key;
                    listControl.Items.Add(new ListItem(name, value));
                    CreateSubNode(listControl, value, name);
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
            string commName = e.CommandName;
            switch (commName)
            {
                case "lbtnsave":
                    OnSave();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void OnSave()
        {
            hBackToN.Value = (int.Parse(hBackToN.Value) + 1).ToString(); ;

            #region 获取输入并验证

            string sName = txtCategoryname.Value.Trim();
            string sParentID = "0";
            string sRemark = txtCategoryname.Value.Trim();

            if (ddlCategory.SelectedIndex > -1)
            {
                sParentID = ddlCategory.SelectedValue;
            }

            if (string.IsNullOrEmpty(sName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "分类名称不能为空","操作错误","error");
                return;
            }

            #endregion

            if (cBll == null) cBll = new BLL.Category();

            Model.CategoryInfo model = new Model.CategoryInfo();
            model.CategoryName = sName;
            model.ParentID = sParentID;
            model.Remark = sRemark;
            model.Sort = 0;
            model.CreateDate = DateTime.Now;
            model.Title = title;

            int result = -1;
            if (!string.IsNullOrEmpty(cId))
            {
                model.NumberID = cId;
                result = cBll.Update(model);
            }
            else
            {
                result = cBll.Insert(model);
            }

            if (result == 110)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "分类名称已存在，请换一个再重试！");
                return;
            }

            if (result > 0)
            {
                WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "提交成功！");

                //重新加载所属分类
                InitCategoryList(ddlCategory);
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "提交失败,系统异常！","系统提示");
            }
        }
    }
}