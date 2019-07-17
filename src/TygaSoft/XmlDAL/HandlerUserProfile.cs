using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using TygaSoft.IDAL;

namespace TygaSoft.XmlDAL
{
    public class HandlerUserProfile : IHandlerUserProfile
    {
        public void Create(string userFile)
        {
            DateTime d = DateTime.Now;
            XDocument xd = new XDocument(
                new XElement("Datas",
                    new XElement("Data",
                        new XAttribute("ID", "Tabs"),
                        new XElement("Add",
                            new XAttribute("Href", "~/Admin/Default.aspx"),
                            new XAttribute("LowerHref", "~/Admin/Default.aspx".ToLower()),
                            new XAttribute("Url", "~/Admin/Default.aspx"),
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

            xd.Save(userFile);
        }

        public void Create(string userFile, string initPage)
        {
            DateTime d = DateTime.Now;
            XDocument xd = new XDocument(
                new XElement("Datas",
                    new XElement("Data",
                        new XAttribute("ID", "Tabs"),
                        new XElement("Add",
                            new XAttribute("Href", initPage),
                            new XAttribute("LowerHref", initPage.ToLower()),
                            new XAttribute("Url", initPage),
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

            xd.Save(userFile);
        }

        public void RequestSave(string href,string url, string userFile)
        {
            DateTime d = DateTime.Now;
            XElement root = XElement.Load(userFile);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Tabs" && r.Attribute("LowerHref").Value == href.ToLower()
                    select r;
            if (q != null && q.Count() > 0)
            {
                foreach (var item in q)
                {
                    item.SetAttributeValue("Url", url);
                    item.SetAttributeValue("UpdateDate", d);
                    root.Save(userFile);
                }
            }
            else
            { 
                XElement node = new XElement("Add",
                    new XAttribute("Href", href),
                    new XAttribute("LowerHref", href.ToLower()),
                    new XAttribute("Url", url),
                    new XAttribute("CreateDate", d),
                    new XAttribute("UpdateDate", d)
                );

                root.Descendants("Data").Where(r => r.Attribute("ID").Value == "Tabs").First().Add(node);
                root.Save(userFile);
            }
        }

        public List<Model.HandlerUserProfileInfo> GetList(string userFile)
        {
            List<Model.HandlerUserProfileInfo> list = null;

            XElement root = XElement.Load(userFile);
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("ID").Value == "Tabs"
                    orderby DateTime.Parse(r.Attribute("CreateDate").Value)
                    select new { Href = r.Attribute("Href").Value, LowerHref = r.Attribute("LowerHref").Value, Url = r.Attribute("Url").Value, CreateDate = r.Attribute("CreateDate").Value, UpdateDate = r.Attribute("UpdateDate").Value };

            if (q.Count() > 0)
            {
                list = new List<Model.HandlerUserProfileInfo>();
                foreach (var item in q)
                {
                    Model.HandlerUserProfileInfo model = new Model.HandlerUserProfileInfo();
                    model.Href = item.Href;
                    model.LowerHref = item.LowerHref;
                    model.Url = item.Url;
                    model.CreateDate = DateTime.Parse(item.CreateDate);
                    model.UpdateDate = DateTime.Parse(item.UpdateDate);
                    list.Add(model);
                }
            }

            return list;
        }

        public void Delete(string url, string userFile)
        {
            XElement root = XElement.Load(userFile);
            IEnumerable<XElement> matches = from r in root.Descendants("Add")
                                            where r.Parent.Attribute("ID").Value == "Tabs" && r.Attribute("Url").Value == url
                                            select r;
            if (matches.Count() > 0)
            {
                matches.Remove();
                root.Save(userFile);
            }
        }

        public void InsertForLayout(string name, string status, string userFile)
        {
            XElement root = XElement.Load(userFile);
            var q = from r in root.Descendants("Data")
                    where r.Attribute("ID").Value == "Layout"
                    select r;

            if (q == null) return;

            var qc = from r in root.Descendants("Add")
                     where r.Parent.Attribute("ID").Value == "Layout" && r.Attribute("ID").Value == name
                     select r;
            if (qc != null && qc.Count() > 0)
            {
                foreach (var item in qc)
                {
                    item.SetAttributeValue("Status", status);
                    root.Save(userFile);
                }
            }
            else
            {
                XElement node = new XElement("Add",
                                       new XAttribute("ID", name),
                                       new XAttribute("Status", status)
                                       );
                q.First().Add(node);
                root.Save(userFile);
            }
        }

        public Dictionary<string, string> GetListForLayout(string userFile)
        {
            Dictionary<string, string> dic = null;

            XElement root = XElement.Load(userFile);
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

    }
}
