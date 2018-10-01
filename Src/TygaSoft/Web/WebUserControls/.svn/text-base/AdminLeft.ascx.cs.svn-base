using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TygaSoft.Web.WebUserControls
{
    public partial class AdminLeft : System.Web.UI.UserControl
    {
        XElement root;
        string menuAppend;
        string r;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                r = Request.AppRelativeCurrentExecutionFilePath;
                BindMenuNav();
            }
        }

        /// <summary>
        ///绑定菜单导航
        /// </summary>
        private void BindMenuNav()
        {
            if (root == null) root = XElement.Load(Server.MapPath("~/Web.sitemap"));
            var q = root.Elements().Elements()
                .Where(r => !string.IsNullOrEmpty((string)r.Attribute("roles")))
                .Select(nd => new
                {
                    title = nd.Attribute("title").Value,
                    url = nd.Attribute("url").Value,
                    description = nd.Attribute("description").Value,
                    roles = nd.Attribute("roles").Value
                });
            foreach (var item in q)
            {
                if (item.description.IndexOf("hide") == -1)
                {
                    if (isCurrentUserInrole(item.roles))
                    {
                        menuAppend += string.Format("<div title=\"{0}\" style=\"overflow:auto;padding:10px;\">", item.title);

                        //创建当前节点下的子节点
                        CreateSubMi(item.title);

                        menuAppend += "</div>";
                    }
                }
            }

            ltrMenu.Text = menuAppend;
        }

        /// <summary>
        /// 当前用户是否有访问菜单导航的权限，对应角色访问对应的菜单
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool isCurrentUserInrole(string role)
        {
            if (string.IsNullOrEmpty(role)) return false;

            string[] roles = role.Split(',');
            foreach (string item in roles)
            {
                if (HttpContext.Current.User.IsInRole(item))
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateSubMi(string parentTitle)
        {
            if (root == null) root = XElement.Load(Server.MapPath("~/Web.sitemap"));
            var q = root.Descendants()
            .Where(r => (string)r.Parent.Attribute("title") == parentTitle)
            .Select(nd => new
            {
                title = nd.Attribute("title").Value,
                url = nd.Attribute("url").Value,
                description = nd.Attribute("description").Value,
                roles = nd.Attribute("roles").Value
            });

            foreach (var item in q)
            {
                if (item.description.IndexOf("hide") == -1)
                {
                    if (isCurrentUserInrole(item.roles))
                    {
                        if (item.url == r)
                        {
                            menuAppend += string.Format("<a href='{0}' class='hover'>{1}</a>", item.url.Replace("~/", "/"), item.title);
                        }
                        else
                        {
                            menuAppend += string.Format("<a href='{0}' class>{1}</a>", item.url.Replace("~/", "/"), item.title);
                        }
                        
                    }
                }
            }
        }
    }
}