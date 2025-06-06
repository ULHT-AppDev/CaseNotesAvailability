﻿using System;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class AuditClinicAnswersUnAvailableBO
    {
        public int AuditClinicAnswersID { get; set; }
        public int AuditID { get; set; }
        public string ClinicCode { get; set; }
        public Nullable<int> Totalappointments { get; set; }
        public Nullable<int> NumberOfAppointmentsAllocated { get; set; }
        public Nullable<int> CaseNotesAvailableStartCount { get; set; }
        public Nullable<int> TemporaryNotesCount { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> UnavailableCount { get; set; }
        public List<CompleteCallbackBO> UnavailableList { get; set; }
    }
}
