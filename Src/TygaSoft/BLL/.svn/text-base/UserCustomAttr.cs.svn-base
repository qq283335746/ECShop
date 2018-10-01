using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.BLL
{
    [Serializable]
    public class UserCustomAttr
    {
        private List<Model.UserCustomAttrInfo> list = new List<Model.UserCustomAttrInfo>();

        public void Insert(Model.UserCustomAttrInfo model)
        {
            if (!list.Contains(model))
            {
                list.Add(model);
            }
        }

        public void Update(Model.UserCustomAttrInfo model)
        {
            int i = list.FindIndex(delegate(Model.UserCustomAttrInfo m) { return m == model; });
            if (i >= 0)
            {
                list.IndexOf(model, i);
            }
        }

        public List<Model.UserCustomAttrInfo> GetList()
        {
            return list;
        }

        public Model.UserCustomAttrInfo GetModel(Model.UserCustomAttrInfo model)
        {
            return list.Find(delegate(Model.UserCustomAttrInfo m) { return m == model; });
        }

        public int Count
        {
            get { return list.Count(); }
        }

        public void Remove(Model.UserCustomAttrInfo model)
        {
            list.RemoveAll(delegate(Model.UserCustomAttrInfo m) { return m == model; });
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
