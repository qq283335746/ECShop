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
    public class ContentType : IContentType
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.ContentTypeInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.TypeName,"")) return 110;

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
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.ContentTypeInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.TypeName,model.NumberID.ToString())) return 110;

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
            parms[0].Value = Guid.Parse(model.NumberID.ToString());
            parms[1].Value = model.TypeName;
            parms[2].Value = Guid.Parse(model.ParentID.ToString());
            parms[3].Value = model.Sort;
            parms[4].Value = model.SameName;
            parms[5].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(string numberId)
        {
		    if(string.IsNullOrEmpty(numberId)) return -1;

            string cmdText = "delete from ContentType where NumberID = @NumberID";
			SqlParameter parm = new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier);
			parm.Value = Guid.Parse(numberId);

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
                sb.Append(@"delete from [ContentType] where NumberID = @NumberID"+n+" ;");
                SqlParameter parm = new SqlParameter("@NumberID"+n+"", SqlDbType.UniqueIdentifier);
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
                        int effect = SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sb.ToString(),parms != null ? parms.ToArray():null);
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
        public Model.ContentTypeInfo GetModel(string numberId)
        {
            Model.ContentTypeInfo model = null;

            string cmdText = @"select top 1 t1.NumberID,t1.TypeName,t1.ParentID,t1.Sort,t1.SameName,t1.LastUpdatedDate,
                             (select t2.TypeName from ContentType t2 where t2.NumberID = t1.ParentID) as ParentName 
                             from [ContentType] t1 where t1.NumberID = @NumberID order by t1.LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
            parm.Value = Guid.Parse(numberId);

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
					    model = new Model.ContentTypeInfo();
                        model.NumberID = reader["NumberID"];
                        model.TypeName = reader["TypeName"].ToString();
                        model.ParentID = reader["ParentID"];
                        model.Sort = int.Parse(reader["Sort"].ToString());
                        model.SameName = reader["SameName"].ToString();
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());
                        model.ParentName = reader["ParentName"].ToString();
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
        public IList<Model.ContentTypeInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [ContentType] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.ContentTypeID,t1.TypeName,t1.ParentID,t1.Sort,t1.SameName,t1.LastUpdatedDate from [ContentType] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

			IList<Model.ContentTypeInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.ContentTypeInfo>();

                    while (reader.Read())
                    {
                        Model.ContentTypeInfo model = new Model.ContentTypeInfo();
                        model.NumberID = reader["NumberID"];
                        model.TypeName = reader["TypeName"].ToString();
                        model.ParentID = reader["ParentID"];
                        model.Sort = int.Parse(reader["Sort"].ToString());
                        model.SameName = reader["SameName"].ToString();
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());

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
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.ContentTypeID,t1.TypeName,t1.ParentID,t1.Sort,t1.SameName,t1.LastUpdatedDate from [ContentType] t1 ";
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

            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select count(*) from [ContentType] where lower(TypeName) = @TypeName";
            if (!string.IsNullOrEmpty(numberId))
            {
                cmdText = "select count(*) from [ContentType] where lower(TypeName) = @TypeName and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
                parm1.Value = Guid.Parse(numberId);
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@TypeName", SqlDbType.NVarChar, 256);
            parm.Value = name.ToLower();
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
        }

        /// <summary>
        /// 获取所有内容类型
        /// </summary>
        /// <returns></returns>
        public List<Model.ContentTypeInfo> GetList()
        {
            List<Model.ContentTypeInfo> list = new List<Model.ContentTypeInfo>();

            string cmdText = @"select t1.NumberID,t1.TypeName,t1.ParentID,t1.Sort,
                             (select t2.TypeName from ContentType t2 where t2.NumberID = t1.ParentID) as ParentName 
                             from [ContentType] t1 order by t1.LastUpdatedDate ";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.ContentTypeInfo model = new Model.ContentTypeInfo();
                        model.NumberID = reader["NumberID"];
                        model.TypeName = reader["TypeName"].ToString();
                        model.ParentID = reader["ParentID"];
                        model.ParentName = reader["ParentName"].ToString();
                        model.Sort = int.Parse(reader["Sort"].ToString());

                        list.Add(model);
                    }
                }
            }

            return list;
        }
    }
}
