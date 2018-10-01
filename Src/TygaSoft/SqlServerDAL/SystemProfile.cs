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
    public class SystemProfile : ISystemProfile
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.SystemProfileInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.Title, "")) return 110;

            string cmdText = "insert into [SystemProfile] (NumberID,Title,ContentText,LastUpdatedDate) values (@NumberID,@Title,@ContentText,@LastUpdatedDate)";
            //创建查询命令参数集
            SqlParameter[] parms = {
			                         new SqlParameter("@NumberID",SqlDbType.VarChar,40),
                                     new SqlParameter("@Title",SqlDbType.NVarChar,50), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)  
                                   };
            parms[0].Value = NumberID.GetNumberID();
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.LastUpdatedDate;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.SystemProfileInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.Title, model.NumberID)) return 110;

            //定义查询命令
            string cmdText = @"update [SystemProfile] set Title = @Title,ContentText = @ContentText,LastUpdatedDate = @LastUpdatedDate where NumberID = @NumberID";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@NumberID",SqlDbType.VarChar,40),
                                     new SqlParameter("@Title",SqlDbType.NVarChar,50), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.NumberID;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.LastUpdatedDate;

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

            string cmdText = "delete from SystemProfile where NumberID = @NumberID";
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
                sb.Append(@"delete from [SystemProfile] where NumberID = @NumberID" + n + " ;");
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
        public Model.SystemProfileInfo GetModel(string numberId)
        {
            Model.SystemProfileInfo model = null;

            string cmdText = @"select top 1 NumberID,Title,ContentText,LastUpdatedDate from [SystemProfile] where NumberID = @NumberID order by LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.VarChar, 50);
            parm.Value = numberId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.SystemProfileInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.Title = reader["Title"].ToString();
                        model.ContentText = reader["ContentText"].ToString();
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());

                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 获取当前标题对应的数据
        /// </summary>
        /// <param name="titleAppend"></param>
        /// <returns></returns>
        public List<Model.SystemProfileInfo> GetModelInTitle(string titleAppend)
        {
            List<Model.SystemProfileInfo> list = null;

            string cmdText = "select Title,ContentText from SystemProfile where Title in(" + titleAppend + ")";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null)
                {
                    list = new List<Model.SystemProfileInfo>();
                    while (reader.Read())
                    {
                        Model.SystemProfileInfo model = new Model.SystemProfileInfo();
                        model.Title = reader.GetString(0);
                        model.ContentText = reader.GetString(1);
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
        public IList<Model.SystemProfileInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [SystemProfile] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,NumberID,Title,ContentText,LastUpdatedDate from [SystemProfile] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<Model.SystemProfileInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.SystemProfileInfo>();

                    while (reader.Read())
                    {
                        Model.SystemProfileInfo model = new Model.SystemProfileInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.Title = reader["Title"].ToString();
                        model.ContentText = reader["ContentText"].ToString();
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
            string cmdText = "select count(*) from [SystemProfile] ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,NumberID,Title,LastUpdatedDate from [SystemProfile] ";
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

            string cmdText = "select count(*) from [SystemProfile] where Title = @Title";
            if (!string.IsNullOrEmpty(numberId))
            {
                cmdText = "select count(*) from [SystemProfile] where Title = @Title and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.VarChar, 50);
                parm1.Value = numberId;
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@Title", SqlDbType.VarChar, 50);
            parm.Value = name;
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
        }
    }
}
