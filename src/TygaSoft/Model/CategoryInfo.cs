using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    [Serializable]
    public class CategoryInfo
    {
        public string NumberID { get; set; }
        public string CategoryName { get; set; }
        public string ParentID { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
