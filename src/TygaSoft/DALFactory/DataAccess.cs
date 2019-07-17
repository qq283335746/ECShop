using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using TygaSoft.IDAL;

namespace TygaSoft.DALFactory
{
    public sealed class DataAccess
    {
        private static readonly string path = ConfigurationManager.AppSettings["WebDAL"];

        public static ICategory CreateCategory()
        {
            string className = path + ".Category";
            return (ICategory)Assembly.Load(path).CreateInstance(className);
        }

        public static IDAL.IProduct CreateProduct()
        {
            string className = path + ".Product";
            return (IProduct)Assembly.Load(path).CreateInstance(className);
        }

        public static ISystemProfile CreateSystemProfile()
        {
            string className = path + ".SystemProfile";
            return (ISystemProfile)Assembly.Load(path).CreateInstance(className);
        }

        public static IProvinceCity CreateProvinceCity()
        {
            string className = path + ".ProvinceCity";
            return (IProvinceCity)Assembly.Load(path).CreateInstance(className);
        }

        public static IOrder CreateOrder()
        {
            string className = path + ".Order";
            return (IOrder)Assembly.Load(path).CreateInstance(className);
        }

        public static ISearchKeyword CreateSearchKeyword()
        {
            string className = path + ".SearchKeyword";
            return (ISearchKeyword)Assembly.Load(path).CreateInstance(className);
        }

        public static IContentType CreateContentType()
        {
            string className = path + ".ContentType";
            return (IContentType)Assembly.Load(path).CreateInstance(className);
        }

        public static IContentDetail CreateContentDetail()
        {
            string className = path + ".ContentDetail";
            return (IContentDetail)Assembly.Load(path).CreateInstance(className);
        }

        public static IBbsContentType CreateBbsContentType()
        {
            string className = path + ".BbsContentType";
            return (IBbsContentType)Assembly.Load(path).CreateInstance(className);
        }

        public static IBbsContentDetail CreateBbsContentDetail()
        {
            string className = path + ".BbsContentDetail";
            return (IBbsContentDetail)Assembly.Load(path).CreateInstance(className);
        }
      
    }
}
