using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.IDAL;

namespace TygaSoft.SqlServerDAL
{
    public class BbsContentType : IBbsContentType
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(BbsContentTypeInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.TypeName, null)) return 110;

            string cmdText = "insert into [ContentType] (TypeName,ParentID,Sort,SameName,LastUpdatedDate) values (@TypeName,@ParentID,@Sort,@SameName,@LastUpdatedDate)";
            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@TypeName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ParentID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@SameName",SqlDbType.NVarChar,10), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)  
                                   };
            parms[0].Value = model.TypeName;
            parms[1].Value = model.ParentID;
            parms[2].Value = model.Sort;
            parms[3].Value = model.SameName;
            parms[4].Value = model.LastUpdatedDate;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(BbsContentTypeInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.TypeName, model.NumberID)) return 110;

            //定义查询命令
            string cmdText = @"update [ContentType] set TypeName = @TypeName,ParentID = @ParentID,Sort = @Sort,SameName = @SameName,LastUpdatedDate = @LastUpdatedDate where NumberID = @NumberID";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@TypeName",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ParentID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@SameName",SqlDbType.NVarChar,10), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.NumberID;
            parms[1].Value = model.TypeName;
            parms[2].Value = model.ParentID;
            parms[3].Value = model.Sort;
            parms[4].Value = model.SameName;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(object numberId)
        {
            if (numberId == null || (Guid.Parse(numberId.ToString()) == Guid.Empty)) return -1;

            string cmdText = "delete from ContentType where NumberID = @NumberID";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(numberId.ToString());

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parm);
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
                sb.Append(@"delete from [ContentType] where NumberID = @NumberID" + n + " ;");
                SqlParameter parm = new SqlParameter("@NumberID" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }
            using (SqlConnection conn = new SqlConnection(SqlHelper.SqlProviderBbsConnString))
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
        public BbsContentTypeInfo GetModel(object numberId)
        {
            Guid gId = Guid.Empty;
            Guid.TryParse(numberId.ToString(), out gId);
            if (gId == Guid.Empty) return null;

            BbsContentTypeInfo model = null;

            string cmdText = @"select top 1 NumberID,TypeName,ParentID,Sort,SameName,LastUpdatedDate from [ContentType] where NumberID = @NumberID order by LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
            parm.Value = gId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new BbsContentTypeInfo();
                        model.NumberID = reader.GetGuid(0);
                        model.TypeName = reader.GetString(1);
                        model.ParentID = reader.GetGuid(2);
                        model.Sort = reader.GetInt32(3);
                        model.SameName = reader.GetString(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);
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
        public IList<BbsContentTypeInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [ContentType] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,NumberID,TypeName,ParentID,Sort,SameName,LastUpdatedDate from [ContentType] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<BbsContentTypeInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<BbsContentTypeInfo>();

                    while (reader.Read())
                    {
                       BbsContentTypeInfo model = new BbsContentTypeInfo();
                        model.NumberID = reader.GetGuid(1);
                        model.TypeName = reader.GetString(2);
                        model.ParentID = reader.GetGuid(3);
                        model.Sort = reader.GetInt32(4);
                        model.SameName = reader.GetString(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);

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
            string cmdText = "select count(*) from [ContentType] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,NumberID,TypeName,ParentID,Sort,SameName,LastUpdatedDate from [ContentType] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 是否存在对应数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public bool IsExist(string name, object numberId)
        {
            Guid gId = Guid.Empty;
            Guid.TryParse(numberId.ToString(), out gId);

            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select 1 from [ContentType] where lower(TypeName) = @TypeName";
            if (gId != Guid.Empty)
            {
                cmdText = "select 1 from [ContentType] where lower(TypeName) = @TypeName and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
                parm1.Value = Guid.Parse(numberId.ToString());
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@TypeName", SqlDbType.NVarChar, 256);
            parm.Value = name.ToLower();
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) return true;

            return false;
        }
    }
}
