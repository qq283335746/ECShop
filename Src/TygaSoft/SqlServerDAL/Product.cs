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
    public class Product : IProduct
    {
        private const string Sql_Select_Products_By_Search1 = "SELECT p.ProductId, ProductName,Subtitle,ProductPrice,ImagesUrl,p.CategoryId FROM Product p inner join Category c on p.CategoryId = c.NumberID WHERE ((";
        private const string Sql_Select_Products_By_Search2 = "LOWER(ProductName) LIKE '%' + {0} + '%' OR LOWER(c.CategoryName) LIKE '%' + {0} + '%'";
        private const string Sql_Select_Products_By_Search3 = ") OR (";
        private const string Sql_Select_Products_By_Search4 = "))";
        private const string Parm_Keyword = "@Keyword";

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.ProductInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.ProductName, model.CategoryId, "")) return 110;

            string productId = NumberID.GetNumberID();

            string cmdText1 = "insert into Product (ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl,CreateDate,UserId) values (@ProductId,@CategoryId,@ProductName,@Subtitle,@ProductPrice,@ImagesUrl,@CreateDate,@UserId)";
            SqlParameter[] parms1 = {
                                         new SqlParameter("@ProductId",SqlDbType.VarChar,40), 
                                         new SqlParameter("@CategoryId",SqlDbType.VarChar,40), 
                                         new SqlParameter("@ProductName",SqlDbType.NVarChar,256), 
                                         new SqlParameter("@Subtitle",SqlDbType.NVarChar,256), 
                                         new SqlParameter("@ProductPrice",SqlDbType.Decimal), 
                                         new SqlParameter("@ImagesUrl",SqlDbType.NVarChar,300), 
                                         new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                         new SqlParameter("@UserId",SqlDbType.UniqueIdentifier)
                                    };
            parms1[0].Value = productId;
            parms1[1].Value = model.CategoryId;
            parms1[2].Value = model.ProductName;
            parms1[3].Value = model.Subtitle;
            parms1[4].Value = model.ProductPrice;
            parms1[5].Value = model.ImagesUrl;
            parms1[6].Value = model.CreateDate;
            parms1[7].Value = Guid.Parse(model.UserId.ToString());

            string cmdText2 = "insert into ProductItem (ProductId,PNum,StockNum,ImagesAppend,MainImage,LImagesUrl,MImagesUrl,SImagesUrl,MarketPrice,PayOptions,ViewCount,CreateDate) values (@ProductId,@PNum,@StockNum,@ImagesAppend,@MainImage,@LImagesUrl,@MImagesUrl,@SImagesUrl,@MarketPrice,@PayOptions,@ViewCount,@CreateDate)";
            SqlParameter[] parms2 = {
                                                         new SqlParameter("@ProductId",SqlDbType.VarChar,40),
                                                         new SqlParameter("@PNum",SqlDbType.VarChar,30), 
                                                         new SqlParameter("@StockNum",SqlDbType.Int), 
                                                         new SqlParameter("@LImagesUrl",SqlDbType.NVarChar), 
                                                         new SqlParameter("@MImagesUrl",SqlDbType.NVarChar), 
                                                         new SqlParameter("@SImagesUrl",SqlDbType.NVarChar), 
                                                         new SqlParameter("@MarketPrice",SqlDbType.Decimal), 
                                                         new SqlParameter("@PayOptions",SqlDbType.NVarChar,300), 
                                                         new SqlParameter("@ViewCount",SqlDbType.Int),
                                                         new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                                         new SqlParameter("@ImagesAppend",SqlDbType.NVarChar), 
                                                         new SqlParameter("@MainImage",SqlDbType.NVarChar), 
                                                    };
            parms2[0].Value = productId;
            parms2[1].Value = model.PNum;
            parms2[2].Value = model.StockNum;
            parms2[3].Value = model.LImagesUrl;
            parms2[4].Value = model.MImagesUrl;
            parms2[5].Value = model.SImagesUrl;
            parms2[6].Value = model.MarketPrice;
            parms2[7].Value = model.PayOptions;
            parms2[8].Value = model.ViewCount;
            parms2[9].Value = model.CreateDate;
            parms2[10].Value = model.ImagesAppend;
            parms2[11].Value = model.MainImage;

            string cmdText3 = "insert into ProductAttr (ProductId,CustomAttrs,Descr,CreateDate) values (@ProductId,@CustomAttrs,@Descr,@CreateDate)";
            SqlParameter[] parms3 = {
                                                        new SqlParameter("@ProductId",SqlDbType.VarChar,40),
                                                        new SqlParameter("@CustomAttrs",SqlDbType.NVarChar), 
                                                        new SqlParameter("@Descr",SqlDbType.NText),
                                                        new SqlParameter("@CreateDate",SqlDbType.DateTime)
                                                    };
            parms3[0].Value = productId;
            parms3[1].Value = model.CustomAttrs;
            parms3[2].Value = model.Descr;
            parms3[3].Value = model.CreateDate;

            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, cmdText1, parms1);
                        SqlHelper.ExecuteScalar(tran, CommandType.Text, cmdText2, parms2);
                        SqlHelper.ExecuteScalar(tran, CommandType.Text, cmdText3, parms3);

                        tran.Commit();
                        return 1;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.ProductInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.ProductName, model.CategoryId, model.ProductId)) return 110;

            string cmdText = "update Product set CategoryId = @CategoryId,ProductName = @ProductName,Subtitle = @Subtitle,ProductPrice = @ProductPrice,ImagesUrl = @ImagesUrl,CreateDate = @CreateDate where ProductId = @ProductId;";
            cmdText += "update ProductItem set PNum = @PNum,StockNum = @StockNum,ImagesAppend = @ImagesAppend,MainImage = @MainImage,LImagesUrl = @LImagesUrl,MImagesUrl = @MImagesUrl,SImagesUrl = @SImagesUrl,MarketPrice = @MarketPrice,PayOptions = @PayOptions,ViewCount = @ViewCount,CreateDate = @CreateDate where ProductId = @ProductId;";
            cmdText += "update ProductAttr set CustomAttrs = @CustomAttrs,Descr = @Descr,CreateDate = @CreateDate where ProductId = @ProductId;";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@ProductId",SqlDbType.VarChar,40), 
                                     new SqlParameter("@CategoryId",SqlDbType.VarChar,40), 
                                     new SqlParameter("@ProductName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@Subtitle",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ProductPrice",SqlDbType.Decimal), 
                                     new SqlParameter("@ImagesUrl",SqlDbType.NVarChar,300), 
                                     new SqlParameter("@CreateDate",SqlDbType.DateTime), 
                                     new SqlParameter("@PNum",SqlDbType.VarChar,30), 
                                     new SqlParameter("@StockNum",SqlDbType.Int), 
                                     new SqlParameter("@LImagesUrl",SqlDbType.NVarChar), 
                                     new SqlParameter("@MImagesUrl",SqlDbType.NVarChar), 
                                     new SqlParameter("@SImagesUrl",SqlDbType.NVarChar), 
                                     new SqlParameter("@MarketPrice",SqlDbType.Decimal), 
                                     new SqlParameter("@PayOptions",SqlDbType.NVarChar,300), 
                                     new SqlParameter("@ViewCount",SqlDbType.Int), 
                                     new SqlParameter("@CustomAttrs",SqlDbType.NVarChar), 
                                     new SqlParameter("@Descr",SqlDbType.NText),
                                     new SqlParameter("@ImagesAppend",SqlDbType.NVarChar), 
                                     new SqlParameter("@MainImage",SqlDbType.NVarChar)
                                   };
            parms[0].Value = model.ProductId;
            parms[1].Value = model.CategoryId;
            parms[2].Value = model.ProductName;
            parms[3].Value = model.Subtitle;
            parms[4].Value = model.ProductPrice;
            parms[5].Value = model.ImagesUrl;
            parms[6].Value = model.CreateDate;
            parms[7].Value = model.PNum;
            parms[8].Value = model.StockNum;
            parms[9].Value = model.LImagesUrl;
            parms[10].Value = model.MImagesUrl;
            parms[11].Value = model.SImagesUrl;
            parms[12].Value = model.MarketPrice;
            parms[13].Value = model.PayOptions;
            parms[14].Value = model.ViewCount;
            parms[15].Value = model.CustomAttrs;
            parms[16].Value = model.Descr;
            parms[17].Value = model.ImagesAppend;
            parms[18].Value = model.MainImage;

            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderConnString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, cmdText, parms);
                        tran.Commit();
                        return 1;
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(string numberId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量删除数据（启用事务）
        /// 注意：调用此方法时请在包含事务的方法中执行
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteBatch(IList<string> list)
        {
            if (list == null || list.Count == 0) return false;

            StringBuilder sb = new StringBuilder();
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from [Product] where ProductId = @ProductId" + n + " ;");
                sb.Append(@"delete from [ProductItem] where ProductId = @ProductId" + n + " ;");
                sb.Append(@"delete from [ProductAttr] where ProductId = @ProductId" + n + " ;");
                SqlParameter parm = new SqlParameter("@ProductId" + n + "", SqlDbType.VarChar, 40);
                parm.Value = item;
                parms.Add(parm);
            }

            int effect = SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null);
            if (effect > 0) return true;

            return false;
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public Model.ProductInfo GetModel(string numberId)
        {
            Model.ProductInfo model = null;

            string cmdText = @"select top 1 ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl,CreateDate,UserId,PNum,StockNum,ImagesAppend,MainImage,LImagesUrl,MImagesUrl,SImagesUrl,MarketPrice,PayOptions,ViewCount,CustomAttrs,Descr from [View_Product] where ProductId = @ProductId order by CreateDate desc ";
            SqlParameter parm = new SqlParameter("@ProductId", SqlDbType.VarChar, 40);
            parm.Value = numberId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.ProductInfo();
                        model.ProductId = reader["ProductId"].ToString();
                        model.CategoryId = reader["CategoryId"].ToString();
                        model.ProductName = reader["ProductName"].ToString();
                        model.Subtitle = reader["Subtitle"].ToString();
                        model.ProductPrice = decimal.Parse(reader["ProductPrice"].ToString());
                        model.ImagesUrl = reader["ImagesUrl"].ToString();
                        model.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        model.UserId = reader["UserId"];
                        model.PNum = reader["PNum"].ToString();
                        model.StockNum = int.Parse(reader["StockNum"].ToString());
                        model.ImagesAppend = reader["ImagesAppend"].ToString();
                        model.MainImage = reader["MainImage"].ToString();
                        model.LImagesUrl = reader["LImagesUrl"].ToString();
                        model.MImagesUrl = reader["MImagesUrl"].ToString();
                        model.SImagesUrl = reader["SImagesUrl"].ToString();
                        model.MarketPrice = decimal.Parse(reader["MarketPrice"].ToString());
                        model.PayOptions = reader["PayOptions"].ToString();
                        model.ViewCount = int.Parse(reader["ViewCount"].ToString());
                        model.CustomAttrs = reader["CustomAttrs"].ToString();
                        model.Descr = reader["Descr"].ToString();

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
        public IList<Model.ProductInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [View_Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by CreateDate desc) as RowNumber,ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl,CreateDate,PNum,StockNum,LImagesUrl,MImagesUrl,SImagesUrl,MarketPrice,PayOptions,ViewCount,CustomAttrs,Descr from [View_Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<Model.ProductInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.ProductInfo>();

                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ProductId = reader["ProductId"].ToString();
                        model.CategoryId = reader["CategoryId"].ToString();
                        model.ProductName = reader["ProductName"].ToString();
                        model.Subtitle = reader["Subtitle"].ToString();
                        model.ProductPrice = decimal.Parse(reader["ProductPrice"].ToString());
                        model.ImagesUrl = reader["ImagesUrl"].ToString();
                        model.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                        model.PNum = reader["PNum"].ToString();
                        model.StockNum = int.Parse(reader["StockNum"].ToString());
                        model.LImagesUrl = reader["LImagesUrl"].ToString();
                        model.MImagesUrl = reader["MImagesUrl"].ToString();
                        model.SImagesUrl = reader["SImagesUrl"].ToString();
                        model.MarketPrice = decimal.Parse(reader["MarketPrice"].ToString());
                        model.PayOptions = reader["PayOptions"].ToString();
                        model.ViewCount = int.Parse(reader["ViewCount"].ToString());
                        model.CustomAttrs = reader["CustomAttrs"].ToString();
                        model.Descr = reader["Descr"].ToString();

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
        public DataSet GetDataSet(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [View_Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by CreateDate desc) as RowNumber,ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl,CreateDate,PNum,StockNum,LImagesUrl,MImagesUrl,SImagesUrl,MarketPrice,PayOptions,ViewCount,CustomAttrs,Descr,CategoryName from [View_Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 是否存在对应数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public bool IsExist(string productName,string categoryId, string numberId)
        {
            bool isExist = false;
            int totalCount = -1;

            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select count(*) from [Product] where ProductName = @ProductName and CategoryId = @CategoryId";
            if (!string.IsNullOrEmpty(numberId))
            {
                cmdText = "select count(*) from [Product] where ProductName = @ProductName and CategoryId = @CategoryId and ProductId <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.VarChar, 40);
                parm1.Value = numberId;
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 256);
            parm.Value = productName;
            parms.Add(parm);
            parm = new SqlParameter("@CategoryId", SqlDbType.VarChar, 40);
            parm.Value = categoryId;
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
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
            //获取数据集总数
            string cmdText = "select count(*) from [Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            string orderName = "CreateDate desc";
            if (!string.IsNullOrEmpty(orderBy)) orderName = orderBy;
            cmdText = @"select * from(select row_number() over(order by " + orderName + ") as RowNumber,ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl from [Product] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 获取当前产品ID集合下的所有图片集
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public IList<Model.ProductInfo> GetImagesListInProductIds(string productIds)
        {
            IList<Model.ProductInfo> list = null;
            SqlParameter parm = new SqlParameter("@ProductIds",productIds);

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProductImagesInIds", parm))
            {
                if (reader != null)
                {
                    list = new List<Model.ProductInfo>();
                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ImagesUrl = reader["ImagesUrl"].ToString();
                        model.LImagesUrl = reader["LImagesUrl"].ToString();
                        model.MImagesUrl = reader["MImagesUrl"].ToString();
                        model.SImagesUrl = reader["SImagesUrl"].ToString();
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<Model.ProductInfo> GetProductsByCategory(string category)
        {
            List<Model.ProductInfo> list = new List<Model.ProductInfo>();
            SqlParameter parm = new SqlParameter("@Category", SqlDbType.VarChar, 40);
            parm.Value = category;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProductsByCategory", parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ProductId = reader.GetString(0);
                        model.ProductName = reader.GetString(1);
                        model.Subtitle = reader.GetString(2);
                        model.ProductPrice = reader.GetDecimal(3);
                        model.ImagesUrl = reader.GetString(4);
                        model.PNum = reader.GetString(5);
                        model.StockNum = reader.GetInt32(6);
                        model.LImagesUrl = reader.GetString(7);
                        model.MImagesUrl = reader.GetString(8);
                        model.SImagesUrl = reader.GetString(9);
                        model.MarketPrice = reader.GetDecimal(10);
                        model.PayOptions = reader.GetString(11);
                        model.ViewCount = reader.GetInt32(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<Model.ProductInfo> GetProductsBySearch(string[] keywords)
        {
            List<Model.ProductInfo> list = new List<Model.ProductInfo>();
            int numKeywords = keywords.Length;

            StringBuilder sql = new StringBuilder(Sql_Select_Products_By_Search1);

            for (int i = 0; i < numKeywords; i++)
            {
                sql.Append(string.Format(Sql_Select_Products_By_Search2, Parm_Keyword + i));
                sql.Append(i + 1 < numKeywords ? Sql_Select_Products_By_Search3 : Sql_Select_Products_By_Search4);
            }

            string sqlProductsBySearch = sql.ToString();
            SqlParameter[] parms = SqlHelperParameterCache.GetCachedParameterSet(SqlHelper.SqlProviderConnString, sqlProductsBySearch);

            if (parms == null)
            {
                parms = new SqlParameter[numKeywords];

                for (int i = 0; i < numKeywords; i++)
                    parms[i] = new SqlParameter(Parm_Keyword + i, SqlDbType.NVarChar, 10);

                SqlHelperParameterCache.CacheParameterSet(SqlHelper.SqlProviderConnString, sqlProductsBySearch, parms);
            }

            for (int i = 0; i < numKeywords; i++)
                parms[i].Value = keywords[i];

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, sqlProductsBySearch, parms))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ProductId = reader.GetString(0);
                        model.ProductName = reader.GetString(1);
                        model.Subtitle = reader.GetString(2);
                        model.ProductPrice = reader.GetDecimal(3);
                        model.ImagesUrl = reader.GetString(4);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<Model.ProductInfo> GetProducts(int pageIndex, int pageSize, out int totalCount, string sqlWhere,string orderBy, params SqlParameter[] commandParameters)
        {
            List<Model.ProductInfo> list = new List<Model.ProductInfo>();

            //获取数据集总数
            string cmdText = "select count(*) from Product ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            string orderName = "CreateDate desc";
            if (!string.IsNullOrEmpty(orderBy)) orderName = orderBy;
            cmdText = @"select * from(select row_number() over(order by " + orderName + ") as RowNumber,ProductId,CategoryId,ProductName,Subtitle,ProductPrice,ImagesUrl,CreateDate from Product ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ProductId = reader["ProductId"].ToString();
                        model.CategoryId = reader["CategoryId"].ToString();
                        model.ProductName = reader["ProductName"].ToString();
                        model.Subtitle = reader["Subtitle"].ToString();
                        model.ProductPrice = decimal.Parse(reader["ProductPrice"].ToString());
                        model.ImagesUrl = reader["ImagesUrl"].ToString();
                        model.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public List<Model.ProductInfo> GetProductsInCategories(string categories)
        {
            List<Model.ProductInfo> list = new List<Model.ProductInfo>();
            SqlParameter parm = new SqlParameter("@Categories", categories);

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetProductsInCategories", parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Model.ProductInfo model = new Model.ProductInfo();
                        model.ProductId = reader.GetString(0);
                        model.CategoryId = reader.GetString(1);
                        model.ProductName = reader.GetString(2);
                        model.Subtitle = reader.GetString(3);
                        model.ProductPrice = reader.GetDecimal(4);
                        model.ImagesUrl = reader.GetString(5);

                        list.Add(model);
                    }
                }
            }

            return list;
        }
    }
}
