using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Shares.ShoppingCart
{
    public partial class AddCartSucceed : System.Web.UI.Page
    {
        CustomProfileCommon profile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForShare(Page, ltrTheme);

                Bind();
            }
        }

        private void Bind()
        {
            if (profile == null) profile = new CustomProfileCommon();
            rpCart.DataSource = profile.ShoppingCart.CartItems;
            rpCart.DataBind();
        }

        protected string GetRelativePath(string fromPath)
        {
            return VirtualPathUtility.MakeRelative(Request.AppRelativeCurrentExecutionFilePath,fromPath);
        }

        protected string GetCount()
        {
            if (profile == null) profile = new CustomProfileCommon();
            return profile.ShoppingCart.Count.ToString();
        }

        protected string GetTotalPrice()
        {
            if (profile == null) profile = new CustomProfileCommon();
            return profile.ShoppingCart.TotalPrice.ToString();
        }
    }
}