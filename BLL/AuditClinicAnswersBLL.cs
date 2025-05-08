using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DAL;

namespace BLL
{
    public class AuditClinicAnswersBLL
    {
        //LogsBLL logsBLL = new LogsBLL();
        
        public List<AuditClinicAnswersBO> GetAuditClinicAnswers(int CSAAuditId, int SessionID)
        {
            try
            {
                List<AuditClinicAnswersBO> FullAuditClincAnswer = new List<AuditClinicAnswersBO>();
                FullAuditClincAnswer = new DAL.AuditClinicAnswersDAL().GetAuditClincAnswers(CSAAuditId).OrderByDescending(x => x.AuditID).ToList();
                //FullAuditClincAnswer = FullAuditClincAnswer.Where(x => !FullAuditClincAnswer.Contains(x)).ToList(); 
                return FullAuditClincAnswer;
            }
            catch (Exception ex)
            {
                
                new LogsBLL().LogAnError(ex, SessionID);
                //logsBLL.LogAnError(ex,, "Inserting delivery address");

                return null;
            }
        }
        
        
        public List<SpecilatyBO> GetSpeciality(int SessionID)
        {
            try
            {
                List<SpecilatyBO> Specility = new List<SpecilatyBO>();
                Specility = new DAL.AuditDAL().GetSpeciality().OrderBy(x => x.SpecilatiesName).ToList();

                return Specility;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                return null;
            }
        }
        public List<SitesBO> GetSites(int SessionID)
        {
            try
            {
                List<SitesBO> Sites = new List<SitesBO>();
                Sites = new DAL.AuditDAL().GetSites().OrderBy(x => x.SiteName).ToList();

                return Sites;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                return null;
            }
        }

        public List<StatusBO> GetStatus(int SessionID)
        {
            try
            {
                List<StatusBO> Status = new List<StatusBO>();
                Status = new DAL.AuditDAL().GetStatus().OrderBy(x => x.StatusID).ToList();

                return Status;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                return null;
            }
        }
        

        public bool InsertAudit(AuditBO Audit,int SessionID)
        {
            try
            {
                new DAL.AuditDAL().InsertAudit(Audit);
                return true;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                return false;
            }
        }

        public bool UpdateAuditRecords(AuditBO Audit, int SessionID)
        {
            try
            {
                List<string> ClinicCodes = Audit.ClinicCodes.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                List<string> CurrentClinicCodes = new DAL.AuditDAL().SelectClinicCodesforAuditId(Audit.AuditID);

                var ClinicCodesToAdd = ClinicCodes.Where(x => !CurrentClinicCodes.Contains(x)).ToList();
                var ClinicCodesToToRemove = CurrentClinicCodes.Where(x => !ClinicCodes.Contains(x)).ToList();

                // clean up spacing on entries

                if (ClinicCodesToAdd != null && ClinicCodesToAdd.Any())
                {
                    foreach (string entry in ClinicCodesToAdd)
                    {
                        entry.Trim(); // trim start and end of blank space
                    }

                    // remove any duplicates on add as there may be some if spacing is different after trim()

                    ClinicCodesToAdd = ClinicCodesToAdd.Distinct().ToList();
                }


                new DAL.AuditDAL().UpdateAudit(Audit, ClinicCodesToAdd, ClinicCodesToToRemove);
                return true;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        //public static void DeleteAudit(int AuditID)
        //{
        //    try
        //    { 
        //    new DAL.AuditDAL().DeleteAudit(AuditID);
        //    }
        //    catch (Exception ex)
        //    {
        //        new LogsBLL().LogAnError(ex, SessionID);
        //        //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
        //        //new LogsBLL().LogAnError(error);
        //        return false;
        //    }
        //}

        public AuditClinicAnswersBO GetAuditClinicAnswer(int rowID, int SessionID)
        {
            try
            {
                AuditClinicAnswersBO FullAuditClincAnswer = new AuditClinicAnswersBO();
                FullAuditClincAnswer = new DAL.AuditClinicAnswersDAL().GetAuditClincAnswer(rowID);
                //FullAuditClincAnswer = FullAuditClincAnswer.Where(x => !FullAuditClincAnswer.Contains(x)).ToList(); 
                return FullAuditClincAnswer;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }
        //public bool GetAwaitingActionCount(int AuditClinicAnswer, int auditid)
        //{

        //    return new DAL.AuditClinicAnswersDAL().GetAwaitingActionCount(AuditClinicAnswer,auditid);
        //}

        public int SaveCaseNoteAvailability(AuditClinicAnswersUnAvailableBO auditClinicAnswers, int SessionID)
        {
            try
            {

                new DAL.AuditClinicAnswersDAL().SaveCaseNoteAvailability(auditClinicAnswers);
                int remaining = new DAL.AuditClinicAnswersDAL().InsertUnAvailableCaseNoteAvailability(auditClinicAnswers);
                return remaining;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                //return false;
                return -1;
            }
        }

        public bool InsertUnAvailableCaseNoteAvailability(AuditClinicAnswersUnAvailableBO unAvailabelCaseNotes, int SessionID)
        {
            try
            {

                new DAL.AuditClinicAnswersDAL().InsertUnAvailableCaseNoteAvailability(unAvailabelCaseNotes);
                return true;
            }
            catch (Exception ex)
            {
                new LogsBLL().LogAnError(ex, SessionID);
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }
    }
}
