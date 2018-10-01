using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace TygaSoft.CustomProviders
{
    public class CustomMembershipUser : MembershipUser
    {
        private bool _IsSubscriber; //是否为 Web 应用程序订阅服务或新闻稿
        private string _CustomerID; //包含单独客户数据库的唯一标识符
        private string _MobilePhone;

        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; }
        }

        public bool IsSubscriber
        {
            get { return _IsSubscriber; }
            set { _IsSubscriber = value; }
        }

        public string CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        public CustomMembershipUser(string providername,
                                  string username,
                                  object providerUserKey,
                                  string email,
                                  string passwordQuestion,
                                  string comment,
                                  bool isApproved,
                                  bool isLockedOut,
                                  DateTime creationDate,
                                  DateTime lastLoginDate,
                                  DateTime lastActivityDate,
                                  DateTime lastPasswordChangedDate,
                                  DateTime lastLockedOutDate,
                                  bool isSubscriber,
                                  string customerID,
                                  string mobilePhone) :
                                  base(providername,
                                       username,
                                       providerUserKey,
                                       email,
                                       passwordQuestion,
                                       comment,
                                       isApproved,
                                       isLockedOut,
                                       creationDate,
                                       lastLoginDate,
                                       lastActivityDate,
                                       lastPasswordChangedDate,
                                       lastLockedOutDate)
        {
            this.IsSubscriber = isSubscriber;
            this.CustomerID = customerID;
            this.MobilePhone = mobilePhone;
        }
    }
}
