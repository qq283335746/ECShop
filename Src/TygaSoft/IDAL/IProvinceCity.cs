using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TygaSoft.IDAL
{
    public interface IProvinceCity
    {
        #region 成员方法

        /// <summary>
        /// 获取省市区列表
        /// </summary>
        /// <returns></returns>
        List<Model.ProvinceCityInfo> GetList();

        /// <summary>
        /// 获取当前父级ID下的所有子项键值对
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValueByParentId(string parentId);

        /// <summary>
        /// 获取当前父级名称下的所有子项键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValueByParentName(string parentName);

        #endregion
    }
}
