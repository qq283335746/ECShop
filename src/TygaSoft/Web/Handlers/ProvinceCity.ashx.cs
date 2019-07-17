using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Caching;

namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// ProvinceCity 的摘要说明
    /// </summary>
    public class ProvinceCity : IHttpHandler
    {

        XElement root;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            //绑定常用
            BindHighUse();

            List<Model.ProvinceCityInfo> list = WebHelper.ProvinceCityDataProxy.GetListProvinceCity();
            if (list == null || list.Count == 0) return;

            //绑定省份
            List<Model.ProvinceCityInfo> provinceItems = list.FindAll(delegate(Model.ProvinceCityInfo pc) { return pc.ParentID == "1"; });
            ConsoleProvince(provinceItems);

            if (!string.IsNullOrEmpty(context.Request.QueryString["pcity"]))
            {
                string pcity = context.Request.QueryString["pcity"].Trim();
                string[] pcityArr = pcity.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int pcityArrLen = pcityArr.Length;

                if (pcityArrLen == 1)
                {
                    Model.ProvinceCityInfo model = list.Find(delegate(Model.ProvinceCityInfo pc) { return pc.RegionName == pcityArr[0]; });
                    List<Model.ProvinceCityInfo> cityItems = list.FindAll(delegate(Model.ProvinceCityInfo pc) { return pc.ParentID == model.NumberID; });
                    ConsoleCity(cityItems);
                }
                else if (pcityArrLen > 1)
                {
                    Model.ProvinceCityInfo model = list.Find(delegate(Model.ProvinceCityInfo pc) { return pc.RegionName == pcityArr[1]; });
                    List<Model.ProvinceCityInfo> cityItems = list.FindAll(delegate(Model.ProvinceCityInfo pc) { return pc.ParentID == model.ParentID; });
                    ConsoleCity(cityItems);
                    List<Model.ProvinceCityInfo> countyItems = list.FindAll(delegate(Model.ProvinceCityInfo pc) { return pc.ParentID == model.NumberID; });
                    ConsoleCounty(countyItems);
                }
            }

            //if (!string.IsNullOrEmpty(context.Request.QueryString["op"]) || !string.IsNullOrEmpty(context.Request.QueryString["pcity"]))
            //{
            //    string pcity = context.Request.QueryString["pcity"].Trim();
            //    string[] pcityArr = pcity.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            //    int pcityArrLen = pcityArr.Length;
            //    if (pcityArrLen < 1) return;
            //    //获取当前城市节点
            //    Model.ProvinceCity model = list.Find(delegate(Model.ProvinceCity pc) { return pc.RegionName == pcityArr[1]; });
            //    string op = context.Request.QueryString["op"].Trim();
            //    if (op == "m")
            //    {
            //        if (pcityArrLen == 1)
            //        { 

            //        }
            //        List<Model.ProvinceCity> cityItems = list.FindAll(delegate(Model.ProvinceCity pc) { return pc.ParentID == model.ParentID; });
            //        ConsoleCity(cityItems);
            //        List<Model.ProvinceCity> countyItems = list.FindAll(delegate(Model.ProvinceCity pc) { return pc.ParentID == model.NumberID; });
            //        ConsoleCounty(countyItems);
            //    }
            //    else if (op == "s")
            //    {
            //        if (pcityArrLen == 1)
            //        {
            //            List<Model.ProvinceCity> cityItems = list.FindAll(delegate(Model.ProvinceCity pc) { return pc.ParentID == model.NumberID; });
            //            ConsoleCity(cityItems);
            //        }
            //        else
            //        {
            //            List<Model.ProvinceCity> countyItems = list.FindAll(delegate(Model.ProvinceCity pc) { return pc.ParentID == model.NumberID; });
            //            ConsoleCounty(countyItems);
            //        }
            //    }
            //}

            //if (!string.IsNullOrEmpty(context.Request.QueryString["idAppend"]))
            //{
            //    string idAppend = string.Empty;
            //    idAppend = context.Request.QueryString["idAppend"];
            //    //获取当前常用城市的所有区县和所属省份的所有城市
            //    GetHighUse(idAppend, list);
            //}
            //else if (!string.IsNullOrEmpty(context.Request.QueryString["provinceBind"]))
            //{
            //    //获取所有省份
            //    GetProvince(list);
            //    return;
            //}
            //else if (!string.IsNullOrEmpty(context.Request.QueryString["parentId"]))
            //{
            //    string parentId = string.Empty;
            //    parentId = context.Request.QueryString["parentId"];
            //    //获取当前省份的所有城市
            //    GetCityByProvince(parentId, list);
            //}
            //else if (!string.IsNullOrEmpty(context.Request.QueryString["provinceId"]))
            //{
            //    string provinceId = string.Empty;
            //    provinceId = context.Request.QueryString["provinceId"];
            //    //获取当前城市的所有区县
            //    GetCityByProvince(provinceId, list);
            //}
            //else if (!string.IsNullOrEmpty(context.Request.QueryString["cityId"]))
            //{
            //    string cityId = string.Empty;
            //    cityId = context.Request.QueryString["cityId"];
            //    //获取当前父级下的所有子级
            //    GetCountyByCity(cityId, list);
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化绑定常用省市
        /// </summary>
        private void BindHighUse()
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/ProvinceCity.xml"));
            if (root != null && root.HasElements)
            {
                var q = from r in root.Descendants("Add")
                        where r.Parent.Attribute("ID").Value == "HighUse"
                        select new
                        {
                            NumberID = r.Attribute("NumberID").Value,
                            RegionName = r.Attribute("RegionName").Value
                        };
                string textAppend = string.Empty;
                textAppend = "<div id=\"highUse\"> <ul>";

                foreach (var item in q)
                {
                    textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", item.RegionName, item.NumberID);
                }

                textAppend += "</ul></div>";

                HttpContext.Current.Response.Write(textAppend);
            }
        }

        /// <summary>
        /// 常用城市
        /// </summary>
        /// <param name="idAppend"></param>
        private void GetHighUse(string idAppend, List<Model.ProvinceCityInfo> list)
        {
            string[] items = idAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            GetCityByProvince(items[0], list);
            if (items.Length > 1)
            {
                GetCountyByCity(items[1], list);
            }
        }

        /// <summary>
        /// 获取所有的省份
        /// </summary>
        private void GetProvince(List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"province\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    if (model.ParentID == "1")
                    {
                        textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                    }
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 根据当前的省份ID获取对应的所有城市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <param name="list"></param>
        private void GetCityByProvince(string provinceId, List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"city\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    if (model.ParentID == provinceId)
                    {
                        textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                    }
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 获取当前城市对应的所有区县
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="list"></param>
        private void GetCountyByCity(string cityId, List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"county\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    if (model.ParentID == cityId)
                    {
                        textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                    }
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 根据当前的父级ID获取对应的子级数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="list"></param>
        private void GetProvinceCity(string parentId, IList<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    if (model.ParentID == parentId)
                    {
                        textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                    }
                }

                textAppend += "</ul>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 输出省份
        /// </summary>
        /// <param name="list"></param>
        private void ConsoleProvince(List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"province\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 输出城市
        /// </summary>
        /// <param name="list"></param>
        private void ConsoleCity(List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"city\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }

        /// <summary>
        /// 输出区县
        /// </summary>
        /// <param name="list"></param>
        private void ConsoleCounty(List<Model.ProvinceCityInfo> list)
        {
            string textAppend = string.Empty;
            if (list != null)
            {
                textAppend = "<div id=\"county\"> <ul>";

                foreach (Model.ProvinceCityInfo model in list)
                {
                    textAppend += string.Format("<li><a class=\"panel-item\" href=\"javascript:;\" code=\"{1}\">{0}</a></li>", model.RegionName, model.NumberID);
                }

                textAppend += "</ul></div>";
            }

            HttpContext.Current.Response.Write(textAppend);
        }
    }
}