using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TygaSoft.IDAL;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public class Category : ICategory
    {
        #region ICategory Members

        public int Insert(Model.CategoryInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.CategoryName, model.ParentID, "")) return 110;

            //定义查询命令
            string cmdText = "insert into Category (NumberID,CategoryName,ParentID,Sort,Remark,Title,CreateDate) values (@NumberID,@CategoryName,@ParentID,@Sort,@Remark,@Title,@CreateDate)";
            //创建查询命令参数集
            SqlParameter[] parms = {
                                          new SqlParameter("@NumberID",SqlDbType.VarChar,50),
                                          new SqlParameter("@CategoryName",SqlDbType.NVarChar,256),
                                          new SqlParameter("@ParentID",SqlDbType.VarChar,50),
                                          new SqlParameter("@Sort",SqlDbType.Int),
                                          new SqlParameter("@Remark",SqlDbType.NVarChar,300),
                                          new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                          new SqlParameter("@Title",SqlDbType.NVarChar,20),
                                        };
            parms[0].Value = NumberID.GetNumberID("c");
            parms[1].Value = model.CategoryName;
            parms[2].Value = model.ParentID;
            parms[3].Value = model.Sort;
            parms[4].Value = model.Remark;
            parms[5].Value = model.CreateDate;
            parms[6].Value = model.Title;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Update(Model.CategoryInfo model)
        {
            if (model == null) return -1;

            if (IsExist(model.CategoryName, model.ParentID, model.NumberID)) return 110;

            string cmdText = "update Category set CategoryName = @CategoryName,ParentID = @ParentID,Sort = @Sort,Remark = @Remark,Title = @Title where NumberID = @NumberID";
            SqlParameter[] parms = {
                                       new SqlParameter("@NumberID",SqlDbType.VarChar,50),
                                       new SqlParameter("@CategoryName",SqlDbType.NVarChar,256),
                                       new SqlParameter("@ParentID",SqlDbType.VarChar,50),
                                       new SqlParameter("@Sort",SqlDbType.Int),
                                       new SqlParameter("@Remark",SqlDbType.VarChar,300),
                                       new SqlParameter("@CreateDate",SqlDbType.DateTime),
                                       new SqlParameter("@Title",SqlDbType.NVarChar,20)
                                   };

            parms[0].Value = model.NumberID;
            parms[1].Value = model.CategoryName;
            parms[2].Value = model.ParentID;
            parms[3].Value = model.Sort;
            parms[4].Value = model.Remark;
            parms[5].Value = model.CreateDate;
            parms[6].Value = model.Title;

            return SqlHelper.ExecuteNonQuery(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
        }

        public int Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Model.CategoryInfo GetModel(string numberId)
        {
            Model.CategoryInfo model = null;
            string cmdText = "select top 1 * from Category where NumberID = @NumberID order by CreateDate desc";
            SqlParameter parm = new SqlParameter("@NumberID", numberId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        model = new Model.CategoryInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.CategoryName = reader["CategoryName"].ToString();
                        model.ParentID = reader["ParentID"].ToString();
                        model.Sort = int.Parse(reader["Sort"].ToString());
                        model.Remark = reader["Remark"].ToString();
                        model.CreateDate = DateTime.Parse(reader["CreateDate"].ToString());
                    }
                }
            }

            return model;
        }

        public DataSet GetList(string sqlWhere, params SqlParameter[] commandParameters)
        {
            string cmdText = @"select * from View_Category ";
            if (!string.IsNullOrEmpty(sqlWhere))
            {
                cmdText += "where 1=1 " + sqlWhere;
            }

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
        }

        public DataSet GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = "select count(*) from View_Category t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);

            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by t1.CreateDate desc) as RowNumber,t1.* from View_Category t1 ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            return SqlHelper.ExecuteDataset(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, commandParameters);
        }

        public List<Model.CategoryInfo> GetCategoryByTitle(string title)
        {
            List<Model.CategoryInfo> list = null;

            SqlParameter parm = new SqlParameter("@Title", SqlDbType.NVarChar, 10);
            parm.Value = title;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryByTitle", parm))
            {
                if (reader != null && reader.HasRows)
                {
                    list = new List<Model.CategoryInfo>();
                    while (reader.Read())
                    {
                        Model.CategoryInfo model = new Model.CategoryInfo();
                        model.NumberID = reader.GetString(0);
                        model.CategoryName = reader.GetString(1);
                        model.ParentID = reader.GetString(2);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        private bool IsExist(string categoryName, string parentId, string numberId)
        {
            if (string.IsNullOrEmpty(categoryName)) return false;

            bool isExist = false;
            int totalCount = -1;

            string cmdText = @"select count(*) from Category where CategoryName = @CategoryName and ParentID = @ParentID ";
            SqlParameter[] parms = {
                                       new SqlParameter("@CategoryName", SqlDbType.NVarChar,256),
                                       new SqlParameter("@ParentID", SqlDbType.VarChar,50)
                                   };
            parms[0].Value = categoryName;
            parms[1].Value = parentId;
            if (!string.IsNullOrEmpty(numberId))
            {
                cmdText = "select count(*) from Category where CategoryName = @CategoryName and ParentID = @ParentID and NumberID <> @NumberID";
                Array.Resize(ref parms, parms.Length + 1);
                SqlParameter parm = new SqlParameter("@NumberID", SqlDbType.VarChar, 40);
                parm.Value = numberId;
                parms[2] = parm;
            }

            object obj = SqlHelper.ExecuteScalar(SqlHelper.SqlProviderConnString, CommandType.Text, cmdText, parms);
            if (obj != null) totalCount = Convert.ToInt32(obj);
            if (totalCount > 0) isExist = true;

            return isExist;
        }

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
                sb.Append(@"delete from [Category] where NumberID = @NumberID" + n + " ;");
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

        public Dictionary<string, string> GetKeyValue(string sqlWhere)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SqlParameter parm = new SqlParameter("@SqlWhere", SqlDbType.VarChar);
            parm.Value = sqlWhere;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryKeyValue", parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader["NumberID"].ToString(), reader["CategoryName"].ToString());
                    }
                }
            }

            return dic;
        }

        public Dictionary<string, string> GetKeyValueByParentName(string parentName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SqlParameter parm = new SqlParameter("@ParentName", SqlDbType.VarChar);
            parm.Value = parentName;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryKeyValueByParentName", parm))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader["NumberID"].ToString(), reader["CategoryName"].ToString());
                    }
                }
            }

            return dic;
        }

        public Dictionary<string, string> GetKeyValueByParentId(string parentId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SqlParameter parm = new SqlParameter("@ParentId", SqlDbType.VarChar);
            parm.Value = parentId;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryKeyValueByParentId", parm))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader["NumberID"].ToString(), reader["CategoryName"].ToString());
                    }
                }
            }

            return dic;
        }

        public IList<Model.CategoryInfo> GetListInParentIds(string parentIdsAppend)
        {
            IList<Model.CategoryInfo> list = null;
            SqlParameter parm = new SqlParameter("@ParentIds", parentIdsAppend);

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryInParentIds", parm))
            {
                if (reader.HasRows)
                {
                    list = new List<Model.CategoryInfo>();
                    while (reader.Read())
                    {
                        Model.CategoryInfo model = new Model.CategoryInfo();
                        model.NumberID = reader["NumberID"].ToString();
                        model.CategoryName = reader["CategoryName"].ToString();
                        model.ParentID = reader["ParentID"].ToString();
                        model.Remark = reader["Remark"].ToString();

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public Dictionary<string, string> GetKeyValueOnParentName(string parentName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            SqlParameter parm = new SqlParameter("@ParentName", SqlDbType.NVarChar, 256);
            parm.Value = parentName;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategoryKeyValueOnParentName", parm))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dic.Add(reader["NumberID"].ToString(), reader["CategoryName"].ToString());
                    }
                }
            }

            return dic;
        }

        public List<Model.CategoryInfo> GetList()
        {
            List<Model.CategoryInfo> list = new List<Model.CategoryInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.SqlProviderConnString, CommandType.StoredProcedure, "Pro_GetCategories"))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Model.CategoryInfo model = new Model.CategoryInfo();
                        model.NumberID = reader.GetString(0);
                        model.CategoryName = reader.GetString(1);
                        model.ParentID = reader.GetString(2);
                        model.Sort = reader.GetInt32(3);
                        model.Remark = reader.GetString(4);
                        model.Title = reader.GetString(5);
                        model.CreateDate = reader.GetDateTime(6);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}
