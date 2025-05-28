using System;

namespace BusinessObjects
{
    public class SingleAuditBO
    {
        public int AuditID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public String Specialty { get; set; }
        public String Site { get; set; }
        public int StatusID { get; set; }

    }
}
