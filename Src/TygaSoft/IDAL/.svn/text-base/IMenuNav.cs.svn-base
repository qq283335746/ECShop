using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IDAL
{
    public interface IMenuNav
    {
        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="file"></param>
        void Create(string file);

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="initPage"></param>
        void Create(string file, string initPage);

        /// <summary>
        /// 保存当前请求，有则更新，无则新增
        /// </summary>
        /// <param name="href"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        void RequestSave(string href, string url, string file);

        /// <summary>
        /// 获取下拉列表数据集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        List<Model.MenuNavInfo> GetList(string file);

        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        void Delete(string url, string file);

        /// <summary>
        /// 记录当前面板的关闭或打开状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="file"></param>
        void InsertForLayout(string name, string status, string file);

        /// <summary>
        /// 获取当前布局的操作状态
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetLayoutStatus(string file, string name);

        /// <summary>
        /// 获取当前面板的关闭或打开状态记录
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Dictionary<string, string> GetListForLayout(string file);

        /// <summary>
        /// 获取自定义菜单属性
        /// </summary>
        /// <returns></returns>
        List<Model.MenuItemInfo> GetMenus();
    }
}
