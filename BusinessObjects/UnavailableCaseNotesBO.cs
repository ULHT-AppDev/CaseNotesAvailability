﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UnavailableCaseNotesBO
    {
        public int UnavailableCaseNotesID { get; set; }
        public int AuditClinicAnswersID { get; set; }
        public string PatientDetails { get; set; }
        public int ReasonUnavailableID { get; set; }
        public bool IsActive { get; set; }
    }
}
