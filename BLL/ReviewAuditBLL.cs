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
    public class ReviewAuditBLL
    {
        public List<AuditBO> GetAudit()
        {
            try
            {
                List<AuditClinicAnswersBO> FullAuditClincAnswer = new List<AuditClinicAnswersBO>();
                FullAuditClincAnswer = new DAL.AuditDAL().GetAuditClincAnswer().OrderByDescending(x => x.AuditID).ToList();

                FullAuditClincAnswer = FullAuditClincAnswer.GroupBy(x => x.AuditID).Select(x => new AuditClinicAnswersBO
                {
                    AuditID = x.Key,
                    ClinicCode = string.Join(",", x.ToList().Select(y => y.ClinicCode.ToString()).Distinct().ToArray()) // get clinic codes into string for clinic id seperated by comma
                }).ToList();

                List<AuditBO> Audit = new DAL.AuditDAL().GetAudit().OrderByDescending(x => x.AuditID).ToList();

                foreach (var AuditBO in Audit)
                {
                    var ClinicCode = FullAuditClincAnswer.Where(x => x.AuditID == AuditBO.AuditID).Select(x => x.ClinicCode).Distinct().ToList();

                    if (ClinicCode.Any())
                    {
                        AuditBO.ClinicCodes = string.Join(",", ClinicCode.ToArray());
                    }
                    else
                    {
                        AuditBO.ClinicCodes = "";
                    }
                }

                return Audit;
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


        public List<RequiresImprovementDetailsBO> GetImprovementDetails()
        {
            try
            {
                List<RequiresImprovementDetailsBO> Status = new List<RequiresImprovementDetailsBO>();
                Status = new DAL.ReviewAuditDAL().GetImprovementDetails().OrderBy(x => x.RequiresImprovementDetailsID).ToList();

                return Status;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }

        public List<Issues> GetImprovementReason()
        {
            try
            {
                List<Issues> Status = new List<Issues>();
                Status = new DAL.ReviewAuditDAL().GetImprovementReason();

                return Status;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
            }
        }

        public bool UpdateImprovementDetails(RequiresImprovementDetailsBO Details)
        {
            try
            {
                new DAL.ReviewAuditDAL().UpdateImprovementDetails(Details);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }
        public bool InsertImprovementDetails(RequiresImprovementDetailsBO Details)
        {
            try
            {
                new DAL.ReviewAuditDAL().InsertImprovementDetails(Details);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public List<ReasonUnavailableBO> GetUnavailableReason()
        {
            try
            {
                List<ReasonUnavailableBO> Specility = new List<ReasonUnavailableBO>();
                Specility = new DAL.ReviewAuditDAL().GetUnavailableReason().OrderBy(x => x.ReasonText).ToList();

                return Specility;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, sessionID, null);
                //new LogsBLL().LogAnError(error);
                return null;
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


        public bool ImprovementDetailsCallbackUpdate(List<ImprovementDetailsCallbackBO> ImprovementDetailsCallback, string ClinicCode, int userID)
        {
            try
            {
                new DAL.ReviewAuditDAL().ImprovementDetailsCallbackUpdate(ImprovementDetailsCallback, ClinicCode, userID);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public bool ImprovementActionDetailsCallbackUpdate(List<ActionDetailsCallbackBO> actionDetailsCallback, string clinicCode, short userID)
        {
            try
            {
                //new DAL.ReviewAuditDAL().ImprovementActionDetailsCallbackUpdate(actionDetailsCallback, clinicCode, userID);
                return true;
            }
            catch (Exception ex)
            {
                //ErrorLog error = new ErrorLog(ex, Application.SessionID, null);
                //new LogsBLL().LogAnError(error);
                return false;
            }
        }

        public bool UpdateImprovementActionDetails(short userID,UpdateImprovementActionCallbackBO updateImprovementAction)
        {
            new DAL.ReviewAuditDAL().UpdateImprovementActionDetails(userID,updateImprovementAction);
            return true;
        }

        public bool CheckandUpdateAuditStatus(int AuditClinicAnswersID)
        {
            return new DAL.ReviewAuditDAL().CheckandUpdateAuditStatus(AuditClinicAnswersID);

        }
        public bool CheckWhetherAuditExist(int auditID)
        {
            return new DAL.ReviewAuditDAL().CheckWhetherAuditExist(auditID);
        }
    }
}
