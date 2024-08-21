

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