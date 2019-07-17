using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.CustomThread;

namespace TygaSoft.BLL
{
    public class SearchKeyword : BaseThread
    {
        private string searchName;
        private int dataCount;

        public SearchKeyword() { }

        public SearchKeyword(string searchName, int dataCount) 
        {
            this.searchName = searchName;
            this.dataCount = dataCount;
        }

        private static readonly ISearchKeyword dal = DALFactory.DataAccess.CreateSearchKeyword();

        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.SearchKeywordInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.SearchKeywordInfo model)
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
        public Model.SearchKeywordInfo GetModel(string numberId)
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
        public IList<Model.SearchKeywordInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

        /// <summary>
        /// 创建搜索关键字
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="dataCount"></param>
        public int CreateKeyword(string searchName, int dataCount)
        {
            return dal.CreateKeyword(searchName, dataCount);
        }

        /// <summary>
        /// 创建搜索关键字
        /// </summary>
        public override void ThreadWork()
        {
            base.IsBackground = true;
            dal.CreateKeyword(searchName, dataCount);
        }

        /// <summary>
        /// 获取搜索关键字列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeywords()
        {
            return dal.GetKeywords();
        }

        #endregion
    }
}