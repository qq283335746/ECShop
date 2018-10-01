using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public class SystemProfile
    {
        private static readonly ISystemProfile dal = DataAccess.CreateSystemProfile();

        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.SystemProfileInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.SystemProfileInfo model)
        {
            return dal.Update(model);
        }

		/// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(string numberId)
        {
            return dal.Delete(numberId);
        }

        /// <summary>
        /// 批量删除数据（启用事务）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteBatch(IList<string> list)
        {
            return dal.DeleteBatch(list);
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public Model.SystemProfileInfo GetModel(string numberId)
        {
            return dal.GetModel(numberId);
        }

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetDataSet(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

		/// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public IList<Model.SystemProfileInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

        /// <summary>
        /// 获取当前标题对应的数据
        /// </summary>
        /// <param name="titleAppend"></param>
        /// <returns></returns>
        public List<Model.SystemProfileInfo> GetModelInTitle(string titleAppend)
        {
            return dal.GetModelInTitle(titleAppend);
        }

        #endregion
    }
}