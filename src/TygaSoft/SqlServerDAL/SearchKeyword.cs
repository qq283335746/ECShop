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
    public class SearchKeyword : ISearchKeyword
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.SearchKeywordInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.SearchName, "")) return 110;

            string cmdText = "insert into [SearchKeyword] (NumberID,SearchName,TotalCount,LastUpdatedDate,DataCount) values (@NumberID,@SearchName,@TotalCount,@LastUpdatedDate,@DataCount)";
            //创建查询命令参数集
            SqlParameter[] parms = {
			                         new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@SearchName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@TotalCount",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime), 
                                     new SqlParameter("@DataCount",SqlDbType.Int)  
                                   };
            parms[0].Value = Guid.NewGuid();
            parms[1].Value = model.SearchName;
            parms[2].Value = model.TotalCount;
            parms[3].Value = model.LastUpdatedDate;
            parms[4].Value = model.DataCount;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.SearchKeywordInfo model)
        {
            if (model == null) return -1;

            Guid gId;
            if (!Guid.TryParse(model.NumberID.ToString(), out gId)) return -1;

            if (IsExist(model.SearchName, model.NumberID.ToString())) return 110;

            //定义查询命令
            string cmdText = @"update [SearchKeyword] set SearchName = @SearchName,LoweredSearchName = @LoweredSearchName,TotalCount = @TotalCount,LastUpdatedDate = @LastUpdatedDate,DataCount = @DataCount where NumberID = @NumberID";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@SearchName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@LoweredSearchName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@TotalCount",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime), 
                                     new SqlParameter("@DataCount",SqlDbType.Int)
                                   };
            parms[0].Value = gId;
            parms[1].Value = model.SearchName;
            parms[2].Value = model.SearchName.ToLower();
            parms[3].Value = model.TotalCount;
            parms[4].Value = model.LastUpdatedDate;
            parms[5].Value = model.DataCount;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(string numberId)
        {
            if (string.IsNullOrEmpty(numberId)) return -1;

            string cmdText = "delete from SearchKeyword where NumberID = @NumberID";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.VarChar, 50);
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
                sb.Append(@"delete from [SearchKeyword] where NumberID = @NumberID" + n + " ;");
                SqlParameter parm = new SqlParameter("@NumberID" + n + "", SqlDbType.VarChar, 50);
                parm.Value = item;
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
        public Model.SearchKeywordInfo GetModel(string numberId)
        {
            Model.SearchKeywordInfo model = null;

            string cmdText = @"select top 1 NumberID,SearchName,TotalCount,LastUpdatedDate,DataCount from [SearchKeyword] where NumberID = @NumberID order by LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.VarChar, 50);
            parm.Value = numberId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.SearchKeywordInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.SearchName = reader["SearchName"].ToString();
                        model.TotalCount = int.Parse(reader["TotalCount"].ToString());
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());
                        model.DataCount = int.Parse(reader["DataCount"].ToString());
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
        public IList<Model.SearchKeywordInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [SearchKeyword] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.TotalCount desc,t1.DataCount desc) as RowNumber,t1.NumberID,t1.SearchName,t1.TotalCount,t1.LastUpdatedDate,t1.DataCount from [SearchKeyword] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<Model.SearchKeywordInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.SearchKeywordInfo>();

                    while (reader.Read())
                    {
                        Model.SearchKeywordInfo model = new Model.SearchKeywordInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.SearchName = reader["SearchName"].ToString();
                        model.TotalCount = int.Parse(reader["TotalCount"].ToString());
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());
                        model.DataCount = int.Parse(reader["DataCount"].ToString());

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
            string cmdText = "select count(*) from [SearchKeyword] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.CreateDate desc) as RowNumber,t1.NumberID,t1.SearchName,t1.TotalCount,t1.LastUpdatedDate,t1.DataCount from [SearchKeyword] t1 ";
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
        public bool IsExist(string name, string numberId)
        {
            bool isExist = false;
            int totalCount = -1;
            Guid gId;

            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select count(*) from [SearchKeyword] where SearchName = @SearchName";
            if (Guid.TryParse(numberId,out gId))
            {
                cmdText = "select count(*) from [SearchKeyword] where SearchName = @SearchName and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
                parm1.Value = gId;
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@SearchName", SqlDbType.VarChar, 50);
            parm.Value = name;
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
        }

        /// <summary>
        /// 创建搜索关键字
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="totalCount"></param>
        public int CreateKeyword(string searchName, int dataCount)
        {
            SqlParameter[] parms = {
                                       new SqlParameter("@SearchName",SqlDbType.NVarChar,256),
                                       new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime),
                                       new SqlParameter("@DataCount",SqlDbType.Int),
                                       new SqlParameter("@ReturnValue",SqlDbType.Int)
                                   };
            parms[0].Value = searchName;
            parms[1].Value = DateTime.Now;
            parms[2].Value = dataCount;
            parms[3].Direction = ParameterDirection.ReturnValue;

            SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_CreateKeyword", parms);
            int rv = parms[3].Value != null ? (int)parms[3].Value : -1;
            return rv;
        }

        /// <summary>
        /// 获取搜索关键字列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeywords()
        {
            List<string> list = new List<string>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetSearchKeyword"))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }

            return list;
        }
    }
}
