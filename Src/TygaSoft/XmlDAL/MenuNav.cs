using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using TygaSoft.IDAL;

namespace TygaSoft.XmlDAL
{
    public class MenuNav : IMenuNav
    {
        private static readonly string menusXml = ConfigurationManager.AppSettings["MenusXml"];

        public void Create(string file)
        {
            DateTime d = DateTime.Now;
            XDocument xd = new XDocument(
                new XElement("Datas",
                    new XElement("Data",
                        new XAttribute("ID", "Tabs"),
                        new XElement("Add",
                            new XAttribute("Href", "~/Admin/Default.aspx"),
                            new XAttribute("Url", "~/Admin/Default.aspx"),
                            new XAttribute("Title", GetTitle("~/Admin/Default.aspx")),
                            new XAttribute("CreateDate", d),
                            new XAttribute("UpdateDate", d)
                        )
                    ),
                    new XElement("Data",
                        new XAttribute("ID", "Layout"),
                        new XElement("Add",
                            new XAttribute("ID", "West"),
                            new XAttribute("Status", 0)
                        ),
                        new XElement("Add",
                            new XAttribute("ID", "East"),
                            new XAttribute("Status", 0)
                        )
                    )
                )
            );

            xd.Save(file);
        }

        public void Create(string file, string initPage)
        {
            DateTime d = DateTime.Now;
            XDocument xd = new XDocument(
                new XElement("Datas",
                    new XElement("Data",
                        new XAttribute("ID", "Tabs"),
                        new XElement("Add",
                            new XAttribute("Href", initPage),
                            new XAttribute("Url", initPage),
                            new XAttribute("Title", GetTitle(initPage)),
                            new XAttribute("CreateDate", d),
                            new XAttribute("UpdateDate", d)
                        )
                    ),
                    new XElement("Data",
                        new XAttribute("ID", "Layout"),
                        new XElement("Add",
                            new XAttribute("ID", "West"),
                            new XAttribute("Status", 0)
                        ),
                        new XElement("Add",
                            new XAttribute("ID", "East"),
                            new XAttribute("Status", 0)
                        )
                    )
                )
            );

            xd.Save(file);
        }

        public void RequestSave(string href, string url, string file)
        {
            DateTime d = DateTime.Now;
            XElement root = XElement.Load(file);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Tabs" && r.Attribute("Href").Value.ToLower() == href.ToLower()
                    select r;
            if (q != null && q.Count() > 0)
            {
                foreach (var item in q)
                {
                    item.SetAttributeValue("Url", url);
                    item.SetAttributeValue("UpdateDate", d);
                    root.Save(file);
                }
            }
            else
            {
                XElement node = new XElement("Add",
                    new XAttribute("Href", href),
                    new XAttribute("Url", url),
                    new XAttribute("Title", GetTitle(href)),
                    new XAttribute("CreateDate", d),
                    new XAttribute("UpdateDate", d)
                );

                root.Descendants("Data").Where(r => r.Attribute("ID").Value == "Tabs").First().Add(node);
                root.Save(file);
            }
        }

        public List<Model.MenuNavInfo> GetList(string file)
        {
            List<Model.MenuNavInfo> list = null;

            XElement root = XElement.Load(file);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Tabs"
                    orderby DateTime.Parse(r.Attribute("CreateDate").Value)
                    select r;

            if (q.Count() > 0)
            {
                list = new List<Model.MenuNavInfo>();
                DateTime maxDt = q.Max(x => DateTime.Parse(x.Attribute("UpdateDate").Value));
                foreach (var item in q)
                {
                    Model.MenuNavInfo model = new Model.MenuNavInfo();
                    model.Href = item.Attribute("Href").Value;
                    model.Url = item.Attribute("Url").Value;
                    model.Title = item.Attribute("Title").Value;
                    model.CreateDate = DateTime.Parse(item.Attribute("CreateDate").Value);
                    model.UpdateDate = DateTime.Parse(item.Attribute("UpdateDate").Value);
                    model.Selected = maxDt.Equals(model.UpdateDate);
                    list.Add(model);
                }
            }

            return list;
        }

        public void Delete(string url, string file)
        {
            XElement root = XElement.Load(file);
            IEnumerable<XElement> matches = from r in root.Descendants("Add")
                                            where r.Parent.Attribute("ID").Value == "Tabs" && r.Attribute("Url").Value == url
                                            select r;
            if (matches.Count() > 0)
            {
                matches.Remove();
                root.Save(file);
            }
        }

        public void InsertForLayout(string name, string status, string file)
        {
            XElement root = XElement.Load(file);
            var q = from r in root.Descendants("Data")
                    where r.Attribute("ID").Value == "Layout"
                    select r;

            if (q == null) return;

            name = name.ToLower();
            var qc = from r in root.Descendants("Add")
                     where r.Parent.Attribute("ID").Value == "Layout" && r.Attribute("ID").Value.ToLower() == name
                     select r;
            if (qc != null && qc.Count() > 0)
            {
                foreach (var item in qc)
                {
                    item.SetAttributeValue("Status", status);
                    root.Save(file);
                }
            }
            else
            {
                XElement node = new XElement("Add",
                                       new XAttribute("ID", name),
                                       new XAttribute("Status", status)
                                       );
                q.First().Add(node);
                root.Save(file);
            }
        }

        public int GetLayoutStatus(string file, string name)
        {
            XElement root = XElement.Load(file);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Layout" && r.Attribute("ID").Value.ToLower() == name.ToLower()
                    select new { Status = r.Attribute("Status").Value };
            return int.Parse(q.First().Status);
        }

        public Dictionary<string, string> GetListForLayout(string file)
        {
            Dictionary<string, string> dic = null;

            XElement root = XElement.Load(file);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Layout"
                    select new { ID = r.Attribute("ID").Value, Status = r.Attribute("Status").Value };

            if (q.Count() > 0)
            {
                dic = new Dictionary<string, string>();
                foreach (var item in q)
                {
                    dic.Add(item.ID, item.Status);
                }
            }

            return dic;
        }

        public List<Model.MenuItemInfo> GetMenus()
        {
            List<Model.MenuItemInfo> list = null;

            XElement root = XElement.Load(HttpContext.Current.Server.MapPath(menusXml));
            var q = from r in root.Descendants("Add")
                    select r;

            if (q.Count() > 0)
            {
                list = new List<Model.MenuItemInfo>();
                foreach (var item in q)
                {
                    Model.MenuItemInfo model = new Model.MenuItemInfo();
                    model.Title = item.Attribute("title").Value;
                    model.Url = item.Attribute("url").Value;
                    model.Description = item.Attribute("description").Value;
                    model.Roles = item.Attribute("roles").Value;

                    list.Add(model);
                }
            }

            return list;
        }

        public string GetTitle(string url)
        {
            XElement root = XElement.Load(HttpContext.Current.Server.MapPath(menusXml));
            var q = from r in root.Descendants("add")
                    where r.Attribute("url").Value.ToLower() == url.ToLower()
                    select r;
            if (q.Count() > 0) return q.First().Attribute("title").Value;
            else return string.Empty;
        }
    }
}
