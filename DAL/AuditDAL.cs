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

            using (var ctx = new Model.CNAModel())
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
            using (var ctx = new Model.CNAModel())
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
            using (var ctx = new Model.CNAModel())
            {
                return (from u in ctx.Sites
                            //where u.IsActive 
                        select new BusinessObjects.SitesBO
                        {
                            SiteId = u.SiteId,
                            SiteName = u.SiteName,
                            SiteCode = u.SiteCode
                            //,isactive = u.IsActive
                        }).ToList();
            }
        }

        public List<SpecilatyBO> GetSpeciality()
        {
            using (var ctx = new Model.CNAModel())
            {
                return (from u in ctx.Specilaties
                            //where u.IsActive 
                        select new BusinessObjects.SpecilatyBO
                        {
                            SpecilatiesID = u.SpecilatiesID,
                            SpecilatiesName = u.SpecilatiesName
                            //,isactive = u.IsActive
                        }).ToList();
            }
        }

        public void InsertAudit(AuditBO audit)
        {
            using (var ctxIns = new Model.CNAModel())
            {
                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {
                        if (audit.ClinicCodes != null)
                        {
                            Model.Audit dt = new Model.Audit()
                            {

                                Date = audit.Date,
                                SpecialtyID = audit.SpecialtyID,
                                SiteID = audit.SiteID,
                                CreatedByUserID = audit.CreatedByUserID,
                                CompletedByUserID = audit.CompletedByUserID,
                                DueByDate = audit.DueByDate,
                                IsActive = true

                            };
                            ctxIns.Audits.Add(dt);
                            ctxIns.SaveChanges();
                            //.NotesID = dt.ApplicationNotesID;
                        }

                        var elements = audit.ClinicCodes.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                        foreach (string items in elements)
                        {
                            if (items != null)
                            {
                                Model.AuditClincAnswer dt = new Model.AuditClincAnswer()
                                {
                                    AuditID=audit.AuditID,
                                    ClinicCode = items,
                                    IsActive = true
                                };
                                ctxIns.AuditClincAnswers.Add(dt);
                                ctxIns.SaveChanges();
                                //.NotesID = dt.ApplicationNotesID;
                            }
                        }

                        dbContextTransactionIns.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                    }
                }
            }
        }





    }

}


