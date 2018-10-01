using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.IDAL;

namespace TygaSoft.BLL
{
    public class WebSitemap
    {
        private static readonly IWebSitemap dal = DALFactory.XmlDataAccess.CreateWebSitemap();

        /// <summary>
        /// 获取站点地图数据集
        /// </summary>
        /// <returns></returns>
        public List<Model.WebSitemapInfo> GetList()
        {
            return dal.GetList();
        }
    }
}
