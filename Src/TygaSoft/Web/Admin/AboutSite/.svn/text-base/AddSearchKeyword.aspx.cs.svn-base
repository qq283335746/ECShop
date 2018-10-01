using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class AddSearchKeyword : System.Web.UI.Page
    {
        BLL.SearchKeyword bll;
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
                if (bll == null) bll = new BLL.SearchKeyword();
                Model.SearchKeywordInfo model = bll.GetModel(nId);
                if (model != null)
                {
                    txtSearchName.Value = model.SearchName;
                    txtTotalCount.Value = model.TotalCount.ToString();
                    txtDataCount.Value = model.DataCount.ToString();
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

            string sSearchName = txtSearchName.Value.Trim();
            string sTotalCount = txtTotalCount.Value.Trim();
            string sDataCount = txtDataCount.Value.Trim();

            if (string.IsNullOrEmpty(sSearchName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "关键字名称为必填项，请检查","操作错误","error");
                return;
            }

            int totalCount = 0;
            if (!string.IsNullOrEmpty(sTotalCount))
            {
                if (!int.TryParse(sTotalCount, out totalCount))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "累计次数正确格式为整数，请检查", "操作错误", "error");
                    return;
                }
            }
            int dataCount = 0;
            if (!string.IsNullOrEmpty(sDataCount))
            {
                if (!int.TryParse(sDataCount, out dataCount))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "数据个数正确格式为整数，请检查", "操作错误", "error");
                    return;
                }
            }

            #endregion

            if (bll == null) bll = new BLL.SearchKeyword();
            if (!string.IsNullOrEmpty(nId))
            {
                Model.SearchKeywordInfo model = new Model.SearchKeywordInfo();
                model.NumberID = nId;
                model.SearchName = sSearchName;
                model.TotalCount = totalCount;
                model.LastUpdatedDate = DateTime.Now;
                model.DataCount = dataCount;
                if (bll.Update(model) > 0)
                {
                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功！");
                    return;
                }
                else
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败，请检查","系统提示");
                    return;
                }
            }
            else
            {
                if (bll.CreateKeyword(sSearchName, dataCount) > -1)
                {
                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功！");
                    return;
                }
                else
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败，请检查", "系统提示");
                    return;
                }
            }
        }
    }
}