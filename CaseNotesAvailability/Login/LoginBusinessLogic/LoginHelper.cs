using Login.LoginObjects;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Login
{
    public static class LoginHelper
    {
        public static bool UseRolePermissions() => LoginConfig.Database_UseRolePermissions;

        public static bool GetRequireDBForLogin() => LoginConfig.Database_RequireDBForLogin;

        public static string GetSupportLink() => LoginConfig.SupportLink;

        public static string[] GetPlannedMaintenanceExclusionList() => LoginConfig.GetPlannedMaintenanceExclusionList.Select(x => x.ToUpper()).ToArray();

        public static string[] GetLoginGroups(bool bypass = false)
        {
            string[] groupList = ((!bypass) ? LoginConfig.GetAccessGroupList() : LoginConfig.BypassAccessGroups);

            //check congif is ok
            if (groupList.Contains("") || (groupList.Contains(LoginConfig.OpenAccessNoGroup) && groupList.Count() != 1) || (bypass && groupList.Contains(LoginConfig.OpenAccessNoGroup)))
            {
                throw new System.Configuration.ConfigurationErrorsException("Login Config - Group formatting");
            }

            return groupList;
        }

        public static bool CheckUserAgainstConfigGroups(UserPrincipal user, string[] groupList)
        {
            if (groupList.Contains(LoginConfig.OpenAccessNoGroup))
            {
                return true;
            }

            foreach (string groupName in groupList)
            {
                bool isUserInGroup = CheckGroupMembership(user, groupName);

                if (isUserInGroup)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckGroupMembership(UserPrincipal user, string groupName)
        {
            bool isMember = false;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ulh.xlincs.nhs.uk")) //ensure principal context for group is within ULH
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);

                if (group != null)
                {
                    //isMember = user.IsMemberOf(group); -- do not use
                    //isMember = group.GetMembers(true).Contains(user); // updated uses recursion for the membership
                    isMember = user.IsMemberOf(group); // updated (doesnt use recursion)
                }
            }

            return isMember;
        }

        public static BasicAccountDetails IsUserAuthenticatedAndAuthorisedInActiveDirectory(UserPrincipal user, LoginCredentials LC)
        {
            BasicAccountDetails bad = null;

            //UserPrincipal already verified, but, just in case...
            if (user != null)
            {
                bad = new BasicAccountDetails(LC.Domain, LC.Username, LC.IsBypass);
                bad.Guid = (Guid)user.Guid;
                bad.Authenticated = user.Context.ValidateCredentials(LC.Username, LC.Password, ContextOptions.Negotiate | ContextOptions.Signing | ContextOptions.Sealing);
                bad.Forename = user.GivenName;
                bad.Surname = user.Surname;
                bad.Authorised = CheckUserAgainstConfigGroups(user, GetLoginGroups());
                bad.EmailAddress = user.EmailAddress;
                bad.ContactNumber = user.VoiceTelephoneNumber;
            }

            // get job role

            try
            {
                // try add job title properties to userdata

                Directory​Searcher ds = new Directory​Searcher
                {
                    Filter = String.Format("(sAMAccountName={0})", bad.Username)
                };

                string propNeeded = "title";

                ds.PropertiesToLoad.Add(propNeeded);

                //ds.PropertiesToLoad.AddRange(userProperties.Values.ToArray());

                SearchResult sr = ds.FindOne();

                if (sr != null)
                {
                    var foundProp = sr.Properties[propNeeded];

                    if (foundProp != null)
                    {
                        var foundFirst = foundProp[0];

                        if (foundFirst != null)
                        {
                            bad.JobRole = foundFirst.ToString();
                        }
                    }
                }
            }
            catch
            {

            }


            return bad;
        }

        public static LoginRoles GetUsersSystemRoles(int userID)
        {
            LoginRoles loginRoles = new LoginRoles();

            try
            {
                loginRoles.RoleList = LoginConfig.GetUserRoles(userID);
            }
            catch (Exception ex)
            {
                LoginLogHelper.LogErrorNoSession(ex);
                loginRoles.ErrorType = LoginErrorType.UnableToGetRoles;
                return loginRoles;
            }

            return loginRoles;
        }

        public static LoginRights GetUserRights(byte roleID)
        {
            LoginRights lr = new LoginRights();

            try
            {
                lr.RightList = LoginConfig.GetUserRights(roleID);
            }
            catch (Exception ex)
            {
                LoginLogHelper.LogErrorNoSession(ex);
                lr.ErrorType = LoginErrorType.UnableToGetRights;
            }

            if (lr.RightList == null)
            {
                lr.ErrorType = LoginErrorType.RoleDoesNotHaveRights;
            }

            return lr;
        }

        public static LoginResult ManageDatabaseUserLogin(BasicAccountDetails BAD)
        {
            LoginResult lr = new LoginResult();
            int? userID = null;
            bool tryUpdate = false;

            try
            {
                userID = LoginConfig.CheckIfUserExists(BAD.Guid, BAD.Username, BAD.Forename, BAD.Surname);

                if (LoginConfig.Database_UpdateUsernameAndSurname && userID == null)
                {
                    userID = LoginConfig.CheckIfUserExistsByGuid(BAD.Guid);

                    if (userID != null)
                    {
                        tryUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoginLogHelper.LogErrorNoSession(ex);
                lr.ErrorType = LoginErrorType.UnableToCheckUser;
                return lr;
            }

            if (tryUpdate)
            {
                try
                {
                    LoginConfig.UpdateAccountDetailsWithNew((int)userID, BAD.Username, BAD.Forename, BAD.Surname);
                }
                catch (Exception ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    lr.ErrorType = LoginErrorType.UnableToUpdateUser;
                    return lr;
                }
            }
            else if (LoginConfig.Database_InsertUserIfNotInDB && userID == null)
            {
                try
                {
                    userID = LoginConfig.InsertNewUserIfNotInDB(BAD);
                }
                catch (Exception ex)
                {
                    string source = "";

                    if (BAD != null)
                    {
                        if (!String.IsNullOrEmpty(BAD.Username))
                        {
                            source = $"{BAD.Username} - {ex.Source}";
                        }
                        else
                        {
                            source = $"No username - {BAD.Forename} {BAD.Surname}";
                        }
                    }

                    if (source.Length > 99)
                    {
                        source = source.Substring(0, 99);
                    }

                    ex.Source = source;

                    LoginLogHelper.LogErrorNoSession(ex);
                    lr.ErrorType = LoginErrorType.UnableToInsertUser;
                    return lr;
                }

                if (userID == null)
                {
                    lr.ErrorType = LoginErrorType.ConfigNotImplemented;
                }
            }

            if (LoginConfig.Database_InsertUserIfNotInDB == false && userID == null)
            {
                lr.ErrorType = LoginErrorType.NotAuthorisedToAccessBecauseUser; // user needs to be inserted into the database manually
            }

            lr.ReturnID = userID;

            return lr;
        }

        public static void CreateLoginAuthTicket(BasicAccountDetails basicAccountDetails, byte? roleID, List<short> rights, int? userID, DateTime expireTime)
        {
            CookieUserData userData = GetFormattedUserData(basicAccountDetails, roleID, rights, userID);
            CookieHelper.SetLoginCookie(basicAccountDetails.Username, basicAccountDetails.Domain, userData, expireTime);
        }

        public static CookieUserData GetFormattedUserData(BasicAccountDetails BAD, byte? roleID, List<short> rights, int? userID)
        {
            CookieUserData userdata = new CookieUserData()
            {
                Forename = BAD.Forename,
                Surname = BAD.Surname,
                Username = BAD.Username,
                Domain = BAD.Domain,
                IsAdminBypass = BAD.IsBypass,
                ContactNumber = BAD.ContactNumber,
                EmailAddress = BAD.EmailAddress,
                JobTitle = BAD.JobRole,
                UserGuid = BAD.Guid
            };

            // Dictionary<string, string> userProperties = LoginConfig.GetADUserProperties();

            //if (userProperties.Any())
            //{

            //}

            if (userID != null)
            {
                userdata.UserID = userID;
            }

            int? sessionID = null;

            if (LoginConfig.Database_RequireDBForLogin)
            {
                try
                {
                    sessionID = LoginConfig.CreateSessionID(BAD.Guid);
                }
                catch (Exception)
                {
                    throw new CreateSessionIDException();
                }
            }

            if (sessionID != null)
            {
                userdata.SessionID = sessionID;
            }

            if (roleID != null)
            {
                userdata.RoleID = (byte)roleID;
            }

            if (rights != null && rights.Any())
            {
                userdata.Rights = rights;
            }

            return userdata;
        }

    }
}


//public class UserDetails
//{
//    public bool Authorised { get; set; } = false;
//    public string Forename { get; set; }
//    public string Surname { get; set; }
//    public Guid guid { get; set; }
//}

//public static UserDetails IsUserAuthorisedInActiveDirectory(string username, string password, string domain)
//{
//    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
//    {
//        UserDetails ud = new UserDetails();
//        ud.Authorised = pc.ValidateCredentials(username, password, ContextOptions.Negotiate | ContextOptions.Signing | ContextOptions.Sealing);

//        if (ud.Authorised)
//        {
//            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(pc, username);
//            ud.Forename = userPrincipal.GivenName;
//            ud.Surname = userPrincipal.Surname;
//            ud.guid = (Guid)userPrincipal.Guid;
//        }

//        return ud;
//    }
//}





//private static bool CheckGroupMembership(string username, string groupname, string domain)
//{
//    bool isMember = false;

//    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
//    {
//        if (pc != null)
//        {
//            UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, username);

//            if (user != null)
//            {
//                isMember = CheckGroupMembership(user, groupname);
//            }
//        }
//    }

//    return isMember;
//}

//using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, LC.Domain))
//{
//    UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, LC.Username);

//    if (user != null)
//    {
//        bad = new BasicAccountDetails(LC.Domain, LC.Username, LC.IsBypass);
//        bad.Guid = (Guid)user.Guid;
//        bad.Authenticated = pc.ValidateCredentials(LC.Username, LC.Password, ContextOptions.Negotiate | ContextOptions.Signing | ContextOptions.Sealing);

//        string[] groupList = GetLoginGroups();
//        bad.Authorised = CheckUserAgainstConfigGroups(user, groupList);
//    }
//}

//public static CheckUserResult CheckDatabaseSystemRoles(string username, byte domainID)
//{
//    // CONFIG - check here whether to return roles/ currently returns bool due to just a yes no in system
//    return null;
//}