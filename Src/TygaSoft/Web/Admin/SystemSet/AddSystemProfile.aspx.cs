using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.Admin.SystemSet
{
    public partial class AddSystemProfile : System.Web.UI.Page
    {
        BLL.SystemProfile bll;
        string nId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            {
                nId = HttpUtility.UrlDecode(Request.QueryString["nId"]);
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(nId))
            {
                if (bll == null) bll = new BLL.SystemProfile();
                Model.SystemProfileInfo model = bll.GetModel(nId);
                if (model != null)
                {
                    txtTitle.Value = model.Title;
                    hEditor1.Value = model.ContentText;
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
            hBackToN.Value = (Int32.Parse(hBackToN.Value) + 1).ToString();
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
            #region 获取输入并验证

            string sTitle = txtTitle.Value.Trim();
            string sContentText = hEditor1.Value;

            #endregion

            if (bll == null) bll = new BLL.SystemProfile();
            Model.SystemProfileInfo model = new Model.SystemProfileInfo();
            model.Title = sTitle;
            model.ContentText = sContentText;
            model.LastUpdatedDate = DateTime.Now;

            int result = -1;
            if (!string.IsNullOrEmpty(nId))
            {
                model.NumberID = nId;
                result = bll.Update(model);
            }
            else
            {
                result = bll.Insert(model);
            }

            if (result == 110)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "已存在相同记录！");
                return;
            }

            if (result > 0)
            {
                WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "提交成功！");
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "提交失败,系统异常！","系统提示");
            }
        }
    }
}