using System;

namespace TygaSoft.CacheDependencyHelper
{
    public class ProvinceCity : MsSqlCacheDependency
    {
        /// <summary>
        /// Call its base constructor by passing its specific configuration key
        /// </summary>
        public ProvinceCity() : base("ProvinceCityTableDependency") { }
    }
}
