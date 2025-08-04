using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AuditClinicAnswersDAL
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

        public List<AuditClinicAnswersBO> GetAuditClincAnswers(int AuditID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.AuditClinicAnswers
                        where u.IsActive
                        && u.AuditID == AuditID
                        orderby u.IsReviewed
                        select new BusinessObjects.AuditClinicAnswersBO
                        {
                            AuditClinicAnswersID = u.AuditClinicAnswersID,
                            AuditID = u.AuditID,
                            ClinicCode = u.ClinicCode,
                            Totalappointments = u.Totalappointments,
                            NumberOfAppointmentsAllocated = u.NumberOfAppointmentsAllocated,
                            CaseNotesAvailableStartCount = u.CaseNotesAvailableStartCount,
                            TemporaryNotesCount = u.TemporaryNotesCount,
                            UnavailableCount = u.UnavailableCount,
                            IsActive = u.IsActive,
                            StatusID = u.StatusID,
                            IsReviewed = u.IsReviewed
                        }).ToList();
            }
        }
        public bool CheckWhetherAuditExist(int auditID)
        {
            using (var ctxSelect = new Model.CNAEntities())
            {

                //    var query = from c in db.Emp
                //                from d in db.EmpDetails
                //                where c.ID == d.ID && c.FirstName == "A" && c.LastName == "D"
                //this.result = query.Any().ToString()

                return ctxSelect.Audits
                       .Where(e => e.AuditID == auditID)    // Filter by the specific Id
                       .Any();
                //.All(e => e.StatusID == (byte)Enums.AuditStatus.Completed);
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
        public void SaveCaseNoteAvailability(AuditClinicAnswersUnAvailableBO auditClinicAnswers)
        {
            using (var ctxUpdate = new Model.CNAEntities())
            {
                using (var dbContextTransactionIns = ctxUpdate.Database.BeginTransaction())
                {
                    try
                    {
                        //List<AuditBO> audit = new List<AuditBO>();
                        var audit1 = ctxUpdate.AuditClinicAnswers.Where(x => x.AuditClinicAnswersID == auditClinicAnswers.AuditClinicAnswersID).Single();
                        audit1.TemporaryNotesCount = auditClinicAnswers.TemporaryNotesCount;
                        audit1.Totalappointments = auditClinicAnswers.Totalappointments;
                        audit1.CaseNotesAvailableStartCount = auditClinicAnswers.CaseNotesAvailableStartCount;
                        audit1.NumberOfAppointmentsAllocated = auditClinicAnswers.NumberOfAppointmentsAllocated;
                        audit1.UnavailableCount = auditClinicAnswers.UnavailableCount;
                        audit1.StatusID = 4;
                        ctxUpdate.SaveChanges();
                        dbContextTransactionIns.Commit();
                        UpdateAuditStatus(auditClinicAnswers.AuditID);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void UpdateAuditStatus(int Auditid)
        {
            using (var ctxSelect = new Model.CNAEntities())
            {
                try
                {


                    bool hasOnlyCompletedStatus = ctxSelect.AuditClinicAnswers
                                        .Where(e => e.AuditID == Auditid && e.IsActive)    // Filter by the specific Id
                                            .All(e => e.StatusID == (byte)Enums.AuditStatus.Completed); // Ensure all statuses are "Completed"

                    bool hasOnlyNotStartedStatus = ctxSelect.AuditClinicAnswers
                                       .Where(e => e.AuditID == Auditid && e.IsActive)    // Filter by the specific Id
                                           .All(e => e.StatusID == (byte)Enums.AuditStatus.NotStarted ); // Ensure all statuses are "not started"

                    //bool hasReviewed = ctxSelect.AuditClinicAnswers
                    //                   .Where(e => e.AuditID == Auditid)    // Filter by the specific Id
                    //                       .All(e => e.StatusID == (byte)Enums.AuditStatus.Reviewed); // Ensure all statuses are "Audited"

                    //byte status = hasReviewed ? (byte)Enums.AuditStatus.Reviewed : !hasOnlyCompletedStatus && !hasOnlyNotStartedStatus ? (byte)Enums.AuditStatus.InProgress : hasOnlyCompletedStatus ? (byte)Enums.AuditStatus.PendingHRreview : (byte)Enums.AuditStatus.NotStarted;
                    byte status = !hasOnlyCompletedStatus && !hasOnlyNotStartedStatus ? (byte)Enums.AuditStatus.InProgress : hasOnlyCompletedStatus ? (byte)Enums.AuditStatus.PendingHRreview : (byte)Enums.AuditStatus.NotStarted;
                    if (status != (byte)Enums.AuditStatus.NotStarted)
                    {
                        //update as in progress

                        using (var ctxUpdate = new Model.CNAEntities())
                        {
                            using (var dbContextTransactionIns = ctxUpdate.Database.BeginTransaction())
                            {
                                try
                                {
                                    //List<AuditBO> audit = new List<AuditBO>();
                                    var audit1 = ctxUpdate.Audits.Where(x => x.AuditID == Auditid).FirstOrDefault();
                                    audit1.StatusID = status; //(byte)Enums.AuditStatus.InProgress;
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
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public bool GetAwaitingActionCount(int AuditClinicAnswer, int auditid)
        //{
        //    using (var ctx = new Model.CNAEntities())
        //    {
        //        var AuditClinicAnswersNumber = ctx.AuditClinicAnswers.Count(me => me.AuditID == auditid && me.IsActive);
        //        var UnavailableCaseNotesNumber = ctx.UnavailableCaseNotes.Where(p => p.IsActive).GroupBy(p => p.AuditClinicAnswersID).Count();
        //        var UnavailableCaseNotesNumber1 = ctx.UnavailableCaseNotes
        //                                            .Where(e => e.IsActive && e.AuditClinicAnswersID == AuditClinicAnswer)
        //                                            .GroupBy(e => e.AuditClinicAnswersID).Count();
        //        return true;

        //    }
        //}
        public int InsertUnAvailableCaseNoteAvailability(AuditClinicAnswersUnAvailableBO unAvailabelCaseNotes)
        {
            int auditid = unAvailabelCaseNotes.AuditID;
            using (var ctxIns = new Model.CNAEntities())
            {

                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {
                        if (unAvailabelCaseNotes.UnavailableList.Count() != 0)
                        {
                            foreach (CompleteCallbackBO UnAvailableCaseN in unAvailabelCaseNotes.UnavailableList)
                            {
                                Model.UnavailableCaseNote dt = new Model.UnavailableCaseNote()
                                {
                                    AuditClinicAnswersID = unAvailabelCaseNotes.AuditClinicAnswersID,
                                    PatientDetails = UnAvailableCaseN.PatientDetails,
                                    ReasonUnavailableID = UnAvailableCaseN.ReasonID,
                                    IsActive = true
                                };
                                ctxIns.UnavailableCaseNotes.Add(dt);
                                ctxIns.SaveChanges();
                            }
                        }

                        dbContextTransactionIns.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransactionIns.Rollback();
                        throw ex;
                    }
                    unAvailabelCaseNotes.AuditID = 0;
                }
            }

            int count = findremainigAudit(auditid);
            return count;
        }

        public int findremainigAudit(int auditID)
        {
            int AuditNumber;
            using (var ctxSelect = new Model.CNAEntities())
            {
                AuditNumber = ctxSelect.AuditClinicAnswers.Where(x => x.AuditID == auditID && x.IsActive && x.StatusID != 4).Select(x => x.ClinicCode).Count();
            }

            return AuditNumber;
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

        public void DeleteAudit(int AuditID, string StatusID)
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

        public AuditClinicAnswersBO GetAuditClincAnswer(int cSAAuditId)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.AuditClinicAnswers
                        where u.IsActive
                        && u.AuditClinicAnswersID == cSAAuditId
                        select new BusinessObjects.AuditClinicAnswersBO
                        {
                            AuditClinicAnswersID = u.AuditClinicAnswersID,
                            AuditID = u.AuditID,
                            ClinicCode = u.ClinicCode,
                            NumberOfAppointmentsAllocated = u.NumberOfAppointmentsAllocated,
                            CaseNotesAvailableStartCount = u.CaseNotesAvailableStartCount,
                            TemporaryNotesCount = u.TemporaryNotesCount,
                            IsActive = u.IsActive
                        }).FirstOrDefault();
            }
        }
    }
}



