using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.DALFactory;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public class Order
    {
        private static readonly IOrder dal = DataAccess.CreateOrder();

        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(OrderInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(OrderInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(object numberId)
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
        public OrderInfo GetModel(object numberId)
        {
            return dal.GetModel(numberId);
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfo GetModel(string orderNum, object userId)
        {
            return dal.GetModel(orderNum, userId);
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
        public DataSet GetDsForOrderInfo(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetDsForOrderInfo(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
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
        public IList<OrderInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="payPrice">付款金额</param>
        /// <returns></returns>
        public int PayPrice(object userId, object orderId, decimal payPrice)
        {
            return dal.PayPrice(userId, orderId, payPrice);
        }

        /// <summary>
        /// 获取当前ID集合对应的数据集
        /// </summary>
        /// <param name="nIdAppend"></param>
        /// <returns></returns>
        public List<Model.ProductInfo> GetProductInIds(string nIdAppend)
        {
            return dal.GetProductInIds(nIdAppend);
        }

        #endregion
    }
}