using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

        private void OnSave()
        {
            string userName = txtUserName.Value.Trim();
            string password = txtPsw.Value.Trim();
            string email = txtEmail.Value.Trim();
            string sVc = txtVc.Value.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "用户名、密码、邮箱为必填项", "操作错误", "error");
                return;
            }

            Regex r = new Regex(@"(([0-9]+)|([a-zA-Z]+)){6,30}");
            if (!r.IsMatch(password))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "密码正确格式由数字或字母组成的字符串，且最小6位，最大30位", "操作错误", "error");
                return;
            }
            r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!r.IsMatch(email))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请输入正确的电子邮箱格式", "操作错误", "error");
                return;
            }

            if (string.IsNullOrEmpty(sVc))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "验证码输入不能为空！", "操作错误", "error");
                return;
            }

            if (sVc.ToLower() != Request.Cookies["RegisterVc"].Value.ToLower())
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "验证码输入不正确，请检查！", "操作错误", "error");
                return;
            }

            string errorMsg = string.Empty;
            try
            {
                MembershipUser user = Membership.CreateUser(userName, password, email);

                if (user != null)
                {
                    Roles.AddUserToRole(user.UserName, "Users");
                    WebHelper.MessageBox.Show(this.Page, lbtnPostBack, string.Format("{0}即将跳转到登录页，请先登录",EnumMembershipCreateStatus.GetStatusMessage(MembershipCreateStatus.Success)), "Login.aspx");
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
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, errorMsg,"系统提示");
                return;
            }
        }
    }
}