using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            string userName = tbName.Text.Trim();
            string psw = tbPsw.Text.Trim();
            //string sVc = tbVc.Text.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(psw))
            {
                WebHelper.MessageBox.Messager(this.Page, btnCommit, "用户名或密码输入不能为空！", "操作错误", "error");
                return;
            }

            //if (string.IsNullOrEmpty(sVc))
            //{
            //    WebHelper.MessageBox.Show(this.Page, btnCommit, "验证码输入不能为空！");
            //    return;
            //}

            //if (sVc.ToLower() != Request.Cookies["LoginVc"].Value.ToLower())
            //{
            //    WebHelper.MessageBox.Show(this.Page, btnCommit, "验证码输入不正确，请检查！");
            //    return;
            //}

            string userData = string.Empty;

            try
            {
                MembershipUser userInfo = Membership.GetUser(userName);
                if (!Membership.ValidateUser(userName, psw))
                {
                    if (userInfo == null)
                    {
                        WebHelper.MessageBox.Messager(this.Page, btnCommit, "用户名不存在！","系统提示");
                        return;
                    }
                    if (userInfo.IsLockedOut)
                    {
                        WebHelper.MessageBox.Messager(this.Page, btnCommit, "您的账号已被锁定，请联系管理员先解锁后才能登录！","系统提示");
                        return;
                    }
                    if (!userInfo.IsApproved)
                    {
                        WebHelper.MessageBox.Messager(this.Page, btnCommit, "您的帐户尚未获得批准。您无法登录，直到管理员批准您的帐户！","系统提示");
                        return;
                    }
                    else
                    {
                        WebHelper.MessageBox.Messager(this.Page, btnCommit, "密码不正确，请检查！","系统提示");
                        return;
                    }
                }

                userData = userInfo.ProviderUserKey.ToString()+"|";
            }
            catch (Exception ex)
            {
                WebHelper.MessageBox.Messager(this.Page, btnCommit, ex.Message,"系统提示");
                return;
            }

            //登录成功，则

            bool isPersistent = true;
            //bool isRemember = true;
            bool isAuto = false;
            double d = 180;
            //if (cbRememberMe.Checked) isAuto = true;
            //自动登录 设置时间为1年
            if (isAuto) d = 525600;

            string[] roles = Roles.GetRolesForUser(userName);
            foreach (string role in roles)
            {
                userData += role + ",";
            }

            userData = userData.Trim(',');

            //创建票证
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(d),
                isPersistent, userData, FormsAuthentication.FormsCookiePath);
            //加密票证
            string encTicket = FormsAuthentication.Encrypt(ticket);
            //创建cookie
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            //FormsAuthentication.RedirectFromLoginPage(userName, isPersistent);//使用此行会清空ticket中的userData ？！！！
            Response.Redirect(FormsAuthentication.GetRedirectUrl(userName, isPersistent));
        }
    }
}