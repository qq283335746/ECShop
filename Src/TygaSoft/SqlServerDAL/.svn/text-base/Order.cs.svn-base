using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.DBUtility;
using TygaSoft.Model;

namespace TygaSoft.SqlServerDAL
{
    public class Order : IOrder
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(OrderInfo model)
        {
            if (model == null) return -1;

            string cmdText = "insert into [Order] (OrderNum,UserId,Receiver,Address,Mobilephone,Telephone,Email,Products,TotalCount,TotalPrice,PayOption,LastUpdatedDate,ProviceCity,CreateDate) values (@OrderNum,@UserId,@Receiver,@Address,@Mobilephone,@Telephone,@Email,@Products,@TotalCount,@TotalPrice,@PayOption,@LastUpdatedDate,@ProviceCity,@CreateDate)";
            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@OrderNum",SqlDbType.VarChar,30), 
                                     new SqlParameter("@UserId",SqlDbType.VarChar,40), 
                                     new SqlParameter("@Receiver",SqlDbType.NVarChar,50), 
                                     new SqlParameter("@Address",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@Mobilephone",SqlDbType.VarChar,20), 
                                     new SqlParameter("@Telephone",SqlDbType.VarChar,20), 
                                     new SqlParameter("@Email",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@Products",SqlDbType.NVarChar,4000), 
                                     new SqlParameter("@TotalCount",SqlDbType.Int), 
                                     new SqlParameter("@TotalPrice",SqlDbType.Decimal), 
                                     new SqlParameter("@PayOption",SqlDbType.NVarChar,20), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime),
                                     new SqlParameter("@ProviceCity",SqlDbType.NVarChar,30),
                                     new SqlParameter("@CreateDate",SqlDbType.DateTime) 
                                   };
            parms[0].Value = model.OrderNum;
            parms[1].Value = model.UserId;
            parms[2].Value = model.Receiver;
            parms[3].Value = model.Address;
            parms[4].Value = model.Mobilephone;
            parms[5].Value = model.Telephone;
            parms[6].Value = model.Email;
            parms[7].Value = model.Products;
            parms[8].Value = model.TotalCount;
            parms[9].Value = model.TotalPrice;
            parms[10].Value = model.PayOption;
            parms[11].Value = model.LastUpdatedDate;
            parms[12].Value = model.ProviceCity;
            parms[13].Value = model.CreateDate;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(OrderInfo model)
        {
            if (model == null) return -1;

            //定义查询命令
            string cmdText = @"update [Order] set Receiver = @Receiver,Address = @Address,Mobilephone = @Mobilephone,Telephone = @Telephone,Email = @Email,Products = @Products,TotalCount = @TotalCount,TotalPrice = @TotalPrice,PayPrice = @PayPrice,PayOption = @PayOption,PayStatus = @PayStatus,Status = @Status,PayDate = @PayDate,LastUpdatedDate = @LastUpdatedDate,ProviceCity = @ProviceCity where OrderId = @OrderId";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@OrderId",SqlDbType.VarChar,40), 
                                     new SqlParameter("@Receiver",SqlDbType.NVarChar,50), 
                                     new SqlParameter("@Address",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@Mobilephone",SqlDbType.VarChar,20), 
                                     new SqlParameter("@Telephone",SqlDbType.VarChar,20), 
                                     new SqlParameter("@Email",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@Products",SqlDbType.NVarChar,4000), 
                                     new SqlParameter("@TotalCount",SqlDbType.Int), 
                                     new SqlParameter("@TotalPrice",SqlDbType.Decimal), 
                                     new SqlParameter("@PayPrice",SqlDbType.Decimal), 
                                     new SqlParameter("@PayOption",SqlDbType.NVarChar,20), 
                                     new SqlParameter("@PayStatus",SqlDbType.TinyInt), 
                                     new SqlParameter("@Status",SqlDbType.TinyInt), 
                                     new SqlParameter("@PayDate",SqlDbType.DateTime), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime),
                                     new SqlParameter("@ProviceCity",SqlDbType.NVarChar,30)
                                   };
            parms[0].Value = model.OrderId;
            parms[1].Value = model.Receiver;
            parms[2].Value = model.Address;
            parms[3].Value = model.Mobilephone;
            parms[4].Value = model.Telephone;
            parms[5].Value = model.Email;
            parms[6].Value = model.Products;
            parms[7].Value = model.TotalCount;
            parms[8].Value = model.TotalPrice;
            parms[9].Value = model.PayPrice;
            parms[10].Value = model.PayOption;
            parms[11].Value = model.PayStatus;
            parms[12].Value = model.Status;
            parms[13].Value = model.PayDate;
            parms[14].Value = model.LastUpdatedDate;
            parms[15].Value = model.ProviceCity;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(object numberId)
        {
            if (numberId != null)
            {
                if (!(numberId is Guid))
                {
                    return -1;
                }
            }
            string cmdText = "delete from [Order] where OrderId = @OrderId";
            SqlParameter parm = new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier);
            parm.Value = numberId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm);
        }

