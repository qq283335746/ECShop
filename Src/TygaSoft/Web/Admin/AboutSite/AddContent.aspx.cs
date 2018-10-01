using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.Admin.AboutSite
{
    public partial class AddContent : System.Web.UI.Page
    {
        string nId;
        BLL.ContentDetail bll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            {
                nId = Request.QueryString["nId"];
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
                if (bll == null) bll = new BLL.ContentDetail();
                Model.ContentDetailInfo model = bll.GetModel(nId);
                if (model != null)
                {
                    txtTitle.Value = model.Title;
                    txtParent.Value = model.ContentTypeID.ToString();
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

        private void OnSave()
        {
            string sTitle = txtTitle.Value.Trim();
            if (string.IsNullOrEmpty(sTitle))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "标题不能为空，请检查", "温馨提醒", "error");
                return;
            }
            string sParent = txtParent.Value.Trim();
            Guid contentTypeId = Guid.Empty;
            if (!string.IsNullOrEmpty(sParent))
            {
                Guid.TryParse(sParent, out contentTypeId);
            }
            string sContent = HttpUtility.UrlDecode(hEditor1.Value).Trim();

            if(bll == null) bll = new BLL.ContentDetail();
            Model.ContentDetailInfo model = new Model.ContentDetailInfo();
            model.Title = sTitle;
            model.ContentTypeID = contentTypeId;
            model.ContentText = sContent;
            model.Sort = 0;
            model.LastUpdatedDate = DateTime.Now;

            int effectCount = -1;
            if (!string.IsNullOrEmpty(nId))
            {
                effectCount = bll.Update(model);
            }
            else
            {
                effectCount = bll.Insert(model);
            }

            if (effectCount == 110)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "已存在相同记录", "温馨提醒", "error");
                return;
            }
            if (effectCount > 0)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作成功");
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "操作失败");
            }
        }
    }
}