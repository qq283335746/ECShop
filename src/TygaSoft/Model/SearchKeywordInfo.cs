using System;

namespace TygaSoft.Model
{
    public class SearchKeywordInfo
    {
        #region 成员方法

        public object NumberID { get; set; }
        public string SearchName { get; set; }
        public int TotalCount { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int DataCount { get; set; }

        #endregion
    }
}