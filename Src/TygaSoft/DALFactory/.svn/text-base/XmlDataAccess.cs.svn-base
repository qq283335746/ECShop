using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace TygaSoft.DALFactory
{
    public class XmlDataAccess
    {
        private static readonly string path = ConfigurationManager.AppSettings["WebXmlDAL"];

        public static IDAL.IHandlerUserProfile CreateHandlerUserProfile()
        {
            string className = path + ".HandlerUserProfile";
            return (IDAL.IHandlerUserProfile)Assembly.Load(path).CreateInstance(className);
        }

        public static IDAL.IWebSitemap CreateWebSitemap()
        {
            string className = path + ".WebSitemap";
            return (IDAL.IWebSitemap)Assembly.Load(path).CreateInstance(className);
        }

        public static IDAL.IMenuNav CreateMenuNav()
        {
            string className = path + ".MenuNav";
            return (IDAL.IMenuNav)Assembly.Load(path).CreateInstance(className);
        }
    }
}
