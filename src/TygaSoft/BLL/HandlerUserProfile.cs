using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.DALFactory;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public class HandlerUserProfile
    {
        private static readonly IHandlerUserProfile dal = XmlDataAccess.CreateHandlerUserProfile();

        /// <summary>
        /// 保存当前请求，有则更新，无则新增
        /// </summary>
        /// <param name="href"></param>
        /// <param name="url"></param>
        /// <param name="userFile"></param>
        public void RequestSave(string href, string url, string userFile)
        {
            dal.RequestSave(href,url, userFile);
        }

        public List<HandlerUserProfileInfo> GetList(string userFile)
        {
            return dal.GetList(userFile);
        }

        /// <summary>
        /// 删除一行数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userFile"></param>
        public void Delete(string url, string userFile)
        {
            dal.Delete(url, userFile);
        }

        /// <summary>
        /// 记录当前面板的关闭或打开状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="userFile"></param>
        public void InsertForLayout(string name, string status, string userFile)
        {
            dal.InsertForLayout(name, status, userFile);
        }

        /// <summary>
        /// 获取当前面板的关闭或打开状态记录
        /// </summary>
        /// <param name="userFile"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetListForLayout(string userFile)
        {
            return dal.GetListForLayout(userFile);
        }

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="userFile"></param>
        public void Create(string userFile)
        {
            dal.Create(userFile);
        }

        /// <summary>
        /// 新建xml文件
        /// </summary>
        /// <param name="userFile"></param>
        /// <param name="initPage"></param>
        public void Create(string userFile, string initPage)
        {
            dal.Create(userFile,initPage);
        }
    }
}
