using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.IDAL
{
    public interface IHandlerUserProfile
    {
        #region 成员方法

        /// <summary>
        /// 保存当前请求，有则更新，无则新增
        /// </summary>
        /// <param name="href"></param>
        /// <param name="url"></param>
        /// <param name="userFile"></param>
        void RequestSave(string href,string url, string userFile);

        /// <summary>
        /// 获取下拉列表数据集
        /// </summary>
        /// <param name="userFile"></param>
        /// <returns></returns>
        List<Model.HandlerUserProfileInfo> GetList(string userFile);

        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userFile"></param>
        void Delete(string url, string userFile);

        /// <summary>
        /// 记录当前面板的关闭或打开状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="userFile"></param>
        void InsertForLayout(string name, string status, string userFile);

        /// <summary>
        /// 获取当前面板的关闭或打开状态记录
        /// </summary>
        /// <param name="userFile"></param>
        /// <returns></returns>
        Dictionary<string, string> GetListForLayout(string userFile);

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="userFile"></param>
        void Create(string userFile);

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="userFile"></param>
        /// <param name="initPage"></param>
        void Create(string userFile, string initPage);

        #endregion
    }
}
