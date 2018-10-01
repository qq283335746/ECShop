using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public interface IOrder
    {
        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(OrderInfo model);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(OrderInfo model);

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        int Delete(object numberId);

        /// <summary>
        /// 批量删除数据（启用事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteBatch(IList<string> list);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        OrderInfo GetModel(object numberId);

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OrderInfo GetModel(string orderNum, object userId);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        IList<OrderInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 获取数据分页列表，并返回所有记录数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        DataSet GetDsForOrderInfo(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters);

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="payPrice">付款金额</param>
        /// <returns></returns>
        int PayPrice(object userId, object orderId, decimal payPrice);

        /// <summary>
        /// 获取当前ID集合对应的数据集
        /// </summary>
        /// <param name="nIdAppend"></param>
        /// <returns></returns>
        List<ProductInfo> GetProductInIds(string nIdAppend);

        #endregion
    }
}