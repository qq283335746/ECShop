using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TygaSoft.IDAL
{
    public interface ICategory
    {
        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(Model.CategoryInfo model);

        /// <summary>
        /// 更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(Model.CategoryInfo model);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int Delete(int Id);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        Model.CategoryInfo GetModel(string numberId);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet GetList(string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取属于当前标题的所有分类
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        List<Model.CategoryInfo> GetCategoryByTitle(string title);

        /// <summary>
        /// 批量删除数据（启用事务）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteBatch(IList<string> list);

        /// <summary>
        /// 获取满足当前条件下的所有分类键值对
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValue(string sqlWhere);

        /// <summary>
        /// 获取当前父级名称下的所有分类键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValueByParentName(string parentName);

        /// <summary>
        /// 获取当前父级ID下的所有分类键值对
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValueByParentId(string parentId);

        /// <summary>
        /// 获取当前父级ID下的所有子级分类
        /// </summary>
        /// <param name="parentIdsAppend"></param>
        /// <returns></returns>
        IList<Model.CategoryInfo> GetListInParentIds(string parentIdsAppend);

        /// <summary>
        /// 获取当前父级名称（包括该父级节点）下的所有子分类键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetKeyValueOnParentName(string parentName);

        /// <summary>
        /// 获取所有分类数据集
        /// </summary>
        /// <returns></returns>
        List<Model.CategoryInfo> GetList();

        #endregion
    }
}
