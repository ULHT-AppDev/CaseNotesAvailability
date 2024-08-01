using BusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessObjects.Enums;

namespace DAL
{
    public class LogsDAL
    {
        public int CreateNewSession(int userID)
        {
            using (var ctx = new Model.CNAModelEntities())
            {
                LogSession newSession = new LogSession()
                {
                    PersonID = (short)userID,
                    StartDateTime = DateTime.Now,
                    EndDateTime = null,
                    IsActive = true
                };

                ctx.LogSessions.Add(newSession);
                ctx.SaveChanges();

                return newSession.SessionID;
            }
        }

        public void EndUserSession(int sessionID)
        {
            using (var ctx = new Model.CNAModelEntities())
            {
                var session = ctx.LogSessions.Where(x => x.SessionID == sessionID).First();
                session.EndDateTime = System.DateTime.Now;
                ctx.SaveChanges();
            }
        }

        public void LogError(ErrorLog le)
        {
            using (var ctx = new Model.CNAModelEntities())
            {
                LogError lg = new LogError()
                {
                    SessionID = le.SessionID,
                    Type = le.ExType,
                    Message = le.Message,
                    StackTrace = le.StackTrace,
                    Source = le.Source,
                    LoggedTime = le.LoggedTime
                };

                ctx.LogErrors.Add(lg);
                ctx.SaveChanges();
            }
        }

    }
}