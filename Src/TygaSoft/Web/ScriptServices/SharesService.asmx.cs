using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// SharesService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class SharesService : System.Web.Services.WebService
    {
        HttpContext context;
        BLL.ContentType ctBll;
        BLL.ContentDetail cdBll;
        List<Model.ContentDetailInfo> cdList;

        //[WebMethod]
        ////public string GetSiteContent()
        //{
        //    if (cdBll == null) cdBll = new BLL.ContentDetail();

        //}

        [WebMethod]
        public string GetSiteHelper()
        {
            string htmlAppend = string.Empty;
            if (ctBll == null) ctBll = new BLL.ContentType();
            if (cdBll == null) cdBll = new BLL.ContentDetail();
            cdList = cdBll.GetList();
            List<Model.ContentTypeInfo> ctList = ctBll.GetList();
            Model.ContentTypeInfo rootModel = ctList.Find(delegate(Model.ContentTypeInfo m) { return m.TypeName == "站点所有帮助"; });
            if (rootModel != null)
            {
                htmlAppend += GetChildren(rootModel, ctList);
            }

            return htmlAppend;
        }

        [WebMethod]
        //public string GetContent(object contentTypeId)
        //{
 
        //}

        private string GetChildren(Model.ContentTypeInfo parentModel,List<Model.ContentTypeInfo> list)
        {
            string htmlAppend = string.Empty;

            List<Model.ContentTypeInfo> childList = list.FindAll(delegate(Model.ContentTypeInfo m) { return m.ParentID.Equals(parentModel.NumberID); });
            if (childList != null)
            {
                foreach (Model.ContentTypeInfo model in childList)
                {
                    string selected = "selected:false";
                    htmlAppend += "<div data-options='"+selected+"' title=\""+model.TypeName+"\" style=\"overflow:auto;padding:10px;\">";
                    htmlAppend += GetAccordionContent(model.NumberID, out selected);
                    htmlAppend += "</div>";
                }
            }

            return htmlAppend;
        }

        private string GetAccordionContent(object contentTypeId, out string selected)
        {
            context = HttpContext.Current;
            string nId = string.Empty;
            if (!string.IsNullOrEmpty(context.Request.QueryString["nId"]))
            {
                nId = context.Request.QueryString["nId"];
            }
            selected = "selected:false";
            string htmlAppend = string.Empty;

            if (cdList != null)
            {
                List<Model.ContentDetailInfo> list = cdList.FindAll(delegate(Model.ContentDetailInfo m) { return m.ContentTypeID.Equals(contentTypeId); });
                if (list != null)
                {
                    foreach (Model.ContentDetailInfo model in list)
                    {
                        if (model.NumberID.ToString() == nId)
                        {
                            selected = "selected:true";
                            htmlAppend += "<a href='javascript:void(0)' code='"+model.NumberID+"' class='hover' onclick='OnMenuSelect(this)'>" + model.Title + "</a>";
                        }
                        else
                        {
                            htmlAppend += "<a href='javascript:void(0)' code='" + model.NumberID + "' onclick='OnMenuSelect(this)'>" + model.Title + "</a>";
                        }
                    }
                }
            }

            return htmlAppend;
        }
    }
}
