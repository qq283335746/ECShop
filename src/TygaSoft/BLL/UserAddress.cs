using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.BLL
{
    [Serializable]
    public class UserAddress
    {
        private List<Model.UserAddressInfo> list = new List<Model.UserAddressInfo>();

        public void Insert(Model.UserAddressInfo model)
        {
            if (!list.Contains(model))
            {
                list.Add(model);
            }
        }

        public void Update(Model.UserAddressInfo model)
        {
            int i = list.FindIndex(delegate(Model.UserAddressInfo m) { return m.NumberID == model.NumberID; });
            if (i >= 0)
            {
                list.IndexOf(model, i);
            }
        }

        public List<Model.UserAddressInfo> GetList()
        {
            return list;
        }

        public Model.UserAddressInfo GetModel(Guid numberId)
        {
            return list.Find(delegate(Model.UserAddressInfo m) { return m.NumberID == numberId; });
        }

        public int Count
        {
            get { return list.Count(); }
        }

        public void Remove(Guid numberId)
        {
            list.RemoveAll(delegate(Model.UserAddressInfo m) { return m.NumberID == numberId; });
        }

        public void Clear()
        {
            list.Clear();
        }
    }
}
