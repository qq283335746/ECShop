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
    public class ProductDataProxy
    {
        private static readonly bool enableCaching = bool.Parse(ConfigurationManager.AppSettings["EnableCaching"]);
        private static readonly int productTimeout = int.Parse(ConfigurationManager.AppSettings["CategoryCacheDuration"]);

        public static List<Model.ProductInfo> GetProductsByCategory(string category)
        {
            BLL.Product bll = new BLL.Product();

            if (!enableCaching)
            {
                if (category.Contains(@"'"))
                {
                    return bll.GetProductsInCategories(category);
                }
                else
                {
                    return bll.GetProductsByCategory(category);
                }
            }

            string key = "product_by_category_" + category;
            List<Model.ProductInfo> data = (List<Model.ProductInfo>)HttpRuntime.Cache[key];

            if (data == null)
            {
                if (category.Contains(@"'"))
                {
                    data = bll.GetProductsInCategories(category);
                }
                else
                {
                    data = bll.GetProductsByCategory(category);
                }

                AggregateCacheDependency cd = DependencyFactory.GetProductDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(productTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }

        public static List<Model.ProductInfo> GetProductsBySearch(string text)
        {
            BLL.Product bll = new BLL.Product();

            if (!enableCaching)
                return bll.GetProductsBySearch(text);

            string key = "product_search_" + text;
            List<Model.ProductInfo> data = (List<Model.ProductInfo>)HttpRuntime.Cache[key];

            if (data == null)
            {
                data = bll.GetProductsBySearch(text);

                AggregateCacheDependency cd = DependencyFactory.GetProductDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(productTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }

        public static Model.ProductInfo GetProduct(string productId)
        {
            BLL.Product bll = new BLL.Product();

            if (!enableCaching)
                return bll.GetModel(productId);

            string key = "product_" + productId;
            Model.ProductInfo data = (Model.ProductInfo)HttpRuntime.Cache[key];

            if (data == null)
            {
                data = bll.GetModel(productId);

                AggregateCacheDependency cd = DependencyFactory.GetProductDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(productTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }

        public static List<Model.ProductInfo> GetProducts(int pageIndex, int pageSize, out int totalCount)
        {
            BLL.Product bll = new BLL.Product();
            totalCount = 0;

            if (!enableCaching)
                return bll.GetProducts(pageIndex,pageSize,out totalCount,"","",null);

            string key = string.Format("product_get_{0}_{1}", pageIndex, pageSize);
            string keyCount = string.Format("product_getCount_{0}_{1}", pageIndex, pageSize);
            List<Model.ProductInfo> data = (List<Model.ProductInfo>)HttpRuntime.Cache[key];
            object obj = HttpRuntime.Cache[keyCount];
            if (obj != null) totalCount = (int)obj;

            if (data == null)
            {
                data = bll.GetProducts(pageIndex, pageSize, out totalCount, "", "", null);

                AggregateCacheDependency cd = DependencyFactory.GetProductDependency();

                HttpRuntime.Cache.Add(key, data, cd, DateTime.Now.AddHours(productTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                HttpRuntime.Cache.Add(keyCount, totalCount, null, DateTime.Now.AddHours(productTimeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            return data;
        }
    }
}
