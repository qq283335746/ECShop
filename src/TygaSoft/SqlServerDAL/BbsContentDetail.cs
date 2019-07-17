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
    public class BbsContentDetail : IBbsContentDetail
    {
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(BbsContentDetailInfo model)
        {
            if (model == null) return -1;

            //判断当前记录是否存在，如果存在则返回;
            if (IsExist(model.Title, "")) return 110;

            string cmdText = "insert into [ContentDetail] (ContentTypeID,Title,ContentText,Sort,LastUpdatedDate,UserId) values (@ContentTypeID,@Title,@ContentText,@Sort,@LastUpdatedDate,@UserId)";
            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@ContentTypeID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Title",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime), 
                                     new SqlParameter("@UserId",SqlDbType.UniqueIdentifier)  
                                   };
            parms[0].Value = model.ContentTypeID;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.Sort;
            parms[4].Value = model.LastUpdatedDate;
            parms[5].Value = model.UserId;

            //执行数据库操作
            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(BbsContentDetailInfo model)
        {
            if (IsExist(model.Title, model.NumberID)) return 110;

            //定义查询命令
            string cmdText = @"update [ContentDetail] set ContentTypeID = @ContentTypeID,Title = @Title,ContentText = @ContentText,Sort = @Sort,LastUpdatedDate = @LastUpdatedDate,UserId = @UserId where NumberID = @NumberID";

            //创建查询命令参数集
            SqlParameter[] parms = {
                                     new SqlParameter("@NumberID",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@ContentTypeID",SqlDbType.UniqueIdentifier), 
                                     new SqlParameter("@Title",SqlDbType.NVarChar,256), 
                                     new SqlParameter("@ContentText",SqlDbType.NText), 
                                     new SqlParameter("@Sort",SqlDbType.Int), 
                                     new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime), 
                                     new SqlParameter("@UserId",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = model.ContentTypeID;
            parms[1].Value = model.Title;
            parms[2].Value = model.ContentText;
            parms[3].Value = model.Sort;
            parms[4].Value = model.LastUpdatedDate;
            parms[5].Value = model.UserId;


            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms);
        }

        /// <summary>
        /// 删除对应数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public int Delete(object numberId)
        {
            Guid gId = Guid.Empty;
            Guid.TryParse(numberId.ToString(), out gId);
            if (gId == Guid.Empty) return -1;

            string cmdText = "delete from ContentDetail where NumberID = @NumberID";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
            parm.Value = gId;

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
                sb.Append(@"delete from [ContentDetail] where NumberID = @NumberID" + n + " ;");
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
        public BbsContentDetailInfo GetModel(object numberId)
        {
            Guid gId = Guid.Empty;
            Guid.TryParse(numberId.ToString(), out gId);
            if (gId == Guid.Empty) return null;

            BbsContentDetailInfo model = null;

            string cmdText = @"select top 1 NumberID,ContentTypeID,Title,ContentText,Sort,LastUpdatedDate,UserId from [ContentDetail] where NumberID = @NumberID order by LastUpdatedDate desc ";
            SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
            parm.Value = gId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new BbsContentDetailInfo();
                        model.NumberID = reader.GetGuid(0);
                        model.ContentTypeID = reader.GetGuid(1);
                        model.Title = reader.GetString(2);
                        model.ContentText = reader.GetString(3);
                        model.Sort = reader.GetInt32(4);
                        model.LastUpdatedDate = reader.GetDateTime(5);
                        model.UserId = reader.GetGuid(6);
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
        public IList<BbsContentDetailInfo> GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.NumberID,t1.ContentTypeID,t1.Title,t1.ContentText,t1.Sort,t1.LastUpdatedDate,t1.UserId from [ContentDetail] t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            IList<BbsContentDetailInfo> list = null;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<BbsContentDetailInfo>();

                    while (reader.Read())
                    {
                        BbsContentDetailInfo model = new BbsContentDetailInfo();
                        model.NumberID = reader.GetGuid(1);
                        model.ContentTypeID = reader.GetGuid(2);
                        model.Title = reader.GetString(3);
                        model.ContentText = reader.GetString(4);
                        model.Sort = reader.GetInt32(5);
                        model.LastUpdatedDate = reader.GetDateTime(6);
                        model.UserId = reader.GetGuid(7);

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
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.LastUpdatedDate desc) as RowNumber,t1.NumberID,t1.ContentTypeID,t1.Title,t1.ContentText,t1.Sort,t1.LastUpdatedDate,t1.UserId from [ContentDetail] t1 ";
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
            Guid.TryParse(numberId.ToString(),out gId);
            ParamsHelper parms = new ParamsHelper();

            string cmdText = "select 1 from [ContentDetail] where lower(Title) = @Title";
            if (gId != Guid.Empty)
            {
                cmdText = "select count(*) from [ContentDetail] where lower(Title) = @Title and NumberID <> @NumberID ";
                SqlParameter parm1 = new SqlParameter("@NumberID", SqlDbType.UniqueIdentifier);
                parm1.Value = gId;
                parms.Add(parm1);
            }
            SqlParameter parm = new SqlParameter("@Title", SqlDbType.NVarChar, 256);
            parm.Value = name;
            parms.Add(parm);

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderBbsConnString, CommandType.Text, cmdText, parms.ToArray());
            if (obj != null) return true;

            return false;
        }
    }
}
