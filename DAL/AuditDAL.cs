using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AuditDAL
    {
        public List<AuditBO> GetAudit()
        {

            using (var ctx = new Model.CNAEntities())
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
                            StatusID = p.StatusID
                        }).ToList();
            }
        }

        public List<AuditClinicAnswersBO> GetAuditClincAnswer()
        {
            using (var ctx = new Model.CNAEntities())
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
                            TemporaryNotesCount = u.TemporaryNotesCount,
                            IsReviewed = u.IsReviewed,
                            IsActive = u.IsActive
                        }).ToList();
            }
        }

        public List<SitesBO> GetSites()
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.Sites
                        where u.IsActive
                        select new BusinessObjects.SitesBO
                        {
                            SiteId = u.SiteId,
                            SiteName = u.SiteName,
                            SiteCode = u.SiteCode
                            ,
                            IsActive = u.IsActive
                        }).ToList();
            }
        }

        public List<SpecilatyBO> GetSpeciality()
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.Specialities
                        where u.IsActive
                        select new BusinessObjects.SpecilatyBO
                        {
                            SpecilatiesID = u.SpecilatiesID,
                            SpecilatiesName = u.SpecilatiesName
                            ,
                            IsActive = u.IsActive
                        }).ToList();
            }
        }


        public List<StatusBO> GetStatus()
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.Status
                        where u.IsActive
                        select new BusinessObjects.StatusBO
                        {
                            StatusID = u.StatusId,
                            StatusName = u.StatusName,
                            IsActive = u.IsActive
                        }).ToList();
            }
        }

        public void InsertAudit(AuditBO audit)
        {
            using (var ctxIns = new Model.CNAEntities())
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
                                StatusID = 1,
                                IsActive = true

                            };
                            ctxIns.Audits.Add(dt);
                            ctxIns.SaveChanges();
                            audit.AuditID = dt.AuditID;

                            //.NotesID = dt.ApplicationNotesID;


                            var elements = audit.ClinicCodes.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Distinct();

                            foreach (string items in elements)
                            {
                                if (items != null)
                                {
                                    Model.AuditClinicAnswer dt1 = new Model.AuditClinicAnswer()
                                    {
                                        AuditID = audit.AuditID,
                                        ClinicCode = items,
                                        IsActive = true
                                    };
                                    ctxIns.AuditClinicAnswers.Add(dt1);
                                    ctxIns.SaveChanges();
                                    //.NotesID = dt.ApplicationNotesID;
                                }
                            }
                        }

                        dbContextTransactionIns.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public List<string> SelectClinicCodesforAuditId(int auditID)
        {
            List<string> ClinicCodesforAudit = new List<string>();

            using (var ctxSelect = new Model.CNAEntities())
            {
                ClinicCodesforAudit = ctxSelect.AuditClinicAnswers.Where(x => x.AuditID == auditID && x.IsActive).Select(x => x.ClinicCode).Distinct().ToList();
            }

            return ClinicCodesforAudit;
        }

        public void UpdateAudit(AuditBO d, List<String> ClinicCodesToAdd, List<String> ClinicCodesToToRemove)
        {
            using (var ctxUpdate = new Model.CNAEntities())
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
                                var clinicAns = ctxUpdate.AuditClinicAnswers.Where(x => x.AuditID == d.AuditID && x.ClinicCode == ClinicCodeId && x.IsActive == true).FirstOrDefault();

                                clinicAns.IsActive = false;
                                ctxUpdate.SaveChanges();
                            }
                        }

                        ctxUpdate.SaveChanges();

                        dbContextTransactionIns.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void DeleteAudit(int AuditID)
        {
            using (var ctxUpdate = new Model.CNAEntities())
            {
                using (var dbContextTransactionDel = ctxUpdate.Database.BeginTransaction())
                {
                    try
                    {
                        var clinicAudit = ctxUpdate.Audits.Where(x => x.AuditID == AuditID && x.IsActive == true).FirstOrDefault();
                        if (clinicAudit != null)
                        {
                            clinicAudit.IsActive = false;
                            ctxUpdate.SaveChanges();
                            dbContextTransactionDel.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionDel.Rollback();
                        throw ex;
                    }
                }
            }
        }




        public SingleAuditBO GetSingleAudit(int auditID)
        {

            using (var ctx = new Model.CNAEntities())
            {
                //return ctx.Applications.Where(x => x.IsActive).Select(x => new audit
                return (from p in ctx.Audits
                        where p.IsActive && auditID == p.AuditID
                        join t in ctx.Specialities on p.SpecialtyID equals t.SpecilatiesID
                        join s in ctx.Sites on p.SiteID equals s.SiteId
                        select new BusinessObjects.SingleAuditBO
                        {
                            AuditID = p.AuditID,
                            Date = p.Date,
                            Specialty = t.SpecilatiesName,
                            Site = s.SiteName,
                            StatusID = p.StatusID
                        }).FirstOrDefault();
            }

        }
    }
}



