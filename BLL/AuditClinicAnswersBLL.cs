﻿using BusinessObjects;
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
       
        public List<AuditClinicAnswersBO> GetAuditClinicAnswers(int CSAAuditId)
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
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }
        
        
        public List<SpecilatyBO> GetSpeciality()
        {
            try
            {
                List<SpecilatyBO> Specility = new List<SpecilatyBO>();
                Specility = new DAL.AuditDAL().GetSpeciality().OrderBy(x => x.SpecilatiesName).ToList();

                return Specility;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }
        public List<SitesBO> GetSites()
        {
            try
            {
                List<SitesBO> Sites = new List<SitesBO>();
                Sites = new DAL.AuditDAL().GetSites().OrderBy(x => x.SiteName).ToList();

                return Sites;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }

        public List<StatusBO> GetStatus()
        {
            try
            {
                List<StatusBO> Status = new List<StatusBO>();
                Status = new DAL.AuditDAL().GetStatus().OrderBy(x => x.StatusID).ToList();

                return Status;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }
        

        public bool InsertAudit(AuditBO Audit)
        {
            try
            {
                new DAL.AuditDAL().InsertAudit(Audit);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public bool UpdateAuditRecords(AuditBO Audit)
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
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public static void DeleteAudit(int AuditID)
        {
            new DAL.AuditDAL().DeleteAudit(AuditID);
        }

        public AuditClinicAnswersBO GetAuditClinicAnswer(int rowID)
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
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }
        //public bool GetAwaitingActionCount(int AuditClinicAnswer, int auditid)
        //{

        //    return new DAL.AuditClinicAnswersDAL().GetAwaitingActionCount(AuditClinicAnswer,auditid);
        //}

        public bool SaveCaseNoteAvailability(AuditClinicAnswersUnAvailableBO auditClinicAnswers)
        {
            try
            {

                new DAL.AuditClinicAnswersDAL().SaveCaseNoteAvailability(auditClinicAnswers);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public bool InsertUnAvailableCaseNoteAvailability(AuditClinicAnswersUnAvailableBO unAvailabelCaseNotes)
        {
            try
            {

                new DAL.AuditClinicAnswersDAL().InsertUnAvailableCaseNoteAvailability(unAvailabelCaseNotes);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }
    }
}
