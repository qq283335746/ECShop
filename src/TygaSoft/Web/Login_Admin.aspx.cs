using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TygaSoft.Web
{
    public partial class Login_Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForJeasyui(Page, ltrTheme);
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

        private void OnSave()
        {
            string userName = txtUsername.Value.Trim();
            string psw = txtPsw.Value.Trim();
            string sVc = txtVc.Value.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(psw))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "用户名或密码输入不能为空！","操作错误","error");
                return;
            }

            if (string.IsNullOrEmpty(sVc))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "验证码输入不能为空！", "操作错误", "error");
                return;
            }

            if (sVc.ToLower() != Request.Cookies["LoginVc"].Value.ToLower())
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "验证码输入不正确，请检查！", "操作错误", "error");
                return;
            }

            string userId = string.Empty;
            try
            {
                if (!Membership.ValidateUser(userName, psw))
                {
                    MembershipUser userInfo = Membership.GetUser(userName);
                    if (userInfo == null)
                    {
                        WebHelper.MessageBox.Messager(this.Page, lbtnSave, "用户名不存在！","系统提示");
                        return;
                    }
                    if (userInfo.IsLockedOut)
                    {
                        WebHelper.MessageBox.Messager(this.Page, lbtnSave, "您的账号已被锁定，请联系管理员先解锁后才能登录！","系统提示");
                        return;
                    }
                    if (!userInfo.IsApproved)
                    {
                        WebHelper.MessageBox.Messager(this.Page, lbtnSave, "您的帐户尚未获得批准。您无法登录，直到管理员批准您的帐户！","系统提示");
                        return;
                    }
                    else
                    {
                        WebHelper.MessageBox.Messager(this.Page, lbtnSave, "密码不正确，请检查！","系统提示");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, ex.Message,"系统提示");
                return;
            }

            //登录成功，则

            bool isPersistent = true;
            string userData = string.Empty;
            bool isRemember = true;
            double d = 180;
            //if (cbRememberMe.Checked) isRemember = true;
            //记住我 设置时间为1个月
            if (isRemember) d = 43800;

            userData = userId;

            //创建票证
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(d),
                isPersistent, userData, FormsAuthentication.FormsCookiePath);
            //加密票证
            string encTicket = FormsAuthentication.Encrypt(ticket);
            //创建cookie
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            //FormsAuthentication.RedirectFromLoginPage(userName, isPersistent);//使用此行会清空ticket中的userData ？！！！
            Response.Redirect("~/Admin/Default.aspx",true);
        }
    }
}