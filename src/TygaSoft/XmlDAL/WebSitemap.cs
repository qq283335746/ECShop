using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using TygaSoft.IDAL;

namespace TygaSoft.XmlDAL
{
    public class WebSitemap : IWebSitemap
    {
        XElement root;

        public List<Model.WebSitemapInfo> GetList()
        {
            List<Model.WebSitemapInfo> list = null;

            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/Web.sitemap"));
            var q = from r in root.Elements().Elements()
                    select r;

            if (q != null)
            {
                list = new List<Model.WebSitemapInfo>();
                foreach (var item in q)
                {
                    Model.WebSitemapInfo model = new Model.WebSitemapInfo();
                    model.Url = item.Attribute("url").Value;
                    model.Title = item.Attribute("title").Value;
                    model.Description = item.Attribute("description").Value;
                    model.Roles = item.Attribute("roles").Value;

                    list.Add(model);

                    CreateSubMi(model.Title, list);
                }
            }

            return list;
        }

        private void CreateSubMi(string parentTitle,List<Model.WebSitemapInfo> list)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/Web.sitemap"));
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
                Model.WebSitemapInfo model = new Model.WebSitemapInfo();
                model.Url = item.url;
                model.Title = item.title;
                model.Description = item.description;
                model.Roles = item.roles;

                list.Add(model);

                CreateSubMi(model.Title, list);
            }
        }
    }
}
