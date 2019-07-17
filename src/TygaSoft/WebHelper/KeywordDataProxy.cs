using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using TygaSoft.CacheDependencyHelper;

namespace TygaSoft.WebHelper
{
    public class KeywordDataProxy
    {
        private static readonly bool enableCaching = bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]);
        private static readonly int keywordTimeout = int.Parse(ConfigurationManager.AppSettings["KeywordCacheDuration"]);

        public static List<string> GetKeywords()
        {
            BLL.SearchKeyword bll = new BLL.SearchKeyword();

            if (!enableCaching)
                return bll.GetKeywords();

            string key = "keyword_all";
            List<string> data = (List<string>)HttpRuntime.Cache[key];

            if (data == null)
            {
                data = bll.GetKeywords();

                AggregateCacheDependency cd = DependencyFactory.GetProvinceCityDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(keywordTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }
    }
}
