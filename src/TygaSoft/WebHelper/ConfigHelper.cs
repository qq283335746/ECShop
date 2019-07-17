using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace TygaSoft.WebHelper
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取应用程序web.config中的文件配置路径
        /// 适用于配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            //获取应用程序的web.config中配置的路径
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }

        /// <summary>
        /// 获取应用程序web.config中的文件配置路径，并返回物理路径
        /// 适用于web应用程序
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetFullPath(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;

            //获取应用程序的web.config中配置的路径
            string appSetting = System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            //如果到的路径不是物理路径，则映射为物理路径
            if (!Path.IsPathRooted(appSetting)) appSetting = System.Web.HttpContext.Current.Server.MapPath(appSetting);

            return appSetting;
        }
    }
}
