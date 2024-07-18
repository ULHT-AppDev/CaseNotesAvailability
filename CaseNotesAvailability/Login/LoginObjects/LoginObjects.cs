using System;
using System.Collections.Generic;

namespace Login
{
    public class BasicLoginCredentials
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public bool IsBypass { get; set; } = false;
    }

    public class LoginCredentials : BasicLoginCredentials
    {
        public string Password { get; set; }
    }

    public class BasicAccountDetails : BasicLoginCredentials
    {
        public BasicAccountDetails(string domain, string username, bool bypass)
        {
            Domain = domain;
            Username = username;
            IsBypass = bypass;
        }

        public Guid Guid { get; set; }
        public bool Authenticated { get; set; } = false;
        public bool Authorised { get; set; } = false;
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string JobRole { get; set; }
    }

    public class CookieUserData
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string FullName { get => Forename + " " + Surname + " (" + Username + ")"; }
        public string Domain { get; set; }
        public int? UserID { get; set; }
        public int? SessionID { get; set; }
        public byte RoleID { get; set; }
        public List<short> Rights { get; set; }
        public bool IsAdminBypass { get; set; }

        public Guid UserGuid { get; set; }

        // extra properties
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string JobTitle { get; set; }

        //public Dictionary<string, string> AdditionalProperties { get; set; }
    }

    public class LoginResult
    {
        public int? ReturnID { get; set; }
        public LoginErrorType? ErrorType { get; set; }
    }

    //public class LoginRoles
    //{
    //    public List<LoginRole> RoleList { get; set; }
    //    public LoginErrorType? ErrorType { get; set; }
    //}

    public class LoginRoles
    {
        public List<KeyValuePair<byte, string>> RoleList { get; set; }
        public LoginErrorType? ErrorType { get; set; }
    }

    public class LoginRights
    {
        public List<short> RightList { get; set; }
        public LoginErrorType? ErrorType { get; set; }
    }

    public class LoginRoleRights
    {
        public byte RoleID { get; set; }
        public List<short> Rights { get; set; }
    }

    public class LoginRole
    {
        public byte RoleID { get; set; }
        public string Role { get; set; }
    }

    public class NoticeObject
    {
        public byte WarningMinutes { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime ExpectedEndDateTime { get; set; }
    }

    public class ConnectionOutcome
    {
        public bool ConnectionSuccess { get; set; }
        public List<NoticeObject> NoticeObjList { get; set; }
    }

    public enum LoginErrorType : byte
    {
        CredentialsIncorrect = 1,
        NotAuthorisedToAccessBecauseUser = 2,
        NotAuthorisedToAccessBecauseGroup = 3,
        //NotAuthorisedToAccessBecauseRole = 4,
        ActiveDirectoryError = 5,
        SystemUnavailable = 6,
        SystemUnavailableLoginNotAttempted = 7,
        UnableToCheckUser = 8,
        UnableToInsertUser = 9,
        UnableToGetRoles = 10,
        UserDoesNotHaveRoles = 11,
        RoleDoesNotHaveRights = 12,
        UnableToGetRights = 13,
        UnableToCreateSessionID = 14,
        ConfigError = 15,
        ConfigNotImplemented = 16,
        ConfigLoginGroups = 17,
        UnknownError = 18,
        UnableToUpdateUser = 19,
        //RoleInsertFailed = 20,
        UserInDBButNoRole = 21,
        UserPrincipalNotFound = 22
    }

    public enum SystemDownType
    {
        None = 0,
        Planned = 1,
        Unplanned = 2
    }

    public enum LoginType
    {
        Login = 1,
        ContinueWithRole = 2
    }

    public class LoginErrorLogObject
    {
        public int? SessionID { get; set; }
        public string ExType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public DateTime LoggedTime { get; set; }
    }

}
