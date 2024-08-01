using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public  class AuditBO
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
    }
}
