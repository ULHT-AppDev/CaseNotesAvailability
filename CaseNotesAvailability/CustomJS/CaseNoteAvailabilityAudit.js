function onDismissalDateChanged(s, e) {
    var contactLayoutGroup = CaseNoteAvailabilityAuditRecordsGridView.GetEditFormLayoutItemOrGroup("groupContactInfo");
    var isContactLayoutGroupVisible = contactLayoutGroup.GetVisible();
    //s.SetText(isContactLayoutGroupVisible ? "Show Details..." : "Hide Details");
    contactLayoutGroup.SetVisible(!isContactLayoutGroupVisible);
}
function CompleteAudit_ClientClick(s, e, ClinicCode, AuditClinicAnswerId, AuditID) {
    //ASPxPopupControl
    ClearFields();
    if (!CaseNoteAvailabilityUnAvailabilityCallbackPanel.InCallback()) {

        let callbackString = {
            ClinicCode: ClinicCode,
            AuditClinicAnswerId: AuditClinicAnswerId,
            AuditID: AuditID
        };

        CaseNoteAvailabilityUnAvailabilityCallbackPanel.PerformCallback(JSON.stringify(callbackString));
        CaseNoteAvailabilityUnAvailabilityPopup.Show();
    }

}

function CloseButton_Init(s, e) 
{
    CaseNoteAvailabilityUnAvailabilityPopup.Hide();
    }

function TempCount_Validation(s, e) {

    if (s.GetText() !== null && s.GetText() !== "") {
        let Count = txtTempNotesCount.GetValue();
         let ActualAppointment = txtActualAppointments.GetValue();

        if (Count !== null) {

            if (Count > ActualAppointment) {
                e.isValid = false;
                e.errorText = "Temp Count can't be greater than Actual Appointment.";
            }
            else if (Count <= 0) {
                e.isValid = false;
                e.errorText = "Temp Count must be greater than  0.";
            } else {
                e.isValid = true;
            }
        } else {
            e.isValid = false;
            e.errorText = "Temp Count must have a value.";
        }
    }

}

function popup_Closing(s, e) 
{
    ASPxClientEdit.ClearGroup('CaseNoteVal',true);
    let obj = {
            ActionID:0,
            NumberofRows: 0
        };
        //Action nothing to do 

    CreateFormDynamically_CallbackPanel.PerformCallback(JSON.stringify(obj));
}


function txtTempNotesCount_ValueChanged(s, e) 
{


}

function AuditDetails_Click(s, e, ClinicCode, AuditClinicAnswerId, AuditID) {
    const relativeURL = 'default.aspx';
    const absoluteURL = new URL(relativeURL, window.location.href);
    window.location.href = absoluteURL.href;
}

function CaseNoteAvailabilityAuditRecordsGridView_EndCallBack(s, e) {
    if (s.cpUpdated == true) {
        SetAndShowAlert(1, 'Record Updated Successfully', '');
        delete s.cpUpdated;
    }
    else if (s.cpInserted == true) {
        SetAndShowAlert(1, 'Record Inserted Successfully', '');
        delete s.cpInserted;
    }
    else if (s.cpPopupUpdatedPending == true) {
        CaseNoteAvailabilityUnAvailabilityPopup.Hide();
        CaseNoteAvailabilityAuditRecordsGridView.Refresh();
        SetAndShowAlert(1, 'All Records Inserted Successfully.. Please click on Go back to Audit page', '');
        delete s.cpPopupUpdatedPending;
    }
    else if (s.cpPopupUpdated == true) {
        CaseNoteAvailabilityUnAvailabilityPopup.Hide();
        CaseNoteAvailabilityAuditRecordsGridView.Refresh();
        SetAndShowAlert(1, 'Record Inserted Successfully', '');
        delete s.cpPopupUpdated;
    }
}

/*
function Complete_Click(s, e)
{
   var arr = [];
    var UnavailableCaseNoteCount1 = UnavailableCaseNoteCount.GetValue();
    for (var i = 1; i <= UnavailableCaseNoteCount1; i++)
    {
        arr[i - 1] = [];
        var Namee = "PatientNameTextBox_" + i;
        var PatientNameTextBox = ASPxClientTextBox.Cast(Namee);
        var PatientNametVal = PatientNameTextBox.GetText();
        var Reason = "ReasonComboBox_" + i;
        var ReasonTextBox = ASPxClientComboBox.Cast(Reason);
        var ReasonVal = ReasonTextBox.GetValue();

        arr[i - 1][0] = PatientNameTextBox.GetText();
        arr[i - 1][1] = ReasonTextBox.GetValue();
    }

  */

