using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Model;

namespace DAL
{
    public class AuditDAL
    {
        public List<AuditBO> GetAudit()
        {

            using (var ctx = new Model.CaseNotesAvailabilityEntities())
            {
                //return ctx.Applications.Where(x => x.IsActive).Select(x => new audit
                return (from p in ctx.Audits
                            //where p.IsActive
                        select new BusinessObjects.AuditBO
                        {
                            AuditID = p.AuditID,
                            Date = p.Date,
                            SpecialtyID = p.SpecialtyID,
                            SiteID = p.SiteID,
                            CreatedByUserID = p.CreatedByUserID,
                            CompletedByUserID = p.CompletedByUserID,
                            DueByDate = p.DueByDate,
                        }).ToList();
            }
        }

        public List<AuditClinicAnswersBO> GetAuditClincAnswers()
        {
            using (var ctx = new Model.CaseNotesAvailabilityEntities())
            {
                return (from u in ctx.AuditClincAnswers
                            //where u.IsActive 
                        select new BusinessObjects.AuditClinicAnswersBO
                        {
                            AuditClinicAnswersID = u.AuditClinicAnswersID,
                            AuditID = u.AuditID,
                            ClinicCode = u.ClinicCode,
                            NumberOfAppointmentsAllocated = u.NumberOfAppointmentsAllocated,
                            CaseNotesAvailableStartCount = u.CaseNotesAvailableStartCount,
                            TemporaryNotesCount = u.TemporaryNotesCount
                            //,isactive = u.IsActive
                        }).ToList();
            }
        }

        public List<SitesBO> GetSites()
        {
            using (var ctx = new Model.CaseNotesAvailabilityEntities())
            {
                return (from u in ctx.Sites
                            //where u.IsActive 
                        select new BusinessObjects.SitesBO
                        {
                          SiteId= u.Sites__SiteId,
                          SiteName= u.SiteName,
                            //,isactive = u.IsActive
                        }).ToList();
            }
        }

        public List<SpecilatyBO> GetSpeciality()
        {
            using (var ctx = new Model.CaseNotesAvailabilityEntities())
            {
                return (from u in ctx.Specilaties
                            //where u.IsActive 
                        select new BusinessObjects.SpecilatyBO
                        {
                          SpecilatiesID = u.SpecilatiesID,
                          Specilaties_Name=u.Specilaties_Name
                            //,isactive = u.IsActive
                        }).ToList();
            }
        }
    }

}


