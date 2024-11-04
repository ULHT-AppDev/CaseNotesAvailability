using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AuditClinicAnswersUnAvailableBO
    {
        public int AuditClinicAnswersID { get; set; }
        public int AuditID { get; set; }
        public string ClinicCode { get; set; }
        public Nullable<int> NumberOfAppointmentsAllocated { get; set; }
        public Nullable<int> CaseNotesAvailableStartCount { get; set; }
        public Nullable<int> TemporaryNotesCount { get; set; }
        public bool IsActive { get; set; }
        public List<CompleteCallbackBO> Unavailable {  get; set; }  
    }
}
