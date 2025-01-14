using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AuditClinicAnswersBO
    {
        public int AuditClinicAnswersID { get; set; }
        public int AuditID { get; set; }
        public string ClinicCode { get; set; }
        public Nullable<int> Totalappointments { get; set; }
        public Nullable<int> NumberOfAppointmentsAllocated { get; set; }
        public Nullable<int> CaseNotesAvailableStartCount { get; set; }
        public Nullable<int> TemporaryNotesCount { get; set; }
        public bool IsActive { get; set; }
        public byte StatusID { get; set; }
    }
}
