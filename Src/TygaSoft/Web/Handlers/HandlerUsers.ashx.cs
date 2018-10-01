using System;
using System.Collections.Generic;
using System.Web;
using TygaSoft.CustomProviders;
using System.Web.Security;

namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// HandlerUsers 的摘要说明
    /// </summary>
    public class HandlerUsers : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userName = context.Request.Params["userName"];
            if (!string.IsNullOrEmpty(userName)) CheckUsersName(userName);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void CheckUsersName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                HttpContext.Current.Response.Write("-1");
                return;
            }

            try
            {
                MembershipUser user = Membership.GetUser(userName);
                if (user != null)
                {
                    HttpContext.Current.Response.Write("1");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
        }
    }
}