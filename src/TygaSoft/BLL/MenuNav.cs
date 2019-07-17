using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.DALFactory;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public class MenuNav
    {
        private static readonly IMenuNav dal = XmlDataAccess.CreateMenuNav();

        #region 成员方法

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="file"></param>
        public void Create(string file)
        {
            dal.Create(file);
        }

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="initPage"></param>
        public void Create(string file, string initPage)
        {
            dal.Create(file, initPage);
        }

        /// <summary>
        /// 保存当前请求，有则更新，无则新增
        /// </summary>
        /// <param name="href"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        public void RequestSave(string href, string url, string file)
        {
            dal.RequestSave(href, url, file);
        }

        /// <summary>
        /// 获取下拉列表数据集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<MenuNavInfo> GetList(string file)
        {
            return dal.GetList(file);
        }

        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        public void Delete(string url, string file)
        {
            dal.Delete(url, file);
        }

        /// <summary>
        /// 记录当前面板的关闭或打开状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="file"></param>
        public void InsertForLayout(string name, string status, string file)
        {
            dal.InsertForLayout(name, status, file);
        }

        /// <summary>
        /// 获取当前布局的操作状态
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetLayoutStatus(string file, string name)
        {
            return dal.GetLayoutStatus(file, name);
        }

        /// <summary>
        /// 获取当前面板的关闭或打开状态记录
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetListForLayout(string file)
        {
            return dal.GetListForLayout(file);
        }

        /// <summary>
        /// 获取自定义菜单属性
        /// </summary>
        /// <returns></returns>
        public List<MenuItemInfo> GetMenus()
        {
            return dal.GetMenus();
        }

        #endregion
    }
}