function ClearFields()
{
    txtTotalAppointments.Clear();
 txtActualAppointments.Clear();
 txtStartCount.Clear();
 txtTempNotesCount.Clear();
 txtUnavailableCaseNoteCount.Clear();
// CreateFormDynamically_CallbackPanel.Hide();
}

function Complete_Click(s, e) {
 txtTotalAppointments.Validate();
 txtActualAppointments.Validate();
 txtStartCount.Validate();
 txtTempNotesCount.Validate();
 txtUnavailableCaseNoteCount.Validate();

 var test = txtTotalAppointments.GetIsValid() && txtActualAppointments.GetIsValid() && txtStartCount.GetIsValid() &&txtTempNotesCount.GetIsValid() && txtUnavailableCaseNoteCount.GetIsValid()  ;
 
 if(test==true)
 {
    let arr = [];
    var UnavailableCaseNoteCount1 = txtUnavailableCaseNoteCount.GetValue();
    for (var i = 1; i <= UnavailableCaseNoteCount1; i++) {

        //  arr[i - 1] = [];
        var Namee = "PatientNameTextBox_" + i;
        var PatientNameTextBox = ASPxClientTextBox.Cast(Namee);
        var PatientNametVal = PatientNameTextBox.GetText();
        var Reason = "ReasonComboBox_" + i;
        var ReasonTextBox = ASPxClientComboBox.Cast(Reason);
        var ReasonVal = ReasonTextBox.GetValue();

        let PatientDetails = {
            PatientDetails: PatientNametVal,
            ReasonID: ReasonVal
        };
        arr.push(PatientDetails);

   
    }

    var jsonArray = JSON.stringify(arr);
        CaseNoteAvailabilityAuditRecordsGridView.PerformCallback(jsonArray);
 
}
}

function CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback(s, e) {
    
}
function StartCount_Validation(s, e) 
{
    if (s.GetText() !== null && s.GetText() !== "") 
    {
        let Count = txtStartCount.GetValue();
        let ActualAppointment = txtActualAppointments.GetValue();

        if (Count !== null) 
        {
             if (Count > ActualAppointment) 
             {
                e.isValid = false;
                e.errorText = "Casenotes available at the start of clinic can't be greater than Actual Appointments.";
            }
            
        }
    }
    }

function UnavailableCaseNoteCount_SelectedIndexChanged(s, e) {

    if (s.GetText() !== null && s.GetText() !== "") {
        let Count = txtUnavailableCaseNoteCount.GetValue();
        let ActualAppointment = txtActualAppointments.GetValue();
       // let StartCount = txtStartCount.GetValue();

        if (Count !== null) {

            if (Count > ActualAppointment) {
                e.isValid = false;
                e.errorText = "Temp Count can't be greater than Actual Appointments.";
            }
            else if (Count < 0) {
                e.isValid = false;
                e.errorText = "Temp Count must be more than or equal to 0.";
            } else {
                //e.isValid = true;
                Count = txtUnavailableCaseNoteCount.GetValue();
                if (Count > 0) {
                      let obj = {
                                ActionID:1,
                                NumberofRows: Count
                                };
                                //Action nothing to do 

                        CreateFormDynamically_CallbackPanel.PerformCallback(JSON.stringify(obj));
                    //CreateFormDynamically_CallbackPanel.PerformCallback(Count);
                    CreateFormDynamically_CallbackPanel.SetVisible(true);

                }
                else {
                    CreateFormDynamically_CallbackPanel.SetVisible(false);
                }
            }
        } else {
            e.isValid = false;
            e.errorText = "Temp Count must have a value.";
        }
    }
    else {
        e.isValid = false;
        e.errorText = "Temp Count Must have a value";
    }




}

//BodyMassIndexCalculatedLabel.SetText(valueToSet);
//document.getElementById('HiddenBMIValue').value = valueToSet;

