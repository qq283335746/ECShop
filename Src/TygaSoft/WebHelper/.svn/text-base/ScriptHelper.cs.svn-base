using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace TygaSoft.WebHelper
{
    public class ScriptHelper
    {
        /// <summary>
        /// 页面注册脚本（客户端方式）：动态加载js文件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        public static void Create(Page page, string key, string url)
        {
            ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered(key))
            {
                csm.RegisterClientScriptInclude(key, url);
            }
        }
    }
}
