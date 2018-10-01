using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// UploadService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class UploadService : System.Web.Services.WebService
    {
        WebHelper.UploadFilesHelper ufh;

        [WebMethod]
        public void UploadifyUpload()
        {
            HttpContext context = HttpContext.Current;
            HttpPostedFile files = context.Request.Files["Filedata"];

            if (ufh == null) ufh = new WebHelper.UploadFilesHelper();
            string fileUrl;
            try
            {
                fileUrl = ufh.UploadToTemp(files);
            }
            catch
            {
                throw;
            }

            context.Response.Write(fileUrl);
            context.Response.End();
        }
    }
}
