using System;

namespace TygaSoft.CacheDependencyHelper
{
    public class Category : MsSqlCacheDependency
    {
        public Category() : base("CategoryTableDependency") { }
    }
}
