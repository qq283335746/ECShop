using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Transactions;

namespace TygaSoft.Web.Admin.Members
{
    public partial class AddUserRole : System.Web.UI.Page
    {
        string userName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["uName"]))
            {
                userName = HttpUtility.UrlDecode(Request.QueryString["uName"]);
            }

            if (!Page.IsPostBack)
            {
                lbTitle.InnerText = userName;
                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(userName))
            {
                string[] allRoles = null;
                string[] rolesForUser = null;

                string errorMsg = "";
                try
                {
                    allRoles = Roles.GetAllRoles();
                    rolesForUser = Roles.GetRolesForUser(userName);

                    if (rolesForUser.Length > 0)
                    {
                        ViewState["RolesForUser"] = rolesForUser;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnSave, errorMsg, "系统提示");
                    return;
                }

                if (allRoles != null)
                {
                    ListItem li = null;
                    foreach (string item in allRoles)
                    {
                        li = new ListItem(item, item);
                        foreach (string s in rolesForUser)
                        {
                            if (s == item)
                            {
                                li.Selected = true;
                            }
                        }

                        cbList.Items.Add(li);
                    }
                }
                else
                {
                    lbMsg.Text = "暂无角色数据";
                }
            }
        }

        /// <summary>
        /// 对比新选中角色集合与已有角色集合，去掉交集，新的则新增到数据库，旧的则从数据库中删除
        /// </summary>
        /// <param name="newRolesList"></param>
        /// <param name="oldRolesList"></param>
        private void CompareInRole(ref List<string> newRolesList, ref List<string> oldRolesList)
        {
            if (ViewState["RolesForUser"] != null)
            {
                string[] oldRoles = (string[])ViewState["RolesForUser"];
                foreach (string s in oldRoles)
                {
                    string current = newRolesList.Find(delegate(string ss) { return ss == s; });
                    if(!string.IsNullOrEmpty(current))
                    {
                        newRolesList.Remove(current);
                    }
                    else
                    {
                        oldRolesList.Add(s);
                    }
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
        /// 保存
        /// </summary>
        private void OnSave()
        {
            hBackToN.Value = (Int32.Parse(hBackToN.Value) + 1).ToString();

            if (string.IsNullOrEmpty(userName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "当前没有任何用户，请检查！","操作错误","error");
                return;
            }

            List<string> newRolesList = new List<string>();

            foreach (ListItem li in cbList.Items)
            {
                if (li.Selected) newRolesList.Add(li.Value);
            }

            if (newRolesList.Count == 0)
            {
                if (ViewState["RolesForUser"] == null)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnSave, "没有选中任何一项，请检查！", "操作错误", "error");
                    return;
                }
            }

            List<string> oldRolesList = new List<string>();
            CompareInRole(ref newRolesList, ref oldRolesList);

            if (newRolesList.Count == 0 && oldRolesList.Count == 0) return;

            string errorMsg = string.Empty;
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = IsolationLevel.ReadUncommitted;
                options.Timeout = TimeSpan.FromSeconds(90);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    if (newRolesList.Count > 0)
                    {
                        Roles.AddUserToRoles(userName, newRolesList.ToArray());
                    }
                    if (oldRolesList.Count > 0)
                    {
                        Roles.RemoveUserFromRoles(userName, oldRolesList.ToArray());
                    }

                    scope.Complete();

                    WebHelper.MessageBox.MessagerShow(this.Page, lbtnSave, "操作成功！");
                }
            }
            catch (Exception ex)
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