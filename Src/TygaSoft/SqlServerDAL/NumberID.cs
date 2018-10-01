using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SqlServerDAL
{
    public class NumberID
    {
        #region INumberID Members

        /// <summary>
        /// 获取自动编号NumberID
        /// </summary>
        /// <returns></returns>
        public static string GetNumberID()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 获取当前指定前缀的自动编号NumberID
        /// </summary>
        /// <param name="prefix">作为前缀的字符</param>
        /// <returns></returns>
        public static string GetNumberID(string prefix)
        {
            return prefix + System.Guid.NewGuid().ToString("N");
        }

        #endregion
    }
}
