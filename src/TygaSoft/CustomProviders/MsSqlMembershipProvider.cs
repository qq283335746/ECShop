using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Globalization;
using TygaSoft.DBUtility;

namespace TygaSoft.CustomProviders
{
    public class MsSqlMembershipProvider : MembershipProvider
    {
        #region 属性

        private string _sqlConnectionString;
        private bool _EnablePasswordRetrieval;
        private bool _EnablePasswordReset;
        private bool _RequiresQuestionAndAnswer;
        private string _AppName;
        private bool _RequiresUniqueEmail;
        private int _MaxInvalidPasswordAttempts;
        private int _CommandTimeout;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        private int _SchemaVersionCheck;
        private MembershipPasswordFormat _PasswordFormat;

        #endregion

        #region 重写Membership方法

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = "SqlMembershipProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", SM.GetString(SM.MembershipSqlProvider_description));
            }
            base.Initialize(name, config);

            _SchemaVersionCheck = 0;

            _EnablePasswordRetrieval = SU.GetBooleanValue(config, "enablePasswordRetrieval", false);
            _EnablePasswordReset = SU.GetBooleanValue(config, "enablePasswordReset", true);
            _RequiresQuestionAndAnswer = SU.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            _RequiresUniqueEmail = SU.GetBooleanValue(config, "requiresUniqueEmail", true);
            _MaxInvalidPasswordAttempts = SU.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            _PasswordAttemptWindow = SU.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            _MinRequiredPasswordLength = SU.GetIntValue(config, "minRequiredPasswordLength", 7, false, 128);
            _MinRequiredNonalphanumericCharacters = SU.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);

            _PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (_PasswordStrengthRegularExpression != null)
            {
                _PasswordStrengthRegularExpression = _PasswordStrengthRegularExpression.Trim();
                if (_PasswordStrengthRegularExpression.Length != 0)
                {
                    try
                    {
                        Regex regex = new Regex(_PasswordStrengthRegularExpression);
                    }
                    catch (ArgumentException e)
                    {
                        throw new ProviderException(e.Message, e);
                    }
                }
            }
            else
            {
                _PasswordStrengthRegularExpression = string.Empty;
            }
            if (_MinRequiredNonalphanumericCharacters > _MinRequiredPasswordLength)
                throw new HttpException(SM.GetString(SM.MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength));

            _CommandTimeout = SU.GetIntValue(config, "commandTimeout", 30, true, 0);
            _AppName = config["applicationName"];
            if (string.IsNullOrEmpty(_AppName))
                _AppName = SU.GetDefaultAppName();

            if (_AppName.Length > 256)
            {
                throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
            }

            string strTemp = config["passwordFormat"];
            if (strTemp == null)
                strTemp = "Hashed";

            switch (strTemp)
            {
                case "Clear":
                    _PasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                case "Encrypted":
                    _PasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Hashed":
                    _PasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                default:
                    throw new ProviderException(SM.GetString(SM.Provider_bad_password_format));
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed && EnablePasswordRetrieval)
                throw new ProviderException(SM.GetString(SM.Provider_can_not_retrieve_hashed_password));
            //if (_PasswordFormat == MembershipPasswordFormat.Encrypted && MachineKeySection.IsDecryptionKeyAutogenerated)
            //    throw new ProviderException(SecurityMessage.GetString(SecurityMessage.Can_not_use_encrypted_passwords_with_autogen_keys));

            string temp = config["connectionStringName"];
            if (temp == null || temp.Length < 1)
                throw new ProviderException(SM.GetString(SM.Connection_name_not_specified));
            _sqlConnectionString = SqlConnectionHelper.GetConnectionString(temp, true, true);
            if (_sqlConnectionString == null || _sqlConnectionString.Length < 1)
            {
                throw new ProviderException(SM.GetString(SM.Connection_string_not_found, temp));
            }

            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("commandTimeout");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(SM.GetString(SM.Provider_unrecognized_attribute, attribUnrecognized));
            }
        }

        public override string ApplicationName
        {
            get { return _AppName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                if (value.Length > 256)
                    throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
                _AppName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            SU.CheckParameter( ref username, true, true, true, 256, "username" );
            SU.CheckParameter( ref oldPassword, true, true, false, 128, "oldPassword" );
            SU.CheckParameter( ref newPassword, true, true, false, 128, "newPassword" );

            string salt = null;
            int passwordFormat;
            int status;

            if (!CheckPassword(username, oldPassword, false, false, out salt, out passwordFormat))
            {
                return false;
            }

            if (newPassword.Length < MinRequiredPasswordLength)
            {
                throw new ArgumentException(SM.GetString(
                              SM.Password_too_short,
                              "newPassword",
                              MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)));
            }

            int count = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    count++;
                }
            }
            if (count < MinRequiredNonAlphanumericCharacters)
            {
                throw new ArgumentException(SM.GetString(
                              SM.Password_need_more_non_alpha_numeric_chars,
                              "newPassword",
                              MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)));
            }

            if (PasswordStrengthRegularExpression.Length > 0)
            {
                if (!Regex.IsMatch(newPassword, PasswordStrengthRegularExpression))
                {
                    throw new ArgumentException(SM.GetString(SM.Password_does_not_match_regular_expression,
                                                             "newPassword"));
                }
            }

            string pass = EncodePassword(newPassword, (int)passwordFormat, salt);
            if (pass.Length > 128)
            {
                throw new ArgumentException(SM.GetString(SM.Membership_password_too_long), "newPassword");
            }

            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);
            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                else
                {
                    throw new ArgumentException(SM.GetString(SM.Membership_Custom_Password_Validation_Failure), "newPassword");
                }
            }

            SqlParameter[] parms = {
                                       new SqlParameter("@ReturnValue", SqlDbType.Int),
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, username),
                                       CreateInputParam("@NewPassword", SqlDbType.NVarChar, pass),
                                       CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, salt),
                                       CreateInputParam("@PasswordFormat", SqlDbType.Int, passwordFormat),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now),
                                   };
            parms[0].Direction = ParameterDirection.ReturnValue;

            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, "dbo.Aspnet_Membership_SetPassword", parms);
            status = ((parms[0].Value != null) ? ((int)parms[0].Value) : -1);

            if (status != 0)
            {
                string errText = GetExceptionText(status);

                if (IsStatusDueToBadPassword(status))
                {
                    throw new MembershipPasswordException(errText);
                }
                else
                {
                    throw new ProviderException(errText);
                }
            }

            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (!SU.ValidateParameter(ref password, true, true, false, 128))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            string salt = GenerateSalt();
            string psw = EncodePassword(password, (int)_PasswordFormat, salt);
            if (psw.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            string encodedPasswordAnswer;
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }

            if (!string.IsNullOrEmpty(passwordAnswer))
            {
                if (passwordAnswer.Length > 128)
                {
                    status = MembershipCreateStatus.InvalidAnswer;
                    return null;
                }
                encodedPasswordAnswer = EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int)_PasswordFormat, salt);
            }
            else
                encodedPasswordAnswer = passwordAnswer;
            if (!SU.ValidateParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, true, false, 128))
            {
                status = MembershipCreateStatus.InvalidAnswer;
                return null;
            }

            if (!SU.ValidateParameter(ref username, true, true, true, 256))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if (!SU.ValidateParameter(ref email,
                                               RequiresUniqueEmail,
                                               RequiresUniqueEmail,
                                               false,
                                               256))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (!SU.ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, true, false, 256))
            {
                status = MembershipCreateStatus.InvalidQuestion;
                return null;
            }

            if (providerUserKey != null)
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }
            }

            if (password.Length < MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            int count = 0;

            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i))
                {
                    count++;
                }
            }

            if (count < MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (PasswordStrengthRegularExpression.Length > 0)
            {
                if (!Regex.IsMatch(password, PasswordStrengthRegularExpression))
                {
                    status = MembershipCreateStatus.InvalidPassword;
                    return null;
                }
            }

            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(e);

            if (e.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            DateTime dt = RoundToSeconds(DateTime.Now);

            string spName = "Aspnet_Membership_CreateUser";
            SqlParameter[] parms = {
                                       new SqlParameter("@ApplicationName",SqlDbType.NVarChar,256),
                                       new SqlParameter("@UserName",SqlDbType.NVarChar,256),
                                       new SqlParameter("@Password",SqlDbType.NVarChar,128),
                                       new SqlParameter("@Email",SqlDbType.NVarChar,256),
                                       new SqlParameter("@PasswordQuestion",SqlDbType.NVarChar,256),
                                       new SqlParameter("@PasswordAnswer",SqlDbType.NVarChar,128),
                                       new SqlParameter("@IsApproved",SqlDbType.Bit),
                                       new SqlParameter("@PasswordFormat",SqlDbType.Int),
                                       new SqlParameter("@PasswordSalt",SqlDbType.NVarChar,128),
                                       new SqlParameter("@UniqueEmail",SqlDbType.Int),
                                       new SqlParameter("@CurrentTimeUtc",SqlDbType.DateTime),
                                       new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
                                       new SqlParameter("@ReturnValue",SqlDbType.Int)
                                   };
            parms[0].Value = ApplicationName;
            parms[1].Value = username;
            parms[2].Value = psw;
            parms[3].Value = email;
            parms[4].Value = passwordQuestion;
            parms[5].Value = passwordAnswer;
            parms[6].Value = isApproved;
            parms[7].Value = (int)PasswordFormat;
            parms[8].Value = salt;
            parms[9].Value = RequiresUniqueEmail ? 1 : 0;
            parms[10].Value = DateTime.Now;
            parms[11].Value = providerUserKey;
            parms[11].Direction = ParameterDirection.InputOutput;
            parms[12].Direction = ParameterDirection.ReturnValue;

            SqlHelper.ExecuteNonQuery(_sqlConnectionString, CommandType.StoredProcedure, spName, parms);

            int iStatus = ((parms[12].Value != null) ? ((int)parms[12].Value) : -1);
            if (iStatus < 0 || iStatus > (int)MembershipCreateStatus.ProviderError)
                iStatus = (int)MembershipCreateStatus.ProviderError;
            status = (MembershipCreateStatus)iStatus;
            if (iStatus != 0) // !success
                return null;

            providerUserKey = new Guid(parms[11].Value.ToString());
            dt = dt.ToLocalTime();
            return new MembershipUser(this.Name,
                                       username,
                                       providerUserKey,
                                       email,
                                       passwordQuestion,
                                       null,
                                       isApproved,
                                       false,
                                       dt,
                                       dt,
                                       dt,
                                       dt,
                                       new DateTime(1754, 1, 1));

        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            SU.CheckParameter(ref username, true, true, true, 256, "username");
            string cmdText = "dbo.Aspnet_Users_DeleteUser";
            ParamsHelper parms = new ParamsHelper();
            SqlParameter p = new SqlParameter("@NumTablesDeletedFrom", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            parms.Add(p);

            parms.Add(CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName));
            parms.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));

            if (deleteAllRelatedData)
            {
                parms.Add(CreateInputParam("@TablesToDeleteFrom", SqlDbType.Int, 0xF));
            }
            else
            {
                parms.Add(CreateInputParam("@TablesToDeleteFrom", SqlDbType.Int, 1));
            }

            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms.ToArray());
            int status = ((p.Value != null) ? ((int)p.Value) : -1);

            return (status > 0);
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _EnablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException(SM.GetString(SM.PageIndex_bad), "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException(SM.GetString(SM.PageSize_bad), "pageSize");

            long upperBound = (long)pageIndex * pageSize + pageSize - 1;
            if (upperBound > Int32.MaxValue)
                throw new ArgumentException(SM.GetString(SM.PageIndex_PageSize_bad), "pageIndex and pageSize");

            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            string cmdText = "dbo.Aspnet_Membership_GetAllUsers";
            SqlParameter[] parms = {
                                       new SqlParameter("@ReturnValue", SqlDbType.Int),
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex),
                                       CreateInputParam("@PageSize", SqlDbType.Int, pageSize)
                                   };
            parms[0].Direction = ParameterDirection.ReturnValue;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string username, email, passwordQuestion, comment;
                        bool isApproved;
                        DateTime dtCreate, dtLastLogin, dtLastActivity, dtLastPassChange;
                        Guid userId;
                        bool isLockedOut;
                        DateTime dtLastLockoutDate;

                        username = GetNullableString(reader, 0);
                        email = GetNullableString(reader, 1);
                        passwordQuestion = GetNullableString(reader, 2);
                        comment = GetNullableString(reader, 3);
                        isApproved = reader.GetBoolean(4);
                        dtCreate = reader.GetDateTime(5).ToLocalTime();
                        dtLastLogin = reader.GetDateTime(6).ToLocalTime();
                        dtLastActivity = reader.GetDateTime(7).ToLocalTime();
                        dtLastPassChange = reader.GetDateTime(8).ToLocalTime();
                        userId = reader.GetGuid(9);
                        isLockedOut = reader.GetBoolean(10);
                        dtLastLockoutDate = reader.GetDateTime(11).ToLocalTime();

                        users.Add(new MembershipUser(this.Name,
                                                       username,
                                                       userId,
                                                       email,
                                                       passwordQuestion,
                                                       comment,
                                                       isApproved,
                                                       isLockedOut,
                                                       dtCreate,
                                                       dtLastLogin,
                                                       dtLastActivity,
                                                       dtLastPassChange,
                                                       dtLastLockoutDate));
                    }
                }
            }

            if (parms[0].Value != null && parms[0].Value is int)
                totalRecords = (int)parms[0].Value;

            return users;
        }

        public override int GetNumberOfUsersOnline()
        {
            SqlParameter[] parms = {
                                        new SqlParameter("@ReturnValue", SqlDbType.Int),
                                        CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                        CreateInputParam("@MinutesSinceLastInActive", SqlDbType.Int, Membership.UserIsOnlineTimeWindow),
                                        CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now)
                                   };
            parms[0].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, "dbo.Aspnet_Membership_GetNumberOfUsersOnline", parms);
            int num = ((parms[0].Value != null) ? ((int)parms[0].Value) : -1);
            return num;
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = HttpContext.Current.User.Identity.Name;
            }

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            SU.CheckParameter(
                            ref username,
                            true,
                            false,
                            true,
                            256,
                            "username");

            string cmdText = "dbo.aspnet_Membership_GetUserByName";

            SqlParameter[] parms = {
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, username),
                                       CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now),
                                       new SqlParameter("@ReturnValue", SqlDbType.Int)
                                   };
            parms[4].Direction = ParameterDirection.ReturnValue;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms))
            {
                if (reader != null && reader.HasRows)
                {
                    if (reader.Read())
                    {
                        string email = GetNullableString(reader, 0);
                        string passwordQuestion = GetNullableString(reader, 1);
                        string comment = GetNullableString(reader, 2);
                        bool isApproved = reader.GetBoolean(3);
                        DateTime dtCreate = reader.GetDateTime(4).ToLocalTime();
                        DateTime dtLastLogin = reader.GetDateTime(5).ToLocalTime();
                        DateTime dtLastActivity = reader.GetDateTime(6).ToLocalTime();
                        DateTime dtLastPassChange = reader.GetDateTime(7).ToLocalTime();
                        Guid userId = reader.GetGuid(8);
                        bool isLockedOut = reader.GetBoolean(9);
                        DateTime dtLastLockoutDate = reader.GetDateTime(10).ToLocalTime();

                        return new MembershipUser(this.Name,
                                                   username,
                                                   userId,
                                                   email,
                                                   passwordQuestion,
                                                   comment,
                                                   isApproved,
                                                   isLockedOut,
                                                   dtCreate,
                                                   dtLastLogin,
                                                   dtLastActivity,
                                                   dtLastPassChange,
                                                   dtLastLockoutDate);
                    }
                }
            }

            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null)
            {
                throw new ArgumentNullException("providerUserKey");
            }

            if (!(providerUserKey is Guid))
            {
                throw new ArgumentException(SM.GetString(SM.Membership_InvalidProviderUserKey), "providerUserKey");
            }

            string cmdText = "dbo.Aspnet_Membership_GetUserByUserId";

            SqlParameter[] parms = {
                                       CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, providerUserKey),
                                       CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now),
                                       new SqlParameter("@ReturnValue", SqlDbType.Int)
                                   };
            parms[3].Direction = ParameterDirection.ReturnValue;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string email = GetNullableString(reader, 0);
                        string passwordQuestion = GetNullableString(reader, 1);
                        string comment = GetNullableString(reader, 2);
                        bool isApproved = reader.GetBoolean(3);
                        DateTime dtCreate = reader.GetDateTime(4).ToLocalTime();
                        DateTime dtLastLogin = reader.GetDateTime(5).ToLocalTime();
                        DateTime dtLastActivity = reader.GetDateTime(6).ToLocalTime();
                        DateTime dtLastPassChange = reader.GetDateTime(7).ToLocalTime();
                        string userName = GetNullableString(reader, 8);
                        bool isLockedOut = reader.GetBoolean(9);
                        DateTime dtLastLockoutDate = reader.GetDateTime(10).ToLocalTime();

                        // Step 4 : Return the result
                        return new MembershipUser(this.Name,
                                                   userName,
                                                   providerUserKey,
                                                   email,
                                                   passwordQuestion,
                                                   comment,
                                                   isApproved,
                                                   isLockedOut,
                                                   dtCreate,
                                                   dtLastLogin,
                                                   dtLastActivity,
                                                   dtLastPassChange,
                                                   dtLastLockoutDate);
                    }
                }
            }

            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _MaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _MinRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _MinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _PasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _PasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _PasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _RequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _RequiresUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            SU.CheckParameter(ref userName, true, true, true, 256, "UserName");
            string cmdText = "dbo.Aspnet_Membership_UnlockUser";
            SqlParameter[] parms = {
                                       new SqlParameter("@ReturnValue", SqlDbType.Int),
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, userName)
                                   };
            parms[0].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms);

            int status = ((parms[0].Value != null) ? ((int)parms[0].Value) : -1);
            if (status == 0)
            {
                return true;
            }

            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string temp = user.UserName;
            SU.CheckParameter(ref temp, true, true, true, 256, "UserName");
            temp = user.Email;
            SU.CheckParameter(ref temp,
                                       RequiresUniqueEmail,
                                       RequiresUniqueEmail,
                                       false,
                                       256,
                                       "Email");
            user.Email = temp;

            string cmdText = "dbo.Aspnet_Membership_UpdateUser";
            SqlParameter[] parms = {
                                       new SqlParameter("@ReturnValue", SqlDbType.Int),
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, user.UserName),
                                       CreateInputParam("@Email", SqlDbType.NVarChar, user.Email),
                                       CreateInputParam("@Comment", SqlDbType.NText, user.Comment),
                                       CreateInputParam("@IsApproved", SqlDbType.Bit, user.IsApproved ? 1 : 0),
                                       CreateInputParam("@LastLoginDate", SqlDbType.DateTime, user.LastLoginDate.ToUniversalTime()),
                                       CreateInputParam("@LastActivityDate", SqlDbType.DateTime, user.LastActivityDate.ToUniversalTime()),
                                       CreateInputParam("@UniqueEmail", SqlDbType.Int, RequiresUniqueEmail ? 1 : 0),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now)
                                   };
            parms[0].Direction = ParameterDirection.ReturnValue;
            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms);
            int status = ((parms[0].Value != null) ? ((int)parms[0].Value) : -1);
            if (status != 0)
                throw new ProviderException(GetExceptionText(status));
            return;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (SU.ValidateParameter(ref username, true, true, true, 256) &&
                    SU.ValidateParameter(ref password, true, true, false, 128) &&
                    CheckPassword(username, password, true, true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 获取用户数据分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalCount, string sqlWhere, params SqlParameter[] commandParameters)
        {
            //获取数据集总数
            string cmdText = @"select count(*) from aspnet_Users u 
                            join aspnet_Membership m on m.UserId = u.UserId
                            join aspnet_Applications a on a.ApplicationId = u.ApplicationId ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += "where 1=1 " + sqlWhere;
            totalCount = (int)SqlHelper.ExecuteScalar(SqlHelper.AspnetDbConnString, CommandType.Text, cmdText, commandParameters);
            //返回分页数据
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            cmdText = @"select * from(select row_number() over(order by m.CreateDate desc) as RowNumber,
                      u.UserId,u.UserName,u.LastActivityDate,m.Email,m.PasswordQuestion,m.Comment,m.IsApproved,m.CreateDate,
                      m.LastLoginDate,m.LastPasswordChangedDate,m.IsLockedOut,m.LastLockoutDate
                      from aspnet_Users u 
                      join aspnet_Membership m on m.UserId = u.UserId
                      join aspnet_Applications a on a.ApplicationId = u.ApplicationId 
                      where 1=1 and a.ApplicationName = '" + ApplicationName+"' ";
            if (!string.IsNullOrEmpty(sqlWhere)) cmdText += sqlWhere;
            cmdText += ")as objTable where RowNumber between " + startIndex + " and " + endIndex + " ";

            MembershipUserCollection users = new MembershipUserCollection();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.Text, cmdText, commandParameters))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string username, email, passwordQuestion, comment;
                        bool isApproved;
                        DateTime dtCreate, dtLastLogin, dtLastActivity, dtLastPassChange;
                        Guid userId;
                        bool isLockedOut;
                        DateTime dtLastLockoutDate;

                        userId = reader.GetGuid(1);
                        username = GetNullableString(reader, 2);
                        dtLastActivity = reader.GetDateTime(3).ToLocalTime();
                        email = GetNullableString(reader, 4);
                        passwordQuestion = GetNullableString(reader, 5);
                        comment = GetNullableString(reader, 6);
                        isApproved = reader.GetBoolean(7);
                        dtCreate = reader.GetDateTime(8).ToLocalTime();
                        dtLastLogin = reader.GetDateTime(9).ToLocalTime();
                        dtLastPassChange = reader.GetDateTime(10).ToLocalTime();
                        isLockedOut = reader.GetBoolean(11);
                        dtLastLockoutDate = reader.GetDateTime(12).ToLocalTime();

                        
                        users.Add(new MembershipUser(this.Name,username,userId,email,passwordQuestion,comment,isApproved,isLockedOut,dtCreate,dtLastLogin,dtLastActivity,dtLastPassChange,dtLastLockoutDate));
                    }
                }
            }

            return users;
        }

        #endregion

        #region 自定义或特定方法

        private int CommandTimeout
        {
            get { return _CommandTimeout; }
        }

        /// <summary>
        /// 转换为加密后的Base64位字符串
        /// </summary>
        /// <returns></returns>
        internal string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="passwordFormat"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        internal string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            if (passwordFormat == 1)
            { 
                // MembershipPasswordFormat.Hashed
                HashAlgorithm s = HashAlgorithm.Create(Membership.HashAlgorithmType);
                bRet = s.ComputeHash(bAll);
            }
            else
            {
                bRet = EncryptPassword(bAll);
            }

            return Convert.ToBase64String(bRet);
        }

        /// <summary>
        /// 密码解密
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        internal string UnEncodePassword(string pass, int passwordFormat)
        {
            switch (passwordFormat)
            {
                case 0: // MembershipPasswordFormat.Clear:
                    return pass;
                case 1: // MembershipPasswordFormat.Hashed:
                    throw new ProviderException(SM.GetString(SM.Provider_can_not_decode_hashed_password));
                default:
                    byte[] bIn = Convert.FromBase64String(pass);
                    byte[] bRet = DecryptPassword(bIn);
                    if (bRet == null)
                        return null;
                    return Encoding.Unicode.GetString(bRet, 16, bRet.Length - 16);
            }
        }

        private DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        /// <summary>
        /// 空值处理
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private string GetNullableString(SqlDataReader reader, int col)
        {
            if (reader.IsDBNull(col) == false)
            {
                return reader.GetString(col);
            }

            return null;
        }

        /// <summary>
        /// 创建SqlParameter
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);

            if (objValue == null)
            {
                param.IsNullable = true;
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = objValue;
            }

            return param;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="updateLastLoginActivityDate"></param>
        /// <param name="failIfNotApproved"></param>
        /// <returns></returns>
        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
        {
            string salt;
            int passwordFormat;
            return CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out salt, out passwordFormat);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="updateLastLoginActivityDate"></param>
        /// <param name="failIfNotApproved"></param>
        /// <param name="salt"></param>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved, out string salt, out int passwordFormat)
        {
            string passwdFromDB;
            int status;
            int failedPasswordAttemptCount;
            int failedPasswordAnswerAttemptCount;
            bool isPasswordCorrect;
            bool isApproved;
            DateTime lastLoginDate, lastActivityDate;

            GetPasswordWithFormat(username, updateLastLoginActivityDate, out status, out passwdFromDB, out passwordFormat, out salt, out failedPasswordAttemptCount,
                                  out failedPasswordAnswerAttemptCount, out isApproved, out lastLoginDate, out lastActivityDate);
            if (status != 0)
                return false;
            if (!isApproved && failIfNotApproved)
                return false;

            string encodedPasswd = EncodePassword(password, passwordFormat, salt);

            isPasswordCorrect = passwdFromDB.Equals(encodedPasswd);

            if (isPasswordCorrect && failedPasswordAttemptCount == 0 && failedPasswordAnswerAttemptCount == 0)
                return true;

            DateTime dtNow = DateTime.Now;

            SqlParameter[] parms = {
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, username),
                                       CreateInputParam("@IsPasswordCorrect", SqlDbType.Bit, isPasswordCorrect),
                                       CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate),
                                       CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, MaxInvalidPasswordAttempts),
                                       CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, PasswordAttemptWindow),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, dtNow),
                                       CreateInputParam("@LastLoginDate", SqlDbType.DateTime, isPasswordCorrect ? dtNow : lastLoginDate),
                                       CreateInputParam("@LastActivityDate", SqlDbType.DateTime, isPasswordCorrect ? dtNow : lastActivityDate),
                                       new SqlParameter("@ReturnValue", SqlDbType.Int)
                                   };
            parms[9].Direction = ParameterDirection.ReturnValue;

            string cmdText = "dbo.Aspnet_Membership_UpdateUserInfo";
            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms);

            status = ((parms[9].Value != null) ? ((int)parms[9].Value) : -1);

            return isPasswordCorrect;
        }

        /// <summary>
        /// 获取密码（可能是加密后的密码）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="updateLastLoginActivityDate"></param>
        /// <param name="status"></param>
        /// <param name="password"></param>
        /// <param name="passwordFormat"></param>
        /// <param name="passwordSalt"></param>
        /// <param name="failedPasswordAttemptCount"></param>
        /// <param name="failedPasswordAnswerAttemptCount"></param>
        /// <param name="isApproved"></param>
        /// <param name="lastLoginDate"></param>
        /// <param name="lastActivityDate"></param>
        private void GetPasswordWithFormat(string username,
                                            bool updateLastLoginActivityDate,
                                            out int status,
                                            out string password,
                                            out int passwordFormat,
                                            out string passwordSalt,
                                            out int failedPasswordAttemptCount,
                                            out int failedPasswordAnswerAttemptCount,
                                            out bool isApproved,
                                            out DateTime lastLoginDate,
                                            out DateTime lastActivityDate)
        {
            string cmdText = "dbo.Aspnet_Membership_GetPasswordWithFormat";
            SqlParameter[] parms = {
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, username),
                                       CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now),
                                       new SqlParameter("@ReturnValue", SqlDbType.Int)
                                   };
            parms[4].Direction = ParameterDirection.ReturnValue;

            status = -1;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms))
            {
                if (reader.Read())
                {
                    password = reader.GetString(0);
                    passwordFormat = reader.GetInt32(1);
                    passwordSalt = reader.GetString(2);
                    failedPasswordAttemptCount = reader.GetInt32(3);
                    failedPasswordAnswerAttemptCount = reader.GetInt32(4);
                    isApproved = reader.GetBoolean(5);
                    lastLoginDate = reader.GetDateTime(6);
                    lastActivityDate = reader.GetDateTime(7);
                }
                else
                {
                    password = null;
                    passwordFormat = 0;
                    passwordSalt = null;
                    failedPasswordAttemptCount = 0;
                    failedPasswordAnswerAttemptCount = 0;
                    isApproved = false;
                    lastLoginDate = DateTime.Now;
                    lastActivityDate = DateTime.Now;
                }
            }

            status = ((parms[4].Value != null) ? ((int)parms[4].Value) : -1);
        }

        private string GetExceptionText(int status)
        {
            string key;
            switch (status)
            {
                case 0:
                    return String.Empty;
                case 1:
                    key = SM.Membership_UserNotFound;
                    break;
                case 2:
                    key = SM.Membership_WrongPassword;
                    break;
                case 3:
                    key = SM.Membership_WrongAnswer;
                    break;
                case 4:
                    key = SM.Membership_InvalidPassword;
                    break;
                case 5:
                    key = SM.Membership_InvalidQuestion;
                    break;
                case 6:
                    key = SM.Membership_InvalidAnswer;
                    break;
                case 7:
                    key = SM.Membership_InvalidEmail;
                    break;
                case 99:
                    key = SM.Membership_AccountLockOut;
                    break;
                default:
                    key = SM.Provider_Error;
                    break;
            }
            return SM.GetString(key);
        }

        private bool IsStatusDueToBadPassword(int status)
        {
            return (status >= 2 && status <= 6 || status == 99);
        }

        #endregion
    }
}
