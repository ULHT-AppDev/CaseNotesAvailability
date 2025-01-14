using BusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ReviewAuditDAL
    {
        public List<RequiresImprovementDetailsBO> GetImprovementDetails()
        {

            using (var ctx = new Model.CNAEntities())
            {
                //return ctx.Applications.Where(x => x.IsActive).Select(x => new audit
                return (from p in ctx.RequiresImprovementDetails
                        where p.IsActive
                        select new BusinessObjects.RequiresImprovementDetailsBO
                        {
                            RequiresImprovementDetailsID = p.RequiresImprovementDetailsID,
                           // ClinicCode = p.ClinicCode,
                            ImprovementReasonID = p.ImprovementReasonID,
                            Comment = p.Comment,
                            ReviewedByUserID = p.ReviewedByUserID,
                            ReviewedDate = p.ReviewedDate
                        }).ToList();
            }
        }



        public void UpdateImprovementDetails(RequiresImprovementDetailsBO details)
        {
            using (var ctxSelect = new Model.CNAEntities())
            {
                try
                {
                    using (var ctxUpdate = new Model.CNAEntities())
                    {
                        using (var dbContextTransactionIns = ctxUpdate.Database.BeginTransaction())
                        {
                            try
                            {
                                //List<AuditBO> audit = new List<AuditBO>();
                                var audit1 = ctxUpdate.RequiresImprovementDetails.Where(x => x.RequiresImprovementDetailsID == details.ImprovementReasonID).FirstOrDefault();
                                audit1.ImprovementReasonID = details.ImprovementReasonID;
                                audit1.Comment = details.Comment;
                                audit1.ReviewedByUserID = details.ReviewedByUserID;
                                audit1.ReviewedDate = details.ReviewedDate;
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
                catch (Exception ex)
                {

                }

            }
        }

        public void InsertImprovementDetails(RequiresImprovementDetailsBO details)
        {

            using (var ctxIns = new Model.CNAEntities())
            {
                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {
                        if (details.ImprovementReasonID != null)
                        {
                            Model.RequiresImprovementDetail dt = new Model.RequiresImprovementDetail()
                            {

                                ImprovementReasonID = details.ImprovementReasonID,
                                Comment = details.Comment,
                                ReviewedByUserID = details.ReviewedByUserID,
                                ReviewedDate = details.ReviewedDate,
                                IsActive = true

                            };
                            ctxIns.RequiresImprovementDetails.Add(dt);
                            ctxIns.SaveChanges();
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

        public List<ReasonUnavailableBO> GetUnavailableReason()
        {

            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.ReasonUnavailables
                        where u.IsActive
                        select new BusinessObjects.ReasonUnavailableBO
                        {
                            ReasonUnavailableID = u.ReasonUnavailableID,
                            ReasonText = u.ReasonText
                        }).ToList();
            }


        }

        public void ImprovementDetailsCallbackUpdate(List<ImprovementDetailsCallbackBO> ImprovementDetailsCallback, string clinicCode, int userID)
        {
            using (var ctxIns = new Model.CNAEntities())
            {
                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {
                        if (ImprovementDetailsCallback.Count != 0)
                        {
                            foreach (ImprovementDetailsCallbackBO SingleImprovementDetailsCallback in ImprovementDetailsCallback)
                            {
                                Model.RequiresImprovementDetail dt = new Model.RequiresImprovementDetail()
                                {
                                   // AuditClinicAnswersID = clinicCode,
                                    ImprovementReasonID = SingleImprovementDetailsCallback.ImprovementDetailID,
                                    Comment = SingleImprovementDetailsCallback.Comment,
                                    ReviewedByUserID= userID,
                                    ReviewedDate=DateTime.Now,
                                    IsActive = true

                                };
                                ctxIns.RequiresImprovementDetails.Add(dt);
                            }
                            ctxIns.SaveChanges();
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

        public void ImprovementActionDetailsCallbackUpdate(List<ActionDetailsCallbackBO> actionDetailsCallback, string clinicCode, short userID)
        {
            using (var ctxIns = new Model.CNAEntities())
            {
                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {
                        if (actionDetailsCallback.Count != 0)
                        {
                            foreach (ActionDetailsCallbackBO SingleImprovementActionDetailsCallback in actionDetailsCallback)
                            {
                                Model.RequiresImprovementActionPoint dt = new Model.RequiresImprovementActionPoint()
                                {
                                    //ClinicCode = clinicCode,
                                    //ImprovementReasonID = SingleImprovementDetailsCallback.ImprovementDetailID,
                                    //ActionPointComment = SingleImprovementActionDetailsCallback.Comment,
                                    //ReviewedByUserID = userID,
                                    //ReviewedDate = DateTime.Now,
                                    //IsActive = true

                                };
                                //ctxIns.RequiresImprovementDetails.Add(dt);
                            }
                            ctxIns.SaveChanges();
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

        public void UpdateImprovementActionDetails(short userID,UpdateImprovementActionCallbackBO updateImprovementAction)
        {

            using (var ctxIns = new Model.CNAEntities())
            {
                using (var dbContextTransactionIns = ctxIns.Database.BeginTransaction())
                {
                    try
                    {

                        if (updateImprovementAction.ActionID != 0)
                        {
                            foreach (ImprovementDetailsCallbackBO SingleImprovementDetailsCallback in updateImprovementAction.ImprovementDetailsDS)
                            {
                                Model.RequiresImprovementDetail dt = new Model.RequiresImprovementDetail()
                                {
                                    AuditClinicAnswersID = updateImprovementAction.AuditClinicAnswersID,
                                    ImprovementReasonID = SingleImprovementDetailsCallback.ImprovementDetailID,
                                    Comment = SingleImprovementDetailsCallback.Comment,
                                    ReviewedByUserID = userID,
                                    ReviewedDate = DateTime.Now,
                                    IsActive = true
                                };
                                ctxIns.RequiresImprovementDetails.Add(dt);
                                ctxIns.SaveChanges();
                            }
                            foreach (ActionDetailsCallbackBO ActionDetailsCallback in updateImprovementAction.ActionPointsDS)
                            {
                                Model.RequiresImprovementActionPoint dt = new Model.RequiresImprovementActionPoint()
                                {
                                    AuditClinicAnswersID = updateImprovementAction.AuditClinicAnswersID,
                                    ActionPointComment = ActionDetailsCallback.Comment,
                                    ReviewedByUserID = userID,
                                    ReviewedDate = DateTime.Now,
                                    IsActive = true
                                };
                                ctxIns.RequiresImprovementActionPoints.Add(dt);
                            }

                            ctxIns.SaveChanges();
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
