using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Model;
using System.Data.Entity.Migrations;

namespace DAL
{
    public class AuditDAL
    {
        public List<AuditBO> GetAudit()
        {

            using (var ctx = new Model.CNAModelEntities())
            {
                //return ctx.Applications.Where(x => x.IsActive).Select(x => new audit
                return (from p in ctx.Audits
                            where p.IsActive
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

        public List<AuditClinicAnswersBO> GetAuditClincAnswer()
        {
            using (var ctx = new Model.CNAModelEntities())
            {
                return (from u in ctx.AuditClinicAnswers
                            where u.IsActive 
                        select new BusinessObjects.AuditClinicAnswersBO
                        {
                            AuditClinicAnswersID = u.AuditClinicAnswersID,
                            AuditID = u.AuditID,
                            ClinicCode = u.ClinicCode,
                            NumberOfAppointmentsAllocated = u.NumberOfAppointmentsAllocated,
                            CaseNotesAvailableStartCount = u.CaseNotesAvailableStartCount,
                            TemporaryNotesCount = u.TemporaryNotesCount
                            //,IsActive = u.IsActive                            
                        }).ToList();
            }
        }

        public List<SitesBO> GetSites()
        {
            using (var ctx = new Model.CNAModelEntities())
            {
                return (from u in ctx.Sites
                            where u.IsActive 
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
            using (var ctx = new Model.CNAModelEntities())
            {
                return (from u in ctx.Specialities
                            where u.IsActive 
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
            using (var ctxIns = new Model.CNAModelEntities())
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
                                Model.AuditClinicAnswer dt = new Model.AuditClinicAnswer()
                                {
                                    AuditID = audit.AuditID,
                                    ClinicCode = items,
                                    IsActive = true
                                };
                                ctxIns.AuditClinicAnswers.Add(dt);
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

        public List<string> SelectClinicCodesforAuditId(int auditID)
        {
            List<string> ClinicCodesforAudit = new List<string>();
            using (var ctxSelect = new Model.CNAModelEntities())
            {
                using (var dbContextTransactionIns = ctxSelect.Database.BeginTransaction())
                {
                    try
                    {
                        ClinicCodesforAudit = ctxSelect.AuditClinicAnswers.Where(x => x.AuditID == auditID).Select(x => x.ClinicCode).ToList();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                    }

                }
            }
            return ClinicCodesforAudit;
        }

        public void UpdateAudit(AuditBO d, List<String> ClinicCodesToAdd, List<String> ClinicCodesToToRemove)
        {
            using (var ctxUpdate = new Model.CNAModelEntities())
            {
                using (var dbContextTransactionIns = ctxUpdate.Database.BeginTransaction())
                {
                    try
                    {
                        //List<AuditBO> audit = new List<AuditBO>();
                        var audit1 = ctxUpdate.Audits.Where(x => x.AuditID == d.AuditID).Single();
                        audit1.SpecialtyID = d.SpecialtyID;
                        audit1.SiteID = d.SiteID;
                        audit1.DueByDate = d.DueByDate;
                        ctxUpdate.SaveChanges();

                        if (ClinicCodesToAdd.Count > 0)
                        {
                            foreach (string items in ClinicCodesToAdd)
                            {

                                Model.AuditClinicAnswer dt = new Model.AuditClinicAnswer()
                                {
                                    AuditID = d.AuditID,
                                    ClinicCode = items,
                                    IsActive = true
                                };
                                ctxUpdate.AuditClinicAnswers.Add(dt);

                            }
                        }
                        if (ClinicCodesToToRemove.Count > 0)
                        {
                            foreach (String ClinicCodeId in ClinicCodesToToRemove)
                            {
                                var clinicAns = ctxUpdate.AuditClinicAnswers.Where(x => x.AuditID == d.AuditID && x.ClinicCode == ClinicCodeId).FirstOrDefault();

                                clinicAns.IsActive = false;
                            }
                        }

                        ctxUpdate.SaveChanges();

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