        /// <summary>
        /// 批量删除数据（启用事务）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteBatch(IList<string> list)
        {
            if (list == null || list.Count == 0) return false;

            bool result = false;
            StringBuilder sb = new StringBuilder();
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from [Order] where OrderId = @OrderId" + n + " ;");
                SqlParameter parm = new SqlParameter("@OrderId" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
                        tran.Commit();
                        if (effect > 0) result = true;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public OrderInfo GetModel(object numberId)
        {
            if (numberId != null)
            {
                if (!(numberId is Guid))
                {
                    return null;
                }
            }
            OrderInfo model = null;

            string cmdText = @"select top 1 OrderId,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,Email,Products,TotalCount,
                             TotalPrice,PayPrice,PayOption,PayStatus,Status,PayDate,LastUpdatedDate,CreateDate from [Order] where OrderId = @OrderId order by CreateDate desc ";
            SqlParameter parm = new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier);
            parm.Value = numberId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.OrderInfo();
                        model.OrderId = reader.GetGuid(0);
                        model.OrderNum = reader.GetString(1);
                        model.UserId = reader.GetGuid(2);
                        model.Receiver = reader.GetString(3);
                        model.ProviceCity = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.Mobilephone = reader.GetString(6);
                        model.Telephone = reader.GetString(7);
                        model.Email = reader.GetString(8);
                        model.Products = reader.GetString(9);
                        model.TotalCount = reader.GetInt32(10);
                        model.TotalPrice = reader.GetDecimal(11);
                        model.PayPrice = reader.GetDecimal(12);
                        model.PayOption = reader.GetString(13);
                        model.PayStatus = reader.GetByte(14);
                        model.Status = reader.GetByte(15);
                        model.PayDate = reader.GetDateTime(16);
                        model.LastUpdatedDate = reader.GetDateTime(17);
                        model.CreateDate = reader.GetDateTime(18);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrderInfo GetModel(string orderNum, object userId)
        {
            OrderInfo model = null;

            string cmdText = @"select top 1 OrderId,OrderNum,UserId,Receiver,ProviceCity,Address,Mobilephone,Telephone,Email,Products,
                             TotalCount,TotalPrice,PayPrice,PayOption,PayStatus,Status,PayDate,LastUpdatedDate,CreateDate,
                             (case PayStatus when 1 then '已支付' else '未支付' end) as PayStatusName,
                             (case Status when 0 then '未处理' when 1 then '正在出库' when 10 then '已完成' when 11 then '已取消' else '未知' end) as StatusName
                             from [Order] where OrderNum = @OrderNum and UserId = @UserId order by LastUpdatedDate desc ";
            SqlParameter[] parms = {
                                       new SqlParameter("OrderNum",SqlDbType.VarChar,30),
                                       new SqlParameter("UserId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = orderNum;
            parms[1].Value = Guid.Parse(userId.ToString());

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new OrderInfo();
                        model.OrderId = reader.GetGuid(0);
                        model.OrderNum = reader.GetString(1);
                        model.UserId = reader.GetGuid(2);
                        model.Receiver = reader.GetString(3);
                        model.ProviceCity = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.Mobilephone = reader.GetString(6);
                        model.Telephone = reader.GetString(7);
                        model.Email = reader.GetString(8);
                        model.Products = reader.GetString(9);
                        model.TotalCount = reader.GetInt32(10);
                        model.TotalPrice = reader.GetDecimal(11);
                        model.PayPrice = reader.GetDecimal(12);
                        model.PayOption = reader.GetString(13);
                        model.PayStatus = reader.GetByte(14);
                        model.Status = reader.GetByte(15);
                        model.PayDate = reader.GetDateTime(16);
                        model.LastUpdatedDate = reader.GetDateTime(17);
                        model.CreateDate = reader.GetDateTime(18);
                        model.PayStatusName = reader.GetString(19);
                        model.StatusName = reader.GetString(20);
                    }
                }
            }

            return model;
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
            //获取数据集总数
            string cmdText = "select count(*) from [Order] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.CreateDate desc) as RowNumber,t1.OrderId,t1.OrderNum,t1.UserId,t1.Receiver,t1.Address,t1.Mobilephone,t1.Telephone,t1.Email,t1.Products,t1.TotalCount,t1.TotalPrice,t1.PayPrice,t1.PayOption,t1.PayStatus,t1.Status,t1.PayDate,t1.LastUpdatedDate from [Order] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<OrderInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<OrderInfo>();

                    while (reader.Read())
                    {
                        OrderInfo model = new OrderInfo();
                        model.OrderId = reader.GetGuid(1);
                        model.OrderNum = reader.GetString(2);
                        model.UserId = reader.GetGuid(3);
                        model.Receiver = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.Mobilephone = reader.GetString(6);
                        model.Telephone = reader.GetString(7);
                        model.Email = reader.GetString(8);
                        model.Products = reader.GetString(9);
                        model.TotalCount = reader.GetInt32(10);
                        model.TotalPrice = reader.GetDecimal(11);
                        model.PayPrice = reader.GetDecimal(12);
                        model.PayOption = reader.GetString(3);
                        model.PayStatus = reader.GetByte(14);
                        model.Status = reader.GetByte(15);
                        model.PayDate = reader.GetDateTime(16);
                        model.LastUpdatedDate = reader.GetDateTime(17);

                        list.Add(model);
                    }
                }
            }

