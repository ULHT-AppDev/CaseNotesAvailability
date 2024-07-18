using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LogsBLL
    {
        LogsDAL DAL;

        public LogsBLL()
        {
            DAL = new DAL.LogsDAL();
        }

        public int CreateNewSession(int userID)
        {
            return DAL.CreateNewSession(userID);
        }

        public void EndUserSession(int sessionID)
        {
            try
            {
                DAL.EndUserSession(sessionID);
                //LogUserAction(sessionID, LogsActions.LoggedOut, null);
            }
            catch (Exception ex)
            {
                LogAnError(ex, sessionID, "LogsBLL.cs - EndUserSession");
                throw ex;
            }
        }

        public void LogAnError(Exception ex, int? sessionID, string source)
        {
            ErrorLog er = new ErrorLog(ex);
            er.SessionID = sessionID;
            er.Source = source; //overwrite source
            DAL.LogError(er);
        }

        public void LogAnError(Exception ex, int? sessionID)
        {
            ErrorLog er = new ErrorLog(ex);
            er.SessionID = sessionID;
            DAL.LogError(er);
        }

    }
}
