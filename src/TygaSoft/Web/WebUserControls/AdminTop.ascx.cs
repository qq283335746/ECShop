using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.ComponentModel;

namespace TygaSoft.Web.WebUserControls
{
    public partial class AdminTop : System.Web.UI.UserControl
    {
        XElement root;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) Bind();
        }

        private void Bind()
        {
            bool IsAdmin = false;
            if (HttpContext.Current.User.IsInRole("系统管理员") || HttpContext.Current.User.IsInRole("超级管理员"))
            {
                IsAdmin = true;
            }
            //动态加载顶部导航菜单
            if (root == null)
                root = XElement.Load(Server.MapPath("~/App_Data/Menus.xml"));
            var q = from r in root.Elements("menu")
                    orderby r.Attribute("order").Value
                    select new { key = r.Attribute("key").Value.Trim(), name = r.Attribute("name").Value.Trim() };

            foreach (var item in q)
            {
                if (!IsAdmin)
                {
                    if (item.key != "admin")
                    {
                        MenuItem mi = new MenuItem();
                        mi.Selectable = false;
                        mi.Text = item.name;
                        var subq = from r in root.Descendants("nav")
                                   where r.Parent.Attribute("name").Value == item.name && r.Attribute("key").Value != "admin"
                                   orderby r.Attribute("id").Value
                                   select new
                                   {
                                       key = r.Attribute("key").Value,
                                       name = r.Attribute("name").Value,
                                       value = r.Attribute("value").Value,
                                       title = r.Attribute("title").Value,
                                       target = r.Attribute("target").Value,
                                       href = r.Attribute("href").Value
                                   };
                        foreach (var subItem in subq)
                        {
                            MenuItem subMi = new MenuItem();
                            subMi.Text = subItem.value;
                            subMi.NavigateUrl = subItem.href;
                            mi.ChildItems.Add(subMi);
                        }

                        menu1.Items.Add(mi);
                    }
                }
                else
                {
                    MenuItem mi = new MenuItem();
                    mi.Text = item.name;
                    mi.Selectable = false;
                    var subq = from r in root.Descendants("nav")
                               where r.Parent.Attribute("name").Value == item.name
                               orderby r.Attribute("id").Value
                               select new
                               {
                                   key = r.Attribute("key").Value,
                                   name = r.Attribute("name").Value,
                                   value = r.Attribute("value").Value,
                                   title = r.Attribute("title").Value,
                                   target = r.Attribute("target").Value,
                                   href = r.Attribute("href").Value
                               };
                    foreach (var subItem in subq)
                    {
                        MenuItem subMi = new MenuItem();
                        subMi.Text = subItem.value;
                        subMi.NavigateUrl = subItem.href;
                        mi.ChildItems.Add(subMi);
                    }

                    menu1.Items.Add(mi);
                }
            }

        }
    }
}