
using BLL;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Login
{
    public static class LoginConfig
    {

        public static readonly string AppEnvironment = ConfigurationManager.AppSettings["Environment"]; // set in the web config release
        public static readonly string SupportLink = "<a href =\"http://ulhintranet/ict-operations\">ICT Service Desk</a>";  //support link for error messages (shouldn't need changing)

        // 1. Set config properties 
        public static readonly bool Database_RequireDBForLogin = true;                                           // is the database required for a successful login (user/role/session/logging)?
        public static readonly bool Database_UpdateUsernameAndSurname = true;                                    // updates user details if changed in ad - should probably always be left on
        public static readonly bool Database_InsertUserIfNotInDB = false;                                        // (false wont insert user, will be done through system management user control) true will insert user when they login (if use Database_UseRolePermissions true then you have to set the role (Database_InsertUserIfNotInDBRole) they get when being inserted)
        public static readonly Enums.UserRoles Database_InsertUserIfNotInDB_RoleToUse = Enums.UserRoles.NotSet;  // the role which will be inserted with the user if Database_InsertUserIfNotInDB is used
        public static readonly bool Database_UseRolePermissions = true;                                         // does the database contain roles or rights for the user?

        public static readonly bool Logging_LogExceptionsErrors = true;                                 // do you want to log major login errors?
        public static readonly bool Logging_LogNonExceptionErrors = false;                              // do you want to log minor login errors (user incorrect password)?
        public static readonly bool Logging_LogUserRoleWhenLoggedIn = false;                            // do you want to log the users role when they log in (roleID:1)?

        public static readonly string OpenAccessNoGroup = "N/A";                                        // set for anyone to access the system - (shouldn't need changing)
        public static readonly string[] DevAccessGroups = { "GG ICT Application Development" };                        // development access groups
        public static readonly string[] LiveAccessGroups = { "GG ICT Application Development" };                       // live access groups
        public static readonly string[] BypassAccessGroups = { "GG ICT Application Development" };      // admin bypass access group
        public static readonly string[] GetPlannedMaintenanceExclusionList = { "LAD" };                 // Check your connection strings in web config - Set the databases that the planned maintenance databse does not exist on

        public static string[] GetAccessGroupList() //shouldn't need changing 
        {
            string env = null;

            if (AppEnvironment != null)
            {
                env = AppEnvironment.ToUpper();
            }

            switch (env)
            {
                case "LIVE":
                    return LiveAccessGroups;

                default:
                    return DevAccessGroups;
            }
        }

        // database
        public static int? CheckIfUserExists(Guid guid, string username, string forename, string surname)
        {
            LoginBLL loginBLL = new LoginBLL();
            return loginBLL.CheckIfUserExists(guid, username, forename, surname);
        }

        public static int? CheckIfUserExistsByGuid(Guid guid)
        {
            LoginBLL loginBLL = new LoginBLL();
            return loginBLL.CheckIfUserExistsByGuid(guid);
        }

        public static int? CreateSessionID(Guid userGuid)
        {
            //code to get sessionID if using session ID's
            int? sessionID = null;

            if (userGuid != null)
            {
                int userID = new LoginBLL().SelectUserID(userGuid);
                sessionID = new LogsBLL().CreateNewSession(userID);
            }

            return sessionID;

            //return null;
        }

        public static int? InsertNewUserIfNotInDB(BasicAccountDetails BAD)
        {
            LoginBLL loginBLL = new LoginBLL();

            if (Database_UseRolePermissions)
            {
                return loginBLL.InsertNewUserWithRole(BAD.Guid, BAD.Username, BAD.Forename, BAD.Surname, BAD.JobRole, BAD.EmailAddress, BAD.ContactNumber, Database_InsertUserIfNotInDB_RoleToUse);
            }
            else
            {
                return loginBLL.InsertNewUserWithoutRole(BAD.Guid, BAD.Username, BAD.Forename, BAD.Surname, BAD.JobRole, BAD.EmailAddress, BAD.ContactNumber);
            }
        }

        public static int UpdateAccountDetailsWithNew(int userID, string username, string forename, string surname)
        {
            LoginBLL loginBLL = new LoginBLL();
            loginBLL.UpdateUserAcountDetails(userID, username, forename, surname);
            return userID;
        }

        // user roles
        public static List<KeyValuePair<byte, string>> GetUserRoles(int userID) // code to get user roles for the cookie data 
        {
            LoginBLL userBLL = new LoginBLL();
            return userBLL.SelectUserRoleIDs(userID);
        }

        public static List<short> GetUserRights(byte roleID) // code to get user rights if role access 
        {
            LoginBLL loginBLL = new LoginBLL();
            return loginBLL.SelectUserRights(roleID);
        }


        // Logging
        public static void LogError(LoginErrorType error)
        {
            //LogsBLL bll = new LogsBLL();
            //bll.LogUserAction(null, LogsActions.LoginError, $"Login Error: {error.ToString()}");
        }

        public static void LogError(Exception error)
        {
            LogsBLL bll = new LogsBLL();
            bll.LogAnError(error, null);
        }

        public static void LogTheUsersRoleOnLogin(int? sessionID, byte? roleID, int? userID)
        {
            //LogsBLL logs = new LogsBLL();
            //logs.LogUserAction(sessionID, LogsActions.LoggedIn, $"Role: {((UserRoles)roleID).ToString()}");
        }

    }
}