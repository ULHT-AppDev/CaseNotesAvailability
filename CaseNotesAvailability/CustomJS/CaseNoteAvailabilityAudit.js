function onDismissalDateChanged(s, e) {
           var contactLayoutGroup = CaseNoteAvailabilityAuditRecordsGridView.GetEditFormLayoutItemOrGroup("groupContactInfo");
            var isContactLayoutGroupVisible = contactLayoutGroup.GetVisible();
            //s.SetText(isContactLayoutGroupVisible ? "Show Details..." : "Hide Details");
            contactLayoutGroup.SetVisible(!isContactLayoutGroupVisible);
        }
    function AuditorView_ClientClick(s, e,AuditAnswerID, AuditID) 
    {
        //ASPxPopupControl
          if (!CaseNoteAvailabilityUnAvailabilityCallbackPanel.InCallback()) {
        
        CaseNoteAvailabilityUnAvailabilityCallbackPanel.PerformCallback(AuditAnswerID,AuditID);
        CaseNoteAvailabilityUnAvailabilityPopup.Show();
    }

        }
        function Complete_Click(s, e)
        {
        }

        function CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback (s,e)
        {
        
        }


      function UnavailableCaseNoteCount_SelectedIndexChanged(s, e) {

    var Count = UnavailableCaseNoteCount.GetValue();
    if (Count > 0) {
       // int i= 0;
        //While (i<=count)
        //{
            //i+=1;
            CreateFormDynamically_CallbackPanel.PerformCallback(Count);    
            CreateFormDynamically_CallbackPanel.SetVisible(true);
            //PatientLayoutCallLayout.SetVisible(true);
        //}
        }
        else
        {
            CreateFormDynamically_CallbackPanel.SetVisible(false);
        }
    } 

    //BodyMassIndexCalculatedLabel.SetText(valueToSet);
    //document.getElementById('HiddenBMIValue').value = valueToSet;

