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
                    return 1;
                case (byte)Enums.AuditStatus.InProgress:
                    return 2;
                case (byte)Enums.AuditStatus.Completed:
                    return 0;
                case (byte)Enums.AuditStatus.PendingHRreview:
                    return 3;
                default:
                    return -1;
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
