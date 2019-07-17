using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web.Admin.Members
{
    public partial class AddRole : System.Web.UI.Page
    {
        //string nId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            //{
            //    nId = Request.QueryString["nId"];
            //}
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            //if (!string.IsNullOrEmpty(nId))
            //{
            //    lbName.Text = "修改角色";
            //    //if(bll == null) bll = new BLL.Roles();
            //    //Entity.Roles entity = bll.GetEntity(Id);
            //    //if (entity != null)
            //    //{
            //    //    tbRoleName.Text = entity.Name;
            //    //    tbEname.Text = entity.Ename;
            //    //}
            //}
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            
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
            hBackToN.Value = (Int32.Parse(hBackToN.Value) + 1).ToString();

            string sRoleName = txtRolename.Value.Trim();
            if (string.IsNullOrEmpty(sRoleName))
            {
                WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "角色名输入不能为空，请检查！");
                return;
            }

            string errorMsg = string.Empty;
            try
            {
                Roles.CreateRole(sRoleName);

                WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "操作成功！");
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, errorMsg, "系统异常提醒");
            }
        }
    }
}