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
    public partial class AddUser : System.Web.UI.Page
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
            string userName = txtUsername.Value.Trim();
            string psw = txtPswset.Value.Trim();
            string email = txtEmail.Value.Trim();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(psw) || string.IsNullOrEmpty(email))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "用户名、密码、邮箱为必填项","操作错误","error");
                return;
            }
            if (string.Compare(Request.Cookies["AddUserVc"].Value.ToLower(), txtVc.Value.Trim().ToLower(), true) != 0)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "输入验证码不正确！", "操作错误", "error");
                return;
            }

            string errorMsg = string.Empty;
            try
            {
                MembershipUser user = Membership.CreateUser(userName, psw, email);

                if (user != null)
                {
                    Roles.AddUserToRole(user.UserName, "Users");
                    WebHelper.MessageBox.Messager(this.Page, lbtnSave, EnumMembershipCreateStatus.GetStatusMessage(MembershipCreateStatus.Success),"系统提示");
                }
            }
            catch (MembershipCreateUserException ex)
            {
                errorMsg = EnumMembershipCreateStatus.GetStatusMessage(ex.StatusCode);
            }
            catch (HttpException ex)
            {
                errorMsg = ex.Message;
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, errorMsg, "系统提示");
                return;
            }
        }
    }
}