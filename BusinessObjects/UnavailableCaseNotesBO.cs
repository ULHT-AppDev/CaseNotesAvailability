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
