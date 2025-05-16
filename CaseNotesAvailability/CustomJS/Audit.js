function HealthRecordsGridView_EndCallBack(s, e) {
    if (s.cpUpdated == true) {
        SetAndShowAlert(1, 'Record Updated Successfully', '');
        delete s.cpUpdated;
    }
    if (s.cpInserted == true) {
        SetAndShowAlert(1, 'Record Inserted Successfully', '');
        delete s.cpInserted;
    }
    if (s.cpDeleted == true) {
        SetAndShowAlert(1, 'Record Deleted Successfully', '');
        HealthRecordsGridView.Refresh();
        delete s.cpDeleted;
    }

}

function EditRow_Click(s, e, AuditID, Index) {
    HealthRecordsGridView.StartEditRow(Index);

}
function DeleteRow_Click() {
    HealthRecordsGridView.Refresh();

}

function ChooseUserButton_Click(s, e, AuditID, StatusID) {
}


function NewRef_Init(s, e) {
    HealthRecordsGridView.AddNewRow();
}

function AuditorView_Click(s, e, AuditID) {
    //const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID='+AuditID+'&Speciality='+Speciality;
    const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID=' + AuditID;
    const absoluteURL =
        new URL(relativeURL, window.location.href);
    //      console.log('Redirecting to:', absoluteURL.href);
    window.location.href = absoluteURL.href;

}
function Send_for_review(s, e, AuditID) {
    //const relativeURL = 'CaseNoteAvailabilityAudit.aspx?AuditID='+AuditID+'&Speciality='+Speciality;
    const relativeURL = 'ReviewAudit.aspx?AuditID=' + AuditID;
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
//delete 

function ShowDeletePopup(values) {

    var text = "Are you sure you want to delete Audit ID: <strong>" + values + "</strong>?";

    DeleteAuditID = values;
    DeleteCaseNoteConfirmationLabel.SetText(text);;

    DeleteCaseNotePopup.Show();
}

function CancelDeleteButton_Click(s, e) {
    DeleteCaseNotePopup.Hide();
}

function DeleteButton_Click(s, e, GridIndex) {
    HealthRecordsGridView.GetRowValues(GridIndex, 'AuditID', ShowDeletePopup);

    DeleteCaseNotePopup.Show();
}



function DeleteThisCaseNoteButton_Click(s, e) {
    if (!HealthRecordsGridView.InCallback()) {


        let obj = {
            ActionID: 1,
            AuditID: DeleteAuditID
        };
        DeleteCaseNotePopup.Hide();
        HealthRecordsGridView.PerformCallback(JSON.stringify(obj));
    }
}

function HealthRecordsGridView_CustomButtonClick(s, e) {

    if (e.buttonID === "DeleteCaseNoteButton") {

        s.GetRowValues(e.visibleIndex, 'UserID;Forename;Surname', ShowDeletePopup);

        DeleteCaseNotePopup.Show();
    }
}
/*
function CheckDueByDate(s, e) 
{
     s.GetRowValues(e.visibleIndex, 'DueByDate;Date',onCallbackMultiValues);

    }
    function onCallbackMultiValues(values) {
    alert(values[0]);
}
*/
function OnTokenBoxValueChanged(s, e) {
    var values = s.GetValue().split(',');
    var uniqueValues = [];
    for (var i = 0; i < values.length; i++) {
        var trimmed = values[i].trim();
        if (trimmed !== "" && uniqueValues.indexOf(trimmed) === -1) {
            uniqueValues.push(trimmed);
        }
    }
    s.SetValue(uniqueValues.join(','));
}



