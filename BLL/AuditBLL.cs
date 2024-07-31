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
    public class AuditBLL
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
                    //AuditClinicAnswersID = string.Join(",", x.ToList().Select(y => y.AuditClinicAnswersID.ToString()).ToArray()), // get group roles into string for token box seperated by comma
                    ClinicCode = string.Join(",", x.ToList().Select(y => y.ClinicCode.ToString()).ToArray()) // get group roles into string for token box seperated by comma
                }).ToList();

                List<AuditBO> Audit = new DAL.AuditDAL().GetAudit().OrderByDescending(x => x.AuditID).ToList();

                foreach (var AuditBO in Audit)
                {
                    var ClinicCode = FullAuditClincAnswer.Where(x => x.AuditID == AuditBO.AuditID).Select(x => x.ClinicCode).ToList();

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
                Specility = new DAL.AuditDAL().GetSpeciality().OrderByDescending(x => x.SpecilatiesName).ToList();

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
                Sites = new DAL.AuditDAL().GetSites().OrderByDescending(x => x.SiteName).ToList();

                return Sites;
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
                new DAL.AuditDAL().UpdateAudit(Audit);
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
