using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;

namespace TygaSoft.BLL
{
    public class Product
    {
        private static readonly IProduct dal = DALFactory.DataAccess.CreateProduct();

        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.ProductInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.ProductInfo model)
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
        /// 注意：调用此方法时请在包含事务的方法中执行
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
        public Model.ProductInfo GetModel(string numberId)
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
        public IList<Model.ProductInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

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
        public DataSet GetListProduct(int pageIndex, int pageSize, out int totalCount, string orderBy, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetListProduct(pageIndex, pageSize, out totalCount, orderBy, sqlWhere, commandParameters);
        }

        /// <summary>
        /// 获取当前产品ID集合下的所有图片集
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public IList<Model.ProductInfo> GetImagesListInProductIds(string productIds)
        {
            return dal.GetImagesListInProductIds(productIds);
        }

        /// <summary>
        /// 获取当前分类下的所有商品信息集
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Model.ProductInfo> GetProductsByCategory(string category)
        {
            return dal.GetProductsByCategory(category);
        }

        /// <summary>
        /// 获取当前关键字搜索商品信息集
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<Model.ProductInfo> GetProductsBySearch(string text)
        {
            if (string.IsNullOrEmpty(text.Trim()))
                return new List<Model.ProductInfo>();

            string[] keywords = text.Split();

            return dal.GetProductsBySearch(keywords);
        }

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
        public List<Model.ProductInfo> GetProducts(int pageIndex, int pageSize, out int totalCount, string sqlWhere, string orderBy, params SqlParameter[] commandParameters)
        {
            return dal.GetProducts(pageIndex, pageSize, out totalCount, sqlWhere, orderBy, commandParameters);
        }

        /// <summary>
        /// 获取当前分类集下的所有商品
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public List<Model.ProductInfo> GetProductsInCategories(string categories)
        {
            return dal.GetProductsInCategories(categories);
        }

        #endregion
    }
}