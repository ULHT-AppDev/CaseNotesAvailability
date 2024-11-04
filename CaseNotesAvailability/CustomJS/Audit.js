function HealthRecordsGridView_EndCallBack(s,e)
{
    if (s.cpUpdated == true)
    {
        SetAndShowAlert(1,'Record Updated Successfully','');
        delete s.cpUpdated;
    }
    if (s.cpInserted == true)
    {
        SetAndShowAlert(1,'Record Inserted Successfully','');
        delete s.cpInserted;
      }
      
    
}

function EditRow_Click(s, e,AuditID,Index) {
   HealthRecordsGridView.StartEditRow(Index);
   
}
function DeleteRow_Click() {
   HealthRecordsGridView.Refresh();
   
}

function ChooseUserButton_Click(s, e, AuditID, StatusID) {
    }


function NewRef_Init(s,e)
{
    HealthRecordsGridView.AddNewRow();
}

function AuditorView_Click(s, e,AuditID) {
     //const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID='+AuditID+'&Speciality='+Speciality;
     const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID='+AuditID;
         const absoluteURL = 
         new URL(relativeURL, window.location.href);
//      console.log('Redirecting to:', absoluteURL.href);
        window.location.href = absoluteURL.href;
   
}
function Send_for_review(s, e,AuditID) {
     //const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID='+AuditID+'&Speciality='+Speciality;
     const relativeURL = 'ReviewAudit.aspx?AuditID='+AuditID;
         const absoluteURL = 
         new URL(relativeURL, window.location.href);
//      console.log('Redirecting to:', absoluteURL.href);
        window.location.href = absoluteURL.href;
}



function ChooseUserButton1_Click(s, e, AuditID, StatusID) {
    let personName;

    //let personName = forename + " " + surname + " (" + username + ")";
    /*
    switch (ADSender) {
        case "Expedite":
        case "Expedite_Existing":
        case "CreateReferral":
        case "CreateReferral_Existing":

            GradedByUserSelectedPanel.SetVisible(true);
            GradedByUserLabel.SetText(personName);

            break;

        case "CreateReferralExpedite":
        case "CreateReferralExpedite_Existing":

            ExpediteGradedByUserSelectedPanel.SetVisible(true);
            ExpediteGradedByUserLabel.SetText(personName);

            break;
    }

    GradedByUserHiddenField.Set("GradedByUserID", personID);
    ResetCreatePopup();
    GradedUserPopup.Hide();
    */


}