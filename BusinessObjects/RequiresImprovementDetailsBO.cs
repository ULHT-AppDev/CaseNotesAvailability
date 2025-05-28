using System;

namespace BusinessObjects
{
    public class RequiresImprovementDetailsBO
    {

        public int RequiresImprovementDetailsID { get; set; }
        public string ClinicCode { get; set; }
        public Nullable<int> ImprovementReasonID { get; set; }
        public string Comment { get; set; }
        public int ReviewedByUserID { get; set; }
        public Nullable<System.DateTime> ReviewedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
