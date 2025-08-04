using System;

namespace BusinessObjects
{
    public class AuditBO
    {
        public int AuditID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int SpecialtyID { get; set; }
        public int SiteID { get; set; }
        public int CreatedByUserID { get; set; }
        public int CompletedByUserID { get; set; }
        public Nullable<System.DateTime> DueByDate { get; set; }
        public bool IsActive { get; set; }
        public string ClinicCodes { get; set; }
        public int StatusID { get; set; }
        public int SessionID { get; set; }
        public int Sortorder { get => getSororder(); }

        private int getSororder()
        {
            switch (StatusID)
            {
                case (byte)Enums.AuditStatus.NotStarted:
                    return (byte)Enums.AuditStatus.NotStarted;
                case (byte)Enums.AuditStatus.InProgress:
                    return (byte)Enums.AuditStatus.InProgress;
                case (byte)Enums.AuditStatus.Completed:
                    return (byte)Enums.AuditStatus.Completed;
                case (byte)Enums.AuditStatus.PendingHRreview:
                    return (byte)Enums.AuditStatus.PendingHRreview;
                default:
                    return 0;
            }
        }
    }

    public class AuditDeleteBO
    {
        public int ActionID { get; set; }
        public int AuditID { get; set; }
    }

    public class DynamicFormBO
    {
        public int ActionID { get; set; }
        public int NumberofRows { get; set; }
    }


}
