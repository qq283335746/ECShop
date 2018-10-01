using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TygaSoft.IDAL
{
    public interface IProduct
    {
        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(Model.ProductInfo model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(Model.ProductInfo model);

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        int Delete(string numberId);

        /// <summary>
        /// 批量删除数据（启用事务
        /// 注意：调用此方法时请在包含事务的方法中执行
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteBatch(IList<string> list);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        Model.ProductInfo GetModel(string numberId);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        IList<Model.ProductInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet GetDataSet(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取产品基本信息数据集
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="orderBy"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet GetListProduct(int pageIndex, int pageSize, out int totalCount, string orderBy, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取当前产品ID集合下的所有图片集
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        IList<Model.ProductInfo> GetImagesListInProductIds(string productIds);

        /// <summary>
        /// 获取当前分类下的所有商品信息集
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        List<Model.ProductInfo> GetProductsByCategory(string category);

        /// <summary>
        /// 获取当前关键字搜索商品信息集
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        List<Model.ProductInfo> GetProductsBySearch(string[] keywords);

        /// <summary>
        /// 获取商品数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        List<Model.ProductInfo> GetProducts(int pageIndex, int pageSize, out int totalCount, string sqlWhere, string orderBy, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取当前分类集下的所有商品
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        List<Model.ProductInfo> GetProductsInCategories(string categories);

        #endregion
    }
}