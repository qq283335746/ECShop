using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;

namespace TygaSoft.BLL
{
    public class Category
    {
        private static readonly ICategory dal = DALFactory.DataAccess.CreateCategory();

        #region 成员方法

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(CategoryInfo model)
        {
            return dal.Insert(model);
        }

        /// <summary>
        /// 更改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(CategoryInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int Delete(int Id)
        {
            return dal.Delete(Id);
        }

        /// <summary>
        /// 获取对应的数据
        /// </summary>
        /// <param name="numberId"></param>
        /// <returns></returns>
        public CategoryInfo GetModel(string numberId)
        {
            return dal.GetModel(numberId);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet GetList(string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(sqlWhere, commandParameters);
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
        public DataSet GetList(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            return dal.GetList(pageIndex, pageSize, out totalCount, sqlWhere, commandParameters);
        }

        /// <summary>
        /// 获取属于当前标题的所有分类
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<CategoryInfo> GetCategoryByTitle(string title)
        {
            return dal.GetCategoryByTitle(title);
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
        /// 获取满足当前条件下的所有分类键值对
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValue(string sqlWhere)
        {
            return dal.GetKeyValue(sqlWhere);
        }

        /// <summary>
        /// 获取当前父级名称下的所有分类键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentName(string parentName)
        {
            return dal.GetKeyValueByParentName(parentName);
        }

        /// <summary>
        /// 获取当前父级ID下的所有分类键值对
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueByParentId(string parentId)
        {
            return dal.GetKeyValueByParentId(parentId);
        }

        /// <summary>
        /// 获取当前父级ID下的所有子级分类
        /// </summary>
        /// <param name="parentIdsAppend"></param>
        /// <returns></returns>
        public IList<CategoryInfo> GetListInParentIds(string parentIdsAppend)
        {
            return dal.GetListInParentIds(parentIdsAppend);
        }

        /// <summary>
        /// 获取当前父级名称（包括该父级节点）下的所有子分类键值对
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetKeyValueOnParentName(string parentName)
        {
            return dal.GetKeyValueOnParentName(parentName);
        }

        /// <summary>
        /// 获取所有分类数据集
        /// </summary>
        /// <returns></returns>
        public List<CategoryInfo> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 获取树json格式字符串
        /// </summary>
        /// <returns></returns>
        public string GetTreeJson()
        {
            StringBuilder jsonAppend = new StringBuilder();
            List<CategoryInfo> list = dal.GetList();
            if (list != null && list.Count > 0)
            {
                GetTreeJson(list, Guid.Empty, ref jsonAppend);
            }

            return jsonAppend.ToString();
        }

        /// <summary>
        /// 获取树json格式字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <param name="jsonAppend"></param>
        private void GetTreeJson(List<CategoryInfo> list, object parentId, ref StringBuilder jsonAppend)
        {
            jsonAppend.Append("[");
            var childList = list.FindAll(x => x.ParentID.Equals(parentId.ToString()));
            if (childList.Count > 0)
            {
                int temp = 0;
                foreach (var item in childList)
                {
                    jsonAppend.Append("{\"id\":\"" + item.NumberID + "\",\"text\":\"" + item.CategoryName + "\",\"state\":\"open\",\"attributes\":{\"parentId\":\"" + item.ParentID + "\"}");
                    if (list.Any(r => r.ParentID.Equals(item.NumberID)))
                    {
                        jsonAppend.Append(",\"children\":");
                        GetTreeJson(list, item.NumberID, ref jsonAppend);
                    }
                    jsonAppend.Append("}");
                    if (temp < childList.Count - 1) jsonAppend.Append(",");
                    temp++;
                }
            }
            jsonAppend.Append("]");
        }  

        #endregion
    }
}
