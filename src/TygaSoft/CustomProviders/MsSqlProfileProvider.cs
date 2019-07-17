using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Profile;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using TygaSoft.DBUtility;

namespace TygaSoft.CustomProviders
{
    public class MsSqlProfileProvider : ProfileProvider
    {
        private string _AppName;
        private string _sqlConnectionString;
        private int _SchemaVersionCheck;
        private int _CommandTimeout;

        private const string Profile_ShoppingCart = "ShoppingCart";
        private const string Profile_UserAddress = "UserAddress";
        private const string Profile_UserCustomAttr = "UserCustomAttr";

        #region 重写方法

        public override void Initialize(string name, NameValueCollection config)
        {
            // Remove CAS in sample: HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, SR.Feature_not_supported_at_this_level);
            if (config == null)
                throw new ArgumentNullException("config");
            if (name == null || name.Length < 1)
                name = "SqlProfileProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", SM.GetString(SM.ProfileSqlProvider_description));
            }
            base.Initialize(name, config);

            _SchemaVersionCheck = 0;

            string temp = config["connectionStringName"];
            if (temp == null || temp.Length < 1)
                throw new ProviderException(SM.GetString(SM.Connection_name_not_specified));
            _sqlConnectionString = SqlConnectionHelper.GetConnectionString(temp, true, true);
            if (_sqlConnectionString == null || _sqlConnectionString.Length < 1)
            {
                throw new ProviderException(SM.GetString(SM.Connection_string_not_found, temp));
            }

            _AppName = config["applicationName"];
            if (string.IsNullOrEmpty(_AppName))
                _AppName = SU.GetDefaultAppName();

            if (_AppName.Length > 256)
            {
                throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
            }

            _CommandTimeout = SU.GetIntValue(config, "commandTimeout", 30, true, 0);

