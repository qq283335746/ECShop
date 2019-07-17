using System;

namespace TygaSoft.CacheDependencyHelper
{
    public class Product : MsSqlCacheDependency
    {
        public Product() : base("ProductTableDependency") { }
    }
}
