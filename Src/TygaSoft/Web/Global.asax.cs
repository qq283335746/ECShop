using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Profile;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //// 在应用程序启动时运行的代码
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Profile_OnMigrateAnonymous(object sender, ProfileMigrateEventArgs args)
        {
            CustomProfileCommon profile = new CustomProfileCommon();
            CustomProfileCommon anonymousProfile = profile.GetProfile(args.AnonymousID,false);

            profile.ShoppingCart = (BLL.Cart)anonymousProfile.GetPropertyValue("ShoppingCart");

            ProfileManager.DeleteProfile(args.AnonymousID);
            AnonymousIdentificationModule.ClearAnonymousIdentifier();

            profile.Save();

            // Delete the user row that was created for the anonymous user.
            Membership.DeleteUser(args.AnonymousID, true);
        }
    }
}