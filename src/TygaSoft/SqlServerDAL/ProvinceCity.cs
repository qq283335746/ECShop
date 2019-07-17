using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public class ProvinceCity : IProvinceCity
    {
        #region 成员方法

        /// <summary>
        /// 获取省市区列表
        /// </summary>
        /// <returns></returns>
        public List<Model.ProvinceCityInfo> GetList()
        {
            List<Model.ProvinceCityInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProvinceCity"))
            {
                if (reader.HasRows)
                {
                    list = new List<Model.ProvinceCityInfo>();
                    while (reader.Read())
                    {
                        Model.ProvinceCityInfo model = new Model.ProvinceCityInfo();
                        model.NumberID = reader.GetString(0);
                        model.RegionName = reader.GetString(1);
                        model.RegionCode = reader.GetString(2);
                        model.ParentID = reader.GetString(3);
                        model.RegionLevel = reader.GetInt32(4);
                        model.RegionOrder = reader.GetInt32(5);
                        model.Ename = reader.GetString(6);
                        model.ShortEname = reader.GetString(7);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取当前父级ID下的所有子项键值对
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentId(string parentId)
        {
            Dictionary<string, string> dic = null;
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.VarChar, 50);
            parm.Value = parentId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProvinceCityKeyValueByParentId", parm))
            {
                if (reader != null && reader.HasRows)
                {
                    dic = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        if (!dic.ContainsKey(reader.GetString(0)))
                        {
                            dic.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 获取当前父级名称下的所有子项键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentName(string parentName)
        {
            Dictionary<string, string> dic = null;
            SqlParameter parm = new SqlParameter("@ParentName", SqlDbType.VarChar, 50);
            parm.Value = parentName;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProvinceCityKeyValueByParentName", parm))
            {
                if (reader != null && reader.HasRows)
                {
                    dic = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        if (!dic.ContainsKey(reader.GetString(0)))
                        {
                            dic.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }

            return dic;
        }

        #endregion
    }
}
