using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.Web.Admin.BBS
{
    public partial class AddContentType : System.Web.UI.Page
    {
        BbsContentType ctBll;
        string nId;

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
                if (ctBll == null) ctBll = new BbsContentType();
                BbsContentTypeInfo model = ctBll.GetModel(nId);
                if (model != null)
                {
                    txtTypeName.Value = model.TypeName;
                    txtParent.Value = model.ParentID.ToString();
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
            string sTypeName = txtTypeName.Value.Trim();
            string sParent = txtParent.Value.Trim();

            Guid parentId = Guid.Empty;
            if (!string.IsNullOrEmpty(sParent))
            {
                Guid.TryParse(sParent, out parentId);
            }

            BbsContentTypeInfo model = new BbsContentTypeInfo();
            model.TypeName = sTypeName;
            model.ParentID = parentId;
            model.Sort = 0;
            model.SameName = "All";
            model.LastUpdatedDate = DateTime.Now;

            if (ctBll == null) ctBll = new BbsContentType();
            int result = -1;
            if (!string.IsNullOrEmpty(nId))
            {
                model.NumberID = nId;
                result = ctBll.Update(model);
            }
            else
            {
                result = ctBll.Insert(model);
            }
            if (result == 110)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "已存在相同记录！");
                return;
            }

            if (result > 0)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "提交成功！");
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "提交失败,系统异常！", "温馨提醒", "error");
            }
        }
    }
}