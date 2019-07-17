using System.Configuration;
using System.Web.Caching;
using System.Collections.Generic;

namespace TygaSoft.CacheDependencyHelper
{
    public static class DependencyFactory
    {
        private static readonly string path = ConfigurationManager.AppSettings["CacheDependencyAssembly"];

        public static AggregateCacheDependency GetProvinceCityDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateProvinceCityDependency().GetDependency();
            else
                return null;
        }

        public static AggregateCacheDependency GetCategoryDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateCategoryDependency().GetDependency();
            else
                return null;
        }

        public static AggregateCacheDependency GetProductDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateProductDependency().GetDependency();
            else
                return null;
        }

        public static AggregateCacheDependency GetContentDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateContentDependency().GetDependency();
            else
                return null;
        }

        public static AggregateCacheDependency GetItemDependency()
        {
            if (!string.IsNullOrEmpty(path))
                return DependencyAccess.CreateItemDependency().GetDependency();
            else
                return null;
        }
    }
}
