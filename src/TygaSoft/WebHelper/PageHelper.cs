using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

namespace TygaSoft.WebHelper
{
    public class PageHelper
    {
        static XElement root;

        /// <summary>
        /// 获取当前请求文件相对于应用程序根的虚拟路径的前缀符号表示
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetVirtualUrlPrefix(Page page)
        {
            string vp = string.Empty;
            string arcefp = VirtualPathUtility.GetDirectory(HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
            int dirCount = arcefp.Substring(2).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (dirCount > 0)
            {
                for (int i = 0; i < dirCount; i++)
                {
                    vp += "../";
                }
            }

            return vp;
        }

        /// <summary>
        /// 动态加载公共页所需的css和script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadHeaderForShare(Page page,Literal ltr)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Theme.xml"));
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("Id").Value == "1"
                    select r;
            if (q != null)
            {
                string themeAppend = string.Empty;
                foreach (var item in q)
                {
                    themeAppend += item.Value.Replace("~/", GetVirtualUrlPrefix(page));
                }

                ltr.Text = themeAppend;
            }
        }

        /// <summary>
        /// 动态加载产品页所需的css和script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadHeaderForProduct(Page page, Literal ltr)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Theme.xml"));
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("Id").Value == "2"
                    select r;
            if (q != null)
            {
                string themeAppend = string.Empty;
                foreach (var item in q)
                {
                    themeAppend += item.Value.Replace("~/", GetVirtualUrlPrefix(page));
                }

                ltr.Text = themeAppend;
            }
        }

        /// <summary>
        /// 动态加载Admin页所需的css和script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadHeaderForAdmin(Page page, Literal ltr)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Theme.xml"));
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("Id").Value == "3"
                    select r;
            if (q != null)
            {
                string themeAppend = string.Empty;
                foreach (var item in q)
                {
                    themeAppend += item.Value.Replace("~/", GetVirtualUrlPrefix(page));
                }

                ltr.Text = themeAppend;
            }
        }

        /// <summary>
        /// 动态加载Users页所需的css和script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadHeaderForUsers(Page page, Literal ltr)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Theme.xml"));
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("Id").Value == "5"
                    select r;
            if (q != null)
            {
                string themeAppend = string.Empty;
                foreach (var item in q)
                {
                    themeAppend += item.Value.Replace("~/", GetVirtualUrlPrefix(page));
                }

                ltr.Text = themeAppend;
            }
        }

        /// <summary>
        /// 动态加载Jeasyui页所需的css和script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadHeaderForJeasyui(Page page, Literal ltr)
        {
            if (root == null) root = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Theme.xml"));
            var q = from r in root.Descendants("Add")
                    where r.Parent.Attribute("Id").Value == "4"
                    select r;
            if (q != null)
            {
                string themeAppend = string.Empty;
                foreach (var item in q)
                {
                    themeAppend += item.Value.Replace("~/", GetVirtualUrlPrefix(page));
                }

                ltr.Text = themeAppend;
            }
        }
    }
}
