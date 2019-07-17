using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using TygaSoft.CacheDependencyHelper;

namespace TygaSoft.WebHelper
{
    public class SiteContentDataProxy
    {
        private static readonly bool enableCaching = bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]);
        private static readonly int siteContentTimeout = int.Parse(ConfigurationManager.AppSettings["SiteContentCacheDuration"]);

        /// <summary>
        /// 获取当前根节点下的所有内容类型和内容
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static List<Model.ContentDetailInfo> GetSiteContent()
        {
            BLL.ContentDetail bll = new BLL.ContentDetail();

            if (!enableCaching)
                return bll.GetSiteContent("站点所有帮助");

            string key = "sitecontent_all";
            List<Model.ContentDetailInfo> data = (List<Model.ContentDetailInfo>)HttpRuntime.Cache[key];

            if (data == null)
            {
                data = bll.GetSiteContent("站点所有帮助");

                AggregateCacheDependency cd = DependencyFactory.GetProvinceCityDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(siteContentTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }
    }
}
