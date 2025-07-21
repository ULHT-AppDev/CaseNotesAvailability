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
    public class UnavailableCaseNotesReasonBO
    {
        public int UnavailableCaseNotesID { get; set; }
        public int AuditClinicAnswersID { get; set; }
        public string PatientDetails { get; set; }
        public string ReasonUnavailable { get; set; }
        public bool IsActive { get; set; }
    }
}