            config.Remove("commandTimeout");
            config.Remove("connectionStringName");
            config.Remove("applicationName");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(SM.GetString(SM.Provider_unrecognized_attribute, attribUnrecognized));
            }
        }

        private int CommandTimeout
        {
            get { return _CommandTimeout; }
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            string cmdText = "dbo.Aspnet_Profile_DeleteInactiveProfiles";
            SqlParameter[] parms = {
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int) authenticationOption),
                                       CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime())
                                   };
            object o = SqlHelper.ExecuteScalar(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms);
            if (o == null || !(o is int))
                return 0;
            return (int)o;
        }

        public override int DeleteProfiles(string[] usernames)
        {
            SU.CheckArrayParameter(ref usernames,
                                            true,
                                            true,
                                            true,
                                            256,
                                            "usernames");

            int numProfilesDeleted = 0;
            bool beginTranCalled = false;
            try
            {
                SqlConnectionHolder holder = null;
                try
                {
                    holder = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    //CheckSchemaVersion(holder.Connection);

                    SqlCommand cmd;

                    int numUsersRemaing = usernames.Length;
                    while (numUsersRemaing > 0)
                    {
                        string allUsers = usernames[usernames.Length - numUsersRemaing];
                        numUsersRemaing--;
                        for (int iter = usernames.Length - numUsersRemaing; iter < usernames.Length; iter++)
                        {
                            if (allUsers.Length + usernames[iter].Length + 1 >= 4000)
                                break;
                            allUsers += "," + usernames[iter];
                            numUsersRemaing--;
                        }
                        if (!beginTranCalled && numUsersRemaing > 0)
                        {
                            cmd = new SqlCommand("BEGIN TRANSACTION", holder.Connection);
                            cmd.ExecuteNonQuery();
                            beginTranCalled = true;
                        }

                        cmd = new SqlCommand("dbo.Aspnet_Profile_DeleteProfiles", holder.Connection);

                        cmd.CommandTimeout = CommandTimeout;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName));
                        cmd.Parameters.Add(CreateInputParam("@UserNames", SqlDbType.NVarChar, allUsers));
                        object o = cmd.ExecuteScalar();
                        if (o != null && o is int)
                            numProfilesDeleted += (int)o;

                    }

                    if (beginTranCalled)
                    {
                        cmd = new SqlCommand("COMMIT TRANSACTION", holder.Connection);
                        cmd.ExecuteNonQuery();
                        beginTranCalled = false;
                    }
                }
                catch
                {
                    if (beginTranCalled)
                    {
                        SqlCommand cmd = new SqlCommand("ROLLBACK TRANSACTION", holder.Connection);
                        cmd.ExecuteNonQuery();
                        beginTranCalled = false;
                    }
                    throw;
                }
                finally
                {
                    if (holder != null)
                    {
                        holder.Close();
                        holder = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return numProfilesDeleted;
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException("profiles");
            }

            if (profiles.Count < 1)
            {
                throw new ArgumentException(
                    SM.GetString(
                        SM.Parameter_collection_empty,
                        "profiles"),
                    "profiles");
            }

            string[] usernames = new string[profiles.Count];

            int iter = 0;
            foreach (ProfileInfo profile in profiles)
            {
                usernames[iter++] = profile.UserName;
            }

            return DeleteProfiles(usernames);
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            SqlParameter[] args = new SqlParameter[1];
            args[0] = CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime());
            return GetProfilesForQuery(args, authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            return GetProfilesForQuery(new SqlParameter[0], authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { return _AppName; }
            set
            {
                if (value.Length > 256)
                {
                    throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
                }
                _AppName = value;
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext sc, SettingsPropertyCollection properties)
        {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            if (properties.Count < 1)
                return svc;

            string username = (string)sc["UserName"];

            foreach (SettingsProperty prop in properties)
            {
                SettingsPropertyValue pv = new SettingsPropertyValue(prop);

                if (prop.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                        prop.SerializeAs = SettingsSerializeAs.String;
                    else
                    {
                        switch (pv.Property.Name)
                        {
                            case Profile_ShoppingCart:
                                prop.SerializeAs = SettingsSerializeAs.Binary;
                                break;
                            case Profile_UserAddress:
                                prop.SerializeAs = SettingsSerializeAs.Binary;
                                break;
                            case Profile_UserCustomAttr:
                                prop.SerializeAs = SettingsSerializeAs.Binary;
                                break;
                            default:
                                prop.SerializeAs = SettingsSerializeAs.Xml;
                                break;
                        }
                    }

                svc.Add(new SettingsPropertyValue(prop));
            }
            if (!String.IsNullOrEmpty(username))
                GetPropertyValuesFromDatabase(username, svc);
            return svc;
        }

        public override void SetPropertyValues(SettingsContext sc, SettingsPropertyValueCollection properties)
        {
            string username = (string)sc["UserName"];
            if (username == null || username.Length < 1 || properties.Count < 1)
                return;
            CheckUserName(username);
            bool userIsAuthenticated = (bool)sc["IsAuthenticated"];

            foreach (SettingsPropertyValue pv in properties)
            {
                if (pv.PropertyValue != null)
                {
                    switch (pv.Property.Name)
                    {
                        case Profile_ShoppingCart:
                            //SetCartItems((BLL.Cart)pv.PropertyValue, true);
                            pv.Property.SerializeAs = SettingsSerializeAs.Binary;
                            break;
                        case Profile_UserAddress:
                            pv.Property.SerializeAs = SettingsSerializeAs.Binary;
                            break;
                        case Profile_UserCustomAttr:
                            pv.Property.SerializeAs = SettingsSerializeAs.Binary;
                            break;
                        default:
                            break;
                    }
                }
            }

            string names = String.Empty;
            string values = String.Empty;
            byte[] buf = null;

            PrepareDataForSaving(ref names, ref values, ref buf, true, properties, userIsAuthenticated);
            if (names.Length == 0)
                return;

            string cmdText = "dbo.Aspnet_Profile_SetProperties";
            SqlParameter[] parms = {
                                       CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                       CreateInputParam("@UserName", SqlDbType.NVarChar, username),
                                       CreateInputParam("@PropertyNames", SqlDbType.NText, names),
                                       CreateInputParam("@PropertyValuesString", SqlDbType.NText, values),
                                       CreateInputParam("@PropertyValuesBinary", SqlDbType.Image, buf),
                                       CreateInputParam("@IsUserAnonymous", SqlDbType.Bit, !userIsAuthenticated),
                                       CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now)
                                   };
            SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms);
        }

        #endregion

        #region 私有

        private void GetPropertyValuesFromDatabase(string userName, SettingsPropertyValueCollection svc)
        {
            // Comment out events in sample: if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(EtwTraceLevel.Information, EtwTraceFlags.AppSvc)) EtwTrace.Trace(EtwTraceType.ETW_TYPE_PROFILE_BEGIN, HttpContext.Current.WorkerRequest);

            HttpContext context = HttpContext.Current;
            string[] names = null;
            string values = null;
            byte[] buf = null;
            string sName = null;

            if (context != null)
                sName = (context.Request.IsAuthenticated ? context.User.Identity.Name : context.Request.AnonymousID);

            string cmdText = "dbo.Aspnet_Profile_GetProperties";
            SqlParameter[] parms = {
                                        CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName),
                                        CreateInputParam("@UserName", SqlDbType.NVarChar, userName),
                                        CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.Now)
                                   };
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, cmdText, parms))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        names = reader.GetString(0).Split(':');
                        values = reader.GetString(1);

                        int size = (int)reader.GetBytes(2, 0, null, 0, 0);

                        buf = new byte[size];
                        reader.GetBytes(2, 0, buf, 0, size);
                    }
                }
            }

            ParseDataFromDB(names, values, buf, svc);
        }

        private static void ParseDataFromDB(string[] names, string values, byte[] buf, SettingsPropertyValueCollection properties)
        {
            if (names == null || values == null || buf == null || properties == null)
                return;
            try
            {
                for (int iter = 0; iter < names.Length / 4; iter++)
                {
                    string name = names[iter * 4];
                    SettingsPropertyValue pp = properties[name];

                    if (pp == null) // property not found
                        continue;

                    int startPos = Int32.Parse(names[iter * 4 + 2], CultureInfo.InvariantCulture);
                    int length = Int32.Parse(names[iter * 4 + 3], CultureInfo.InvariantCulture);

                    if (length == -1 && !pp.Property.PropertyType.IsValueType) // Null Value
                    {
                        pp.PropertyValue = null;
                        pp.IsDirty = false;
                        pp.Deserialized = true;
                    }
                    if (names[iter * 4 + 1] == "S" && startPos >= 0 && length > 0 && values.Length >= startPos + length)
                    {
                        pp.SerializedValue = values.Substring(startPos, length);
                    }

                    if (names[iter * 4 + 1] == "B" && startPos >= 0 && length > 0 && buf.Length >= startPos + length)
                    {
                        byte[] buf2 = new byte[length];

                        Buffer.BlockCopy(buf, startPos, buf2, 0, length);
                        pp.SerializedValue = buf2;
                    }
                }
            }
            catch
            { // Eat exceptions
            }
        }

        private static void PrepareDataForSaving(ref string allNames, ref string allValues, ref byte[] buf, bool binarySupported, SettingsPropertyValueCollection properties, bool userIsAuthenticated)
        {
            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            MemoryStream ms = (binarySupported ? new System.IO.MemoryStream() : null);
            try
            {
                try
                {
                    bool anyItemsToSave = false;

                    foreach (SettingsPropertyValue pp in properties)
                    {
                        if (pp.IsDirty)
                        {
                            if (!userIsAuthenticated)
                            {
                                bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                                if (!allowAnonymous)
                                    continue;
                            }
                            anyItemsToSave = true;
                            break;
                        }
                    }

                    if (!anyItemsToSave)
                        return;

                    foreach (SettingsPropertyValue pp in properties)
                    {
                        if (!userIsAuthenticated)
                        {
                            bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                            if (!allowAnonymous)
                                continue;
                        }

                        if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                            continue;

                        int len = 0, startPos = 0;
                        string propValue = null;

                        if (pp.Deserialized && pp.PropertyValue == null) // is value null?
                        {
                            len = -1;
                        }
                        else
                        {
                            object sVal = pp.SerializedValue;

                            if (sVal == null)
                            {
                                len = -1;
                            }
                            else
                            {
                                if (!(sVal is string) && !binarySupported)
                                {
                                    sVal = Convert.ToBase64String((byte[])sVal);
                                }

                                if (sVal is string)
                                {
                                    propValue = (string)sVal;
                                    len = propValue.Length;
                                    startPos = values.Length;
                                }
                                else
                                {
                                    byte[] b2 = (byte[])sVal;
                                    startPos = (int)ms.Position;
                                    ms.Write(b2, 0, b2.Length);
                                    ms.Position = startPos + b2.Length;
                                    len = b2.Length;
                                }
                            }
                        }

                        names.Append(pp.Name + ":" + ((propValue != null) ? "S" : "B") +
                                     ":" + startPos.ToString(CultureInfo.InvariantCulture) + ":" + len.ToString(CultureInfo.InvariantCulture) + ":");
                        if (propValue != null)
                            values.Append(propValue);
                    }

                    if (binarySupported)
                    {
                        buf = ms.ToArray();
                    }
                }
                finally
                {
                    if (ms != null)
                        ms.Close();
                }
            }
            catch
            {
                throw;
            }
            allNames = names.ToString();
            allValues = values.ToString();
        }

        private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);
            if (objValue == null)
                objValue = String.Empty;
            param.Value = objValue;
            return param;
        }

        private void CheckSchemaVersion(SqlConnection connection)
        {
            string[] features = { "Profile" };
            string version = "1";

            SU.CheckSchemaVersion(this,
                                           connection,
                                           features,
                                           version,
                                           ref _SchemaVersionCheck);
        }

        private static void CheckUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Length > 256 || userName.IndexOf(",") > 0)
                throw new ApplicationException(SM.GetString(SM.Provider_Invalid_Parameter, "user name"));
        }

        private ProfileInfoCollection GetProfilesForQuery(SqlParameter[] args, ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException(SM.GetString(SM.PageIndex_bad), "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException(SM.GetString(SM.PageSize_bad), "pageSize");

            long upperBound = (long)pageIndex * pageSize + pageSize - 1;
            if (upperBound > Int32.MaxValue)
            {
                throw new ArgumentException(SM.GetString(SM.PageIndex_PageSize_bad), "pageIndex and pageSize");
            }

            ParamsHelper parms = new ParamsHelper();
            parms.Add(CreateInputParam("@ApplicationName", SqlDbType.NVarChar, ApplicationName));
            parms.Add(CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption));
            parms.Add(CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
            parms.Add(CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
            foreach (SqlParameter arg in args)
                parms.Add(arg);

            ProfileInfoCollection profiles = new ProfileInfoCollection();
            totalRecords = 0;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.StoredProcedure, "dbo.Aspnet_Profile_GetProfiles",parms.ToArray()))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        string username;
                        DateTime dtLastActivity, dtLastUpdated;
                        bool isAnon;

                        username = reader.GetString(0);
                        isAnon = reader.GetBoolean(1);
                        dtLastActivity = DateTime.SpecifyKind(reader.GetDateTime(2), DateTimeKind.Local);
                        dtLastUpdated = DateTime.SpecifyKind(reader.GetDateTime(3), DateTimeKind.Local);
                        int size = reader.GetInt32(4);
                        profiles.Add(new ProfileInfo(username, isAnon, dtLastActivity, dtLastUpdated, size));
                    }

                    totalRecords = profiles.Count;
                    if (reader.NextResult())
                        if (reader.Read())
                            totalRecords = reader.GetInt32(0);
                }
            }

            return profiles;
        }

        #endregion

        #region 扩展

        #endregion
    }
}
