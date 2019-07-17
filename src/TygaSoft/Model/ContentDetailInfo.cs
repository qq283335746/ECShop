using System;

namespace TygaSoft.Model
{
    [Serializable]
    public class ContentDetailInfo
    {
        #region 成员方法

        public object NumberID { get; set; }
        public object ContentTypeID { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public int Sort { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        //以下数据库表中不存在，为了业务需要
        public string TypeName { get; set; }

        #endregion
    }
}