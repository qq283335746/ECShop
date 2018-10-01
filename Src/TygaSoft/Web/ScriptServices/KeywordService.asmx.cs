using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// KeywordService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class KeywordService : System.Web.Services.WebService
    {
        [WebMethod]
        public List<string> GetHotKeywords()
        {
            int count = 10;
            List<string> list = WebHelper.KeywordDataProxy.GetKeywords();
            return list.Take(count).ToList();
        }

        [WebMethod]
        public List<string> GetKeywords(string prefixText, int count)
        {
            if (count == 0) count = 10;
            List<string> list = WebHelper.KeywordDataProxy.GetKeywords();
            if (list != null)
            {
                List<string> KeywordList = list.FindAll(delegate(string s) { return s.ToLower().Contains(prefixText.ToLower()); });

                return KeywordList;
            }

            return null;
        }
    }
}
