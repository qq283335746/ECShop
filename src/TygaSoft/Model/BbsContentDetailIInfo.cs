using System;

namespace TygaSoft.Model
{
    public class BbsContentDetailInfo
    {
        #region 成员方法

        public object NumberID { get; set; }
        public object ContentTypeID { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public int Sort { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public object UserId { get; set; }

        #endregion
    }
}