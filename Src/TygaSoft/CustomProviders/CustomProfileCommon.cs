using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Profile;
using TygaSoft.BLL;

namespace TygaSoft.CustomProviders
{
    public class CustomProfileCommon : ProfileBase
    {
        public new void Save()
        {
            HttpContext.Current.Profile.Save();
        }

        [SettingsAllowAnonymous(true)]
        [ProfileProvider("MsSqlProfileProvider")]
        public Cart ShoppingCart
        {
            get { return (Cart)HttpContext.Current.Profile.GetPropertyValue("ShoppingCart"); }
            set { HttpContext.Current.Profile.SetPropertyValue("ShoppingCart", value); }
        }

        [SettingsAllowAnonymous(true)]
        [ProfileProvider("MsSqlProfileProvider")]
        public UserAddress UserAddress
        {
            get { return (UserAddress)HttpContext.Current.Profile.GetPropertyValue("UserAddress"); }
            set { HttpContext.Current.Profile.SetPropertyValue("UserAddress", value); }
        }

        [SettingsAllowAnonymous(false)]
        [ProfileProvider("MsSqlProfileProvider")]
        public UserCustomAttr UserCustomAttr
        {
            get { return (UserCustomAttr)HttpContext.Current.Profile.GetPropertyValue("UserCustomAttr"); }
            set { HttpContext.Current.Profile.SetPropertyValue("UserCustomAttr", value); }
        }

        public CustomProfileCommon GetProfile(string userName,bool isAuthenticated)
        {
            return (CustomProfileCommon)ProfileBase.Create(userName, isAuthenticated);
        }

        public string GetUserName()
        {
            if (HttpContext.Current.Profile.IsAnonymous) return HttpContext.Current.Request.AnonymousID;
            else return HttpContext.Current.Profile.UserName;
        }
    }
}