            return list;
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
            //获取数据集总数
            string cmdText = "select count(*) from [Order] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.OrderId,t1.OrderNum,t1.UserId,
                      t1.Receiver,t1.ProviceCity,t1.Address,t1.Mobilephone,t1.Telephone,t1.Email,t1.Products,t1.TotalCount,
                      t1.TotalPrice,t1.PayPrice,t1.PayOption,
                      (case t1.PayStatus when 1 then '已支付' else '未支付' end) as PayStatus,
                      (case t1.Status when 0 then '未处理' when 1 then '正在出库' when 10 then '已完成' when 11 then '已取消' else '未知' end) as Status,
                      t1.PayDate,t1.LastUpdatedDate,t1.CreateDate from [Order] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
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
            string cmdText = @"update OrderInfo set PayPrice = @PayPrice,PayStatus = 1,Status = 1,
                             PayDate = @PayDate,LastUpdatedDate = @LastUpdatedDate
                             where OrderId = @OrderId and UserId = @UserId";

            DateTime dtime = DateTime.Now;
            SqlParameter[] parms = {
                                       new SqlParameter("@PayPrice",SqlDbType.Decimal),
                                       new SqlParameter("@PayDate",dtime),
                                       new SqlParameter("@LastUpdatedDate",dtime),
                                       new SqlParameter("@OrderId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = payPrice;
            parms[3].Value = orderId;
            parms[4].Value = userId;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public List<ProductInfo> GetProductInIds(string nIdAppend)
        {
            List<ProductInfo> list = null;
            string cmdText = "Pro_GetProductInIds";
            SqlParameter parm = new SqlParameter("@ProductIds", SqlDbType.VarChar, 4000);
            parm.Value = nIdAppend;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, cmdText, parm))
            {
                if (reader != null)
                {
                    list = new List<ProductInfo>();
                    while (reader.Read())
                    {
                        ProductInfo model = new ProductInfo();
                        model.ProductName = reader.GetString(0);
                        model.Subtitle = reader.GetString(1);
                        model.PNum = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }
    }
}
