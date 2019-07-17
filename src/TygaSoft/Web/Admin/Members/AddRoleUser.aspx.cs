using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Transactions;
using TygaSoft.DBUtility;


namespace TygaSoft.Web.Admin.Members
{
    public partial class AddRoleUser : System.Web.UI.Page
    {
        string roleName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["rName"]))
            {
                roleName = HttpUtility.UrlDecode(Request.QueryString["rName"]);
            }

            if (!Page.IsPostBack)
            {
                lbTitle.InnerText = roleName;
                
                //绑定数据
                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                string errorMsg = string.Empty;
                try
                {
                    string[] usersInRole = Roles.GetUsersInRole(roleName);
                    ViewState["UsersInRole"] = usersInRole;
                    ListItem li = null;
                    foreach (string item in usersInRole)
                    {
                        li = new ListItem(item, item);
                        li.Selected = true;
                        cbList.Items.Add(li);
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

        /// <summary>
        /// 对比新选中角色集合与已有角色集合，去掉交集，新的则新增到数据库，旧的则从数据库中删除
        /// </summary>
        /// <param name="newRolesList"></param>
        /// <param name="oldRolesList"></param>
        private void CompareInRole(ref List<string> newUsersList, ref List<string> oldUsersList)
        {
            if (ViewState["UsersInRole"] != null)
            {
                string[] oldUsers = (string[])ViewState["UsersInRole"];
                foreach (string s in oldUsers)
                {
                    oldUsersList.Add(s);
                }
                if (oldUsers.Length > 0)
                {
                    string[] copyNewUsersList = new string[newUsersList.Count];
                    newUsersList.CopyTo(copyNewUsersList);
                    foreach (string oldItem in oldUsers)
                    {
                        foreach (string newItem in copyNewUsersList)
                        {
                            if (oldItem == newItem)
                            {
                                newUsersList.Remove(newItem);
                                oldUsersList.Remove(newItem);
                            }
                        }
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

            if (string.IsNullOrEmpty(roleName))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnSave, "当前没有任何角色，请检查！","操作错误","error");
                return;
            }

            List<string> newUsersList = new List<string>();

            foreach (ListItem li in cbList.Items)
            {
                if (li.Selected) newUsersList.Add(li.Value);
            }

            if (newUsersList.Count == 0)
            {
                if (ViewState["UsersInRole"] == null)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnSave, "没有选中任何一项，请检查！", "操作错误", "error");
                    return;
                }
            }

            List<string> oldUsersList = new List<string>();
            CompareInRole(ref newUsersList, ref oldUsersList);

            if (newUsersList.Count == 0 && oldUsersList.Count == 0) return;

            string errorMsg = string.Empty;
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = IsolationLevel.ReadUncommitted;
                options.Timeout = TimeSpan.FromSeconds(90);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    if (newUsersList.Count > 0)
                    {
                        Roles.AddUsersToRole(newUsersList.ToArray(), roleName);
                    }
                    if (oldUsersList.Count > 0)
                    {
                        Roles.RemoveUsersFromRole(oldUsersList.ToArray(), roleName);
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