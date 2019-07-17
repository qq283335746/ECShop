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
    public class ContentDetail : IContentDetail
    {
        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Model.ContentDetailInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.Title,model.ContentTypeID.ToString(), "")) return 110;

            string cmdText = "insert into [ContentDetail] (ContentTypeID,Title,ContentText,Sort,LastUpdatedDate) values (@ContentTypeID,@Title,@LoweredTitle,@ContentText,@Sort,@LastUpdatedDate)";
            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@ContentTypeID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Title",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)  
                                   };
            parms[0].Value = model.ContentTypeID;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.Sort;
            parms[4].Value = model.LastUpdatedDate;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Model.ContentDetailInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.Title,model.ContentTypeID.ToString(), model.NumberID.ToString())) return 110;

            //定义查询命令
            string cmdText = @"update [ContentDetail] set ContentTypeID = @ContentTypeID,Title = @Title,ContentText = @ContentText,Sort = @Sort,LastUpdatedDate = @LastUpdatedDate where NumberID = @NumberID";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@ContentTypeID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Title",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.NumberID;
            parms[1].Value = model.ContentTypeID;
            parms[2].Value = model.Title;
            parms[3].Value = model.ContentText;
            parms[4].Value = model.Sort;
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
            if (string.IsNullOrEmpty(numberId)) return -1;

            string cmdText = "delete from ContentDetail where NumberID = @NumberID";
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
                sb.Append(@"delete from [ContentDetail] where NumberID = @NumberID" + n + " ;");
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
        public Model.ContentDetailInfo GetModel(string numberId)
        {
            Model.ContentDetailInfo model = null;

            string cmdText = @"select top 1 NumberID,ContentTypeID,Title,ContentText,Sort,LastUpdatedDate from [ContentDetail] where NumberID = @NumberID order by LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.VarChar, 50);
            parm.Value = numberId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.ContentDetailInfo();
                        model.NumberID = reader["NumberID"];
                        model.ContentTypeID = reader["ContentTypeID"];
                        model.Title = reader["Title"].ToString();
                        model.ContentText = reader["ContentText"].ToString();
                        model.Sort = int.Parse(reader["Sort"].ToString());
                        model.LastUpdatedDate = DateTime.Parse(reader["LastUpdatedDate"].ToString());
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
        public IList<Model.ContentDetailInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.NumberID,t1.ContentTypeID,t1.Title,t1.ContentText,t1.Sort,t1.LastUpdatedDate from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<Model.ContentDetailInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.ContentDetailInfo>();

                    while (reader.Read())
                    {
                        Model.ContentDetailInfo model = new Model.ContentDetailInfo();
                        model.NumberID = reader["NumberID"];
                        model.ContentTypeID = reader["ContentTypeID"];
                        model.Title = reader["Title"].ToString();
                        model.ContentText = reader["ContentText"].ToString();
                        model.Sort = int.Parse(reader["Sort"].ToString());
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
            string cmdText = "select count(*) from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.NumberID,t1.ContentTypeID,t1.Title,t1.ContentText,t1.Sort,t1.LastUpdatedDate from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 是否存在对应数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contentTypeId"></param>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public bool IsExist(string name,string contentTypeId, string numberId)
        {
            bool isExist = false;
            int totalCount = -1;

            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select count(*) from [ContentDetail] where lower(Title) = @Title and ContentTypeID = @ContentTypeID";
            if (!string.IsNullOrEmpty(numberId))
            {
                cmdText = "select count(*) from [ContentDetail] where lower(Title) = @Title and ContentTypeID = @ContentTypeID and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
                parm1.Value = Guid.Parse(numberId);
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@Title", SqlDbType.NVarChar, 256);
            parm.Value = name.ToLower();
            parms.Add(parm);
            Guid nId = Guid.Empty;
            Guid.TryParse(contentTypeId, out nId);
            parm = new SqlParameter("@ContentTypeID", SqlDbType.UniqueIdentifier);
            parm.Value = nId;
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
        }

        /// <summary>
        /// 获取内容列表
        /// </summary>
        /// <returns></returns>
        public List<Model.ContentDetailInfo> GetList()
        {
            List<Model.ContentDetailInfo> list = new List<Model.ContentDetailInfo>();

            string cmdText = "select NumberID,ContentTypeID,Title from ContentDetail order by Sort";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.ContentDetailInfo>();

                    while (reader.Read())
                    {
                        Model.ContentDetailInfo model = new Model.ContentDetailInfo();
                        model.NumberID = reader["NumberID"];
                        model.ContentTypeID = reader["ContentTypeID"];
                        model.Title = reader["Title"].ToString();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取当前根节点下的所有内容类型和内容
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public List<Model.ContentDetailInfo> GetSiteContent(string typeName)
        {
            List<Model.ContentDetailInfo> list = new List<Model.ContentDetailInfo>();

            string cmdText = @"SELECT ct2.TypeName,cd.NumberID,cd.Title,cd.ContentTypeID
                                FROM dbo.ContentType ct1,dbo.ContentType ct2,dbo.ContentDetail cd
                                WHERE ct1.NumberID = ct2.ParentID and ct2.NumberID = cd.ContentTypeID 
                                and ct1.TypeName = @TypeName";

            SqlParameter parm = new SqlParameter("@TypeName", SqlDbType.NVarChar, 256);

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText,parm))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.ContentDetailInfo>();

                    while (reader.Read())
                    {
                        Model.ContentDetailInfo model = new Model.ContentDetailInfo();
                        model.NumberID = reader["NumberID"];
                        model.ContentTypeID = reader["ContentTypeID"];
                        model.Title = reader["Title"].ToString();
                        model.TypeName = reader["TypeName"].ToString();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
