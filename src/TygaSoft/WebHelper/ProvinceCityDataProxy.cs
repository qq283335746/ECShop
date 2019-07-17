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
    public class ProvinceCityDataProxy
    {
        private static readonly bool enableCaching = bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]);
        private static readonly int provinceCityTimeout = int.Parse(ConfigurationManager.AppSettings["ProvinceCityCacheDuration"]);

        /// <summary>
        /// This method acts as a proxy between the web and business components to check whether the 
        /// underlying data has already been cached.
        /// </summary>
        /// <returns>List of Model.ProvinceCity from Cache or Business component</returns>
        public static List<Model.ProvinceCityInfo> GetListProvinceCity()
        {
            BLL.ProvinceCity pcBll = new BLL.ProvinceCity();

            if (!enableCaching)
                return pcBll.GetList();

            string key = "provinceCity_all";
            List<Model.ProvinceCityInfo> data = (List<Model.ProvinceCityInfo>)HttpRuntime.Cache[key];

            // Check if the data exists in the data cache
            if (data == null)
            {
                // If the data is not in the cache then fetch the data from the business logic tier
                data = pcBll.GetList();

                // Create a AggregateCacheDependency object from the factory
                AggregateCacheDependency cd = DependencyFactory.GetProvinceCityDependency();

                // Store the output in the data cache, and Add the necessary AggregateCacheDependency object
                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(provinceCityTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }

    }
}
