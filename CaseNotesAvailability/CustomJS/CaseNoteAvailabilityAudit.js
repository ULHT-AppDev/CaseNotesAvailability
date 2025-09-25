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

function CloseButton_Init(s, e) {
    CaseNoteAvailabilityUnAvailabilityPopup.Hide();
}

function TempCount_Validation(s, e) {
    if (ASPxClientEdit.AreEditorsValid("CaseNoteAvailabilityUnAvailabilityPopupContainer", "CaseNoteVal", false))
    {
        if (s.GetText() !== null && s.GetText() !== "") {
            let Count = txtTempNotesCount.GetValue();
            let ActualAppointment = txtActualAppointments.GetValue();

            if (Count !== null) {

                if (Count > ActualAppointment) {
                    e.isValid = false;
                    e.errorText = "Temp Count can't be greater than Actual Appointment.";
                }
                else {
                    e.isValid = true;
                }
            }
        }
    }

}

function popup_Closing(s, e) {
    ASPxClientEdit.ClearGroup('CaseNoteVal', true);
    let obj = {
        ActionID: 0,
        NumberofRows: 0
    };
    //Action nothing to do 

    CreateFormDynamically_CallbackPanel.PerformCallback(JSON.stringify(obj));
}


function txtTempNotesCount_ValueChanged(s, e) {


}

function AuditDetails_Click(s, e, ClinicCode, AuditClinicAnswerId, AuditID) {
    const relativeURL = 'default.aspx';
    const absoluteURL = new URL(relativeURL, window.location.href);
    window.location.href = absoluteURL.href;
}

function CaseNoteAvailabilityAuditRecordsGridView_EndCallBack(s, e) {
    if (s.cpUpdated == true) {
        SetAndShowAlert(1, 'Record updated successfully', '');
        delete s.cpUpdated;
    }
    else if (s.cpInserted == true) {
        SetAndShowAlert(1, 'Record inserted successfully', '');
        delete s.cpInserted;
    }
    else if (s.cpPopupUpdatedPending == true) {
        CaseNoteAvailabilityUnAvailabilityPopup.Hide();
        //PageControl.TabPages.Clear();
        CaseNoteAvailabilityAuditRecordsGridView.Refresh();
        SetAndShowAlert(1, 'All records inserted successfully.. Please click on Go back to audit page', '');
        delete s.cpPopupUpdatedPending;
    }
    else if (s.cpPopupUpdated == true) {
        CaseNoteAvailabilityUnAvailabilityPopup.Hide();
        //PageControl.TabPages.Clear();
        CaseNoteAvailabilityAuditRecordsGridView.Refresh();
        SetAndShowAlert(1, 'Record inserted successfully', '');
        delete s.cpPopupUpdated;
    }
}

/*

function ValidateMyGroup() {
    // Trigger client-side validation for 'MyDynamicGroup'
    var isValid = Page_ClientValidate('MyDynamicGroup');

    // Check if the validation passed
    if (isValid) {
        alert("Validation for 'MyDynamicGroup' passed!");
        // Allow the postback to occur
        return true;
    } else {
        alert("Validation for 'MyDynamicGroup' failed! Please check the fields.");
        // Prevent the postback
        return false;
    }
}


  */
function validatePrevious(prevField) {
    prevField.ClearValidation();

    if (prevField.GetText().trim() === "") {
        prevField.SetIsValid(false);
        prevField.SetErrorText("Please fill the previous field before proceeding");
    }
}

function ClearFields() {
    txtTotalAppointments.Clear();
    txtActualAppointments.Clear();
    txtStartCount.Clear();
    txtTempNotesCount.Clear();
    txtUnavailableCaseNoteCount.Clear();
    // CreateFormDynamically_CallbackPanel.Hide();

}

// Validate Dynamic values
function customValidate() {
    var isValid = ASPxClientEdit.ValidateGroup("CaseNoteVal1");
    if (!isValid) return false;
    return true;
}
function AuditValidate(s,e)
{
    var isValid1 = ASPxClientEdit.ValidateGroup("CaseNoteVal");
    if (!isValid1) return false;
}


function Complete_Click(s, e) {
    var test = customValidate();
    if (test == true) {
        //txtUnavailableCaseNoteCount.Validate();
        //if (txtUnavailableCaseNoteCount.GetIsValid()) {
            let arr = [];
            var UnavailableCaseNoteCount1 = txtUnavailableCaseNoteCount.GetValue();
            for (var i = 1; i <= UnavailableCaseNoteCount1; i++) {

                // var layout = "ContentPlaceHolder2_CaseNoteAvailabilityUnAvailabilityPopup_CreateFormDynamically_CallbackPanel_PageControl_formLayout_"+ Math.ceil(i/10);
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
    //}
    /* popup_Closing(s, e);*/
}

function CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback(s, e) {

}
function StartCount_Validation(s, e) {
    if (ASPxClientEdit.AreEditorsValid("CaseNoteAvailabilityUnAvailabilityPopupContainer", "CaseNoteVal", false))
    {
        if (s.GetText() !== null && s.GetText() !== "") {
            let Count = txtStartCount.GetValue();
            let ActualAppointment = txtActualAppointments.GetValue();

            if (Count !== null) {
                if (Count > ActualAppointment) {
                    e.isValid = false;
                    e.errorText = "Casenotes available at the start of clinic can't be greater than Actual Appointments.";
                }

            }
        }
    }
}


function UnavailableCaseNoteCount_Validation(s, e) {

    //if (ASPxClientEdit.AreEditorsValid("CaseNoteAvailabilityUnAvailabilityPopupContainer", "CaseNoteVal", false)) {
    var value = s.GetValue();

        if (s.GetText() !== null && s.GetText() !== "") {
            let Count = txtUnavailableCaseNoteCount.GetValue();
            let ActualAppointment = txtActualAppointments.GetValue();
            // let StartCount = txtStartCount.GetValue();

            if (Count !== null) {

                if (Count > ActualAppointment) {
                    e.isValid = false;
                    e.errorText = "Number of case notes unavailable can't be greater than Actual Appointments.";
                    
                }
                else if (Count < 0) {
                    e.isValid = false;
                    e.errorText = "Number of case notes unavailable must be more than or equal to 0.";
                } else {
                    //e.isValid = true;
                    Count = txtUnavailableCaseNoteCount.GetValue();
                    if (Count > 0) {
                        let obj = {
                            ActionID: 1,
                            NumberofRows: Count
                        };
                        //Action nothing to do 

                        CreateFormDynamically_CallbackPanel.PerformCallback(JSON.stringify(obj));
                        //CreateFormDynamically_CallbackPanel.PerformCallback(Count);
                        CreateFormDynamically_CallbackPanel.SetVisible(true);
                        obj = null;
                    }
                    else {
                        CreateFormDynamically_CallbackPanel.SetVisible(false);
                    }
                }
            } else {
                e.isValid = false;
                e.errorText = "Temp count must have a value.";
            }
        }
        else {
            e.isValid = false;
            e.errorText = "Temp count must have a value";
        }
    }




//}



//BodyMassIndexCalculatedLabel.SetText(valueToSet);
//document.getElementById('HiddenBMIValue').value = valueToSet;

