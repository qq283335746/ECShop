using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.DALFactory;
using TygaSoft.IDAL;

namespace TygaSoft.BLL
{
    public class ProvinceCity
    {
        private static readonly IProvinceCity dal = DataAccess.CreateProvinceCity();

        #region 成员方法

        /// <summary>
        /// 获取省市区列表
        /// </summary>
        /// <returns></returns>
        public List<Model.ProvinceCityInfo> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 获取当前父级ID下的所有子项键值对
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentId(string parentId)
        {
            return dal.GetKeyValueByParentId(parentId);
        }

        /// <summary>
        /// 获取当前父级名称下的所有子项键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentName(string parentName)
        {
            return dal.GetKeyValueByParentName(parentName);
        }

        #endregion
    }
}
