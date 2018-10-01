using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace TygaSoft.WebHelper
{
    public class Common
    {
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetUserId()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        ///创建验证的票据
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        string[] datas = userData.Split(',');
                        if (datas.Length >= 0) Guid.Parse(datas[0]);
                    }
                }
            }

            return Guid.Empty;
        }
    }
}
