using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using TygaSoft.CustomProviders;
using System.Web.Security;

namespace TygaSoft.Web.ScriptServices
{
    /// <summary>
    /// UsersService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class UsersService : System.Web.Services.WebService
    {
        HttpContext context;
        string[] roles;

        [WebMethod]
        public string GetMenus(string path)
        {
            string htmlAppend = string.Empty;

            XElement root = XElement.Load(Server.MapPath("~/Web.sitemap"));
            var q = from r in root.Elements().Elements()
                    select r;

            if (q.Count() > 0)
            {
                foreach (var item in q)
                {
                    if (UserInnRole(item.Attribute("roles").Value))
                    {
                        string selected = "selected:false";
                        string childAppend = GetChildren(item, ref selected, path);
                        htmlAppend += "<div data-options=\"" + selected + "\" title=\"" + item.Attribute("title").Value + "\" style=\"overflow:auto;padding:10px;\">";
                        htmlAppend += childAppend;
                        htmlAppend += "</div>";
                    }
                }
            }
            return htmlAppend;
        }

        private string GetChildren(XElement xel, ref string selected, string path)
        {
            string htmlAppend = string.Empty;
            var q = from i in xel.Elements()
                    select i;
            if (q.Count() > 0)
            {
                foreach (var item in q)
                {
                    if (UserInnRole(item.Attribute("roles").Value))
                    {
                        if (path.IndexOf(item.Attribute("title").Value) > -1)
                        {
                            selected = "selected:true";
                            htmlAppend += "<a href='" + item.Attribute("url").Value.Replace("~", "") + "' class='hover'>" + item.Attribute("title").Value + "</a>";
                        }
                        else
                        {
                            htmlAppend += "<a href='" + item.Attribute("url").Value.Replace("~", "") + "'>" + item.Attribute("title").Value + "</a>";
                        }
                    }
                }
            }

            return htmlAppend;
        }

        /// <summary>
        /// 当前用户是否具有当前角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private bool UserInnRole(string role)
        {
            if (roles == null)
            {
                roles = Roles.GetRolesForUser();
                //MsSqlRoleProvider p = (MsSqlRoleProvider)Roles.Provider;
                //roles = p.GetUserRolesFromTicket();
            }

            string firstRole = roles[0];
            string[] roleArr = role.Split(',');
            return roleArr.Contains(firstRole);
        }

        [WebMethod]
        public void OnLayout(int state, string name)
        {
            BLL.MenuNav mnBll = new BLL.MenuNav();
            mnBll.InsertForLayout(name, state.ToString(), GetMenuNavFile());
        }

        [WebMethod]
        public int GetLayoutByName(string name)
        {
            BLL.MenuNav mnBll = new BLL.MenuNav();
            return mnBll.GetLayoutStatus(GetMenuNavFile(), name);
        }

        [WebMethod]
        public List<Model.MenuNavInfo> GetTabs()
        {
            BLL.MenuNav mnBll = new BLL.MenuNav();
            CustomProfileCommon profile = new CustomProfileCommon();
            string fileName = string.Format("~/App_Data/UsersData/{0}.xml", profile.GetUserName());
            return mnBll.GetList(Server.MapPath(fileName));
        }

        [WebMethod]
        public string TabsClose(string url)
        {
            BLL.MenuNav mnBll = new BLL.MenuNav();
            mnBll.Delete(url, GetMenuNavFile());

            List<Model.MenuNavInfo> list = GetTabs();
            if (list != null && list.Count > 0)
            {
                Model.MenuNavInfo model = list.Find(m => m.UpdateDate == list.Max(mx => mx.UpdateDate));
                if (model != null)
                {
                    return model.Url;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取当前客户端对应的菜单导航文件
        /// </summary>
        /// <returns></returns>
        private string GetMenuNavFile()
        {
            context = HttpContext.Current;
            CustomProfileCommon profile = new CustomProfileCommon();
            string userName = profile.GetUserName();
            
            return context.Server.MapPath(string.Format("~/App_Data/UsersData/{0}.xml", userName));
        }

        [WebMethod]
        public string InsertUserCustomAttr(string attrName,string attrValue)
        {
            context = HttpContext.Current;
            CustomProfileCommon profile = new CustomProfileCommon();
            if (profile.UserCustomAttr.Count > 4)
            {
                return "已超过上限，最多保存5个自定义属性";
            }
            attrName = attrName.Trim();
            attrValue = attrValue.Trim();
            if (string.IsNullOrEmpty(attrName) || string.IsNullOrEmpty(attrValue))
            {
                return "属性名称或属性值不能为空，请检查";
            }
            List<Model.UserCustomAttrInfo> list = profile.UserCustomAttr.GetList();
            Model.UserCustomAttrInfo oldModel = list.Find(delegate(Model.UserCustomAttrInfo m)
            {
                return (m.AttrValue == attrValue);
            });
            if (oldModel != null)
            {
                return "已存在记录";
            }

            Model.UserCustomAttrInfo model = new Model.UserCustomAttrInfo();
            model.AttrName = attrName;
            model.AttrValue = attrValue;
            profile.UserCustomAttr.Insert(model);
            profile.Save();

            return "操作成功";
        }

        [WebMethod]
        public List<Model.UserCustomAttrInfo> GetUserCustomAttrs()
        {
            CustomProfileCommon profile = new CustomProfileCommon();
            List<Model.UserCustomAttrInfo> list = profile.UserCustomAttr.GetList();

            return list;
        }

        [WebMethod]
        public string DeleteUserCustomAttr(string attrName)
        {
            attrName = attrName.Trim();
            CustomProfileCommon profile = new CustomProfileCommon();
            List<Model.UserCustomAttrInfo> list = profile.UserCustomAttr.GetList();
            Model.UserCustomAttrInfo model = list.Find(delegate(Model.UserCustomAttrInfo m) { return m.AttrName.ToLower() == attrName.ToLower(); });
            profile.UserCustomAttr.Remove(model);
            profile.Save();

            return "操作成功";
        }      
    }
}
