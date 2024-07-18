using System;

namespace Login
{
    public static class LoginLogHelper
    {

        public static bool LogErrorNoSession(Exception ex)
        {
            try
            {
                if (LoginConfig.Logging_LogExceptionsErrors)
                {
                    LoginConfig.LogError(ex);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LogError(LoginErrorType error)
        {
            if (LoginConfig.Logging_LogNonExceptionErrors)
            {
                LoginConfig.LogError(error);
            }
        }

        public static void InsertUserLoggedInLog(int? sessionID, byte? roleID, int? userID)
        {
            LoginConfig.LogTheUsersRoleOnLogin(sessionID, roleID, userID);
        }
    }


}
