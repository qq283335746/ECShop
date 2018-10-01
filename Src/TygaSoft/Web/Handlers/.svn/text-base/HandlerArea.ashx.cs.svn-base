using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Caching;
namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// HandlerArea 的摘要说明
    /// </summary>
    public class HandlerArea : IHttpHandler
    {
        XElement root;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (!string.IsNullOrEmpty(context.Request.QueryString["areaName"]))
            {
                List<Model.ProvinceCityInfo> list = WebHelper.ProvinceCityDataProxy.GetListProvinceCity();
                if (list == null || list.Count == 0) return;

                string areaName = context.Request.QueryString["areaName"];
                switch (areaName)
                {
                    case "highUse":
                        GetHighUse();
                        break;
                    case "p":
                        GetProvince(list);
                        break;
                    case "c":
                        GetCityByProvince(list);
                        break;
                    case "v":
                        GetVillageByCity(list);
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 常用城市
        /// </summary>
        private void GetHighUse()
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/AreaHighUse.xml"));
            if (root != null && root.HasElements)
            {
                var q = from r in root.Descendants("Add")
                        where r.Parent.Attribute("ID").Value == "HighUse"
                        select new
                        {
                            NumberID = r.Attribute("NumberID").Value,
                            RegionName = r.Attribute("RegionName").Value,
                            Pname = r.Attribute("Pname").Value,
                            Pid = r.Attribute("Pid").Value,
                        };
                string htmlAppend = string.Empty;
                htmlAppend = "<ul class=\"horizontal\">";

                foreach (var item in q)
                {
                    string[] items = item.NumberID.Split(',');
                    htmlAppend += string.Format("<li><a href=\"javascript:void(0);\" onclick=\"onHighUseClick('{2}','{0}','{3}','{1}')\">{0}</a></li>", item.RegionName, item.NumberID, item.Pname, item.Pid);
                }

                htmlAppend += "</ul>";

                HttpContext.Current.Response.Write(htmlAppend);
            }
        }

        /// <summary>
        /// 获取所有的省份
        /// </summary>
        /// <param name="list"></param>
        private void GetProvince(List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<ul class=\"horizontal\">";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    if (model.ParentID == "1")
                    {
                        textAppend += string.Format("<li><a href=\"javascript:void(0);\" onclick=\"onProvinceClick('{1}','{0}')\">{0}</a></li>", model.RegionName, model.NumberID);
                    }
                }

                textAppend += "</ul>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 根据当前的省份ID获取对应的所有城市
        /// </summary>
        /// <param name="list"></param>
        /// <param name="provinceId"></param>
        private void GetCityByProvince(List<Model.ProvinceCityInfo> list)
        {
            string htmlAppend = string.Empty;
            string pId = HttpContext.Current.Request.QueryString["pId"];
            if (!string.IsNullOrEmpty(pId))
            {
                if (list != null)
                {
                    htmlAppend = "<ul class=\"horizontal\">";

                    foreach (Model.ProvinceCityInfo model in list)
                    {
                        if (model.ParentID == pId)
                        {
                            htmlAppend += string.Format("<li><a href=\"javascript:void(0);\" onclick=\"onCityClick('{1}','{0}')\">{0}</a></li>", model.RegionName, model.NumberID);
                        }
                    }

                    htmlAppend += "</ul>";
                }
            }

            HttpContext.Current.Response.Write(htmlAppend);
        }

        /// <summary>
        /// 获取当前城市对应的所有区县
        /// </summary>
        /// <param name="list"></param>
        private void GetVillageByCity(List<Model.ProvinceCityInfo> list)
        {
            string htmlAppend = string.Empty;
            string cId = HttpContext.Current.Request.QueryString["cId"];
            if (!string.IsNullOrEmpty(cId))
            {
                if (list != null)
                {
                    htmlAppend = "<ul class=\"horizontal\">";

                    foreach (Model.ProvinceCityInfo model in list)
                    {
                        if (model.ParentID == cId)
                        {
                            htmlAppend += string.Format("<li><a href=\"javascript:void(0);\" onclick=\"onVillageClick('{1}','{0}')\">{0}</a></li>", model.RegionName, model.NumberID);
                        }
                    }

                    htmlAppend += "</ul>";
                }
            }

            HttpContext.Current.Response.Write(htmlAppend);
        }
    }
}