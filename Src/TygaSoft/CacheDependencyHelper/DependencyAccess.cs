using System.Reflection;
using System.Configuration;

namespace TygaSoft.CacheDependencyHelper
{
    public static class DependencyAccess
    {
        public static ICacheDependency CreateProvinceCityDependency()
        {
            return LoadInstance("ProvinceCity");
        }

        public static ICacheDependency CreateCategoryDependency()
        {
            return LoadInstance("Category");
        }

        public static ICacheDependency CreateProductDependency()
        {
            return LoadInstance("Product");
        }

        public static ICacheDependency CreateContentDependency()
        {
            return LoadInstance("SiteContent");
        }

        public static ICacheDependency CreateItemDependency()
        {
            return LoadInstance("Item");
        }

        private static ICacheDependency LoadInstance(string className)
        {
            string path = ConfigurationManager.AppSettings["CacheDependencyAssembly"];
            string fullyQualifiedClass = path + "." + className;

            return (ICacheDependency)Assembly.Load(path).CreateInstance(fullyQualifiedClass);
        }
    }
}
