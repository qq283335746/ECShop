using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Admin.Members
{
    public partial class UpdatePsw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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
            string userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "操作失败，没有找到当前登录用户，请检查！", "操作错误", "error");
                return;
            }
            string pswpast = txtPswpast.Value.Trim();
            string pswset = txtPswset.Value.Trim();

            try
            {
                MsSqlMembershipProvider p = (MsSqlMembershipProvider)Membership.Provider;
                if (p.ChangePassword(userName, pswpast, pswset))
                {
                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "修改密码成功！");
                }
                else
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnSave, "修改密码失败，请检查！","系统提示");
                }
            }
            catch (Exception ex)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, ex.Message,"系统提示");
            }
        }
    }
}