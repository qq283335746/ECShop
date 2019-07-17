using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TygaSoft.CustomProviders
{
    /// <summary>
    /// 成员方法
    /// 
    /// 自定义特殊处理
    /// </summary>
    public class CustomsHelper
    {
        #region 自定义开始

        private string GetNullableString(SqlDataReader reader, int col)
        {
            if (reader.IsDBNull(col) == false)
            {
                return reader.GetString(col);
            }

            return null;
        }

        #endregion
    }
}
