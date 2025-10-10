function TotalAppointments_ValueChanged(s, e) {

    if (s.GetValue() != null) {
        CheckAllCountsValidation();
    }

}


function ActualAppointments_ValueChanged(s, e) {
    if (s.GetValue() != null) {
        CheckAllCountsValidation();
    }
}


function StartCount_Validation(s, e) {

    e.isValid = true;

    let startCount = s.GetValue();

    if (startCount !== null) {

        let ActualAppointment = txtActualAppointments.GetValue();

        if (ActualAppointment !== null) {
            if (startCount > ActualAppointment) {
                e.isValid = false;
                e.errorText = "Case notes available at the start of clinic can't be greater than Actual Appointments.";
            }

            CheckAllCountsValidation();
        }
    }
    else {
        e.isValid = false; // required field
    }
}


function TempCount_Validation(s, e) {

    e.isValid = true;

    let tempCount = txtTempNotesCount.GetValue();

    if (tempCount !== null) {

        let ActualAppointment = txtActualAppointments.GetValue();

        if (ActualAppointment !== null) {

            if (tempCount > ActualAppointment) {
                e.isValid = false;
                e.errorText = "Temp Count can't be greater than Actual Appointments.";
            }

            CheckAllCountsValidation();
        }
    } else {
        e.isValid = false; // required field
    }

}

//function CheckAllCountsValidation(e) {
function CheckAllCountsValidation() {

    CaseNoteFormLayout.GetItemByName("SumErrorLabelItem").SetVisible(false);

    let actualAppointment = txtActualAppointments.GetValue();
    let startCount = txtStartCount.GetValue();
    let tempAppointment = txtTempNotesCount.GetValue();
    let unavailableCount = txtUnavailableCaseNoteCount.GetValue();

    if (unavailableCount !== null && actualAppointment !== null && tempAppointment !== null && startCount !== null) {

        let total = unavailableCount + tempAppointment + startCount;

        if (total > actualAppointment) {
            // total of all 3 cant be bigger than actual

            CaseNoteFormLayout.GetItemByName("SumErrorLabelItem").SetVisible(true);
            return false;
            //e.isValid = false;
            //e.errorText = "The sum of start count, temporary count & unavailable count can not be greater than actual appointments.";
        }
    }

    return true;
}

function UnavailableCaseNoteCount_Validation(s, e) {

    e.isValid = true;

    let unavailableCount = txtUnavailableCaseNoteCount.GetValue();

    if (unavailableCount !== null) {

        let ActualAppointment = txtActualAppointments.GetValue();

        if (ActualAppointment !== null) {

            if (unavailableCount > ActualAppointment) {
                e.isValid = false;
                e.errorText = "Number of case notes unavailable can't be greater than Actual Appointments.";
            }

            CheckAllCountsValidation();
        }
    } else {
        e.isValid = false; // required field
    }
}

let tempDataStorage = [];

function txtUnavailableCaseNoteCount_ValueChanged(s, e) {

    let unavailableCount = s.GetValue();

    if (unavailableCount !== null && unavailableCount > 0) {



        // disable complete button
        CompleteButton.SetEnabled(false);


        // set temp data storage
        tempDataStorage = []; // reset

        for (var i = 1; i <= unavailableCount; i++) {

            var Namee = "PatientNameTextBox_" + i;
            var PatientNameTextBox = ASPxClientTextBox.Cast(Namee);

            // may be higher than
            if (PatientNameTextBox !== undefined) {
                var PatientNametVal = PatientNameTextBox.GetText();
                var Reason = "ReasonComboBox_" + i;
                var ReasonTextBox = ASPxClientComboBox.Cast(Reason);
                var ReasonVal = ReasonTextBox.GetValue();

                let PatientDetails = {
                    ID: i,
                    PatientDetails: PatientNametVal,
                    ReasonID: ReasonVal
                };

                tempDataStorage.push(PatientDetails);
            }
        }

        let obj = {
            ActionID: 1,
            NumberofRows: unavailableCount
            /*TempData: tempDataStorage*/
        };

        CreateFormDynamically_CallbackPanel.PerformCallback(JSON.stringify(obj));
        //CreateFormDynamically_CallbackPanel.SetVisible(true);
        obj = null;
    }
    else {
        // clear datasource and hide tab pages
        if (!unavailableCount <= 0)
            PageControl.SetVisible(false);

    }
}

function CreateFormDynamically_CallbackPanel_EndCallback(s, e) {
    if (s.cpGeneratedItemsSuccess) {
        delete s.cpGeneratedItemsSuccess;
        CompleteButton.SetEnabled(true);

        // reset values
        if (tempDataStorage !== null) {
            for (var x = 0; x < tempDataStorage.length; x++) {

                var currentItem = tempDataStorage[x];

                var id = currentItem.ID;
                var currentPatientTextBox = "PatientNameTextBox_" + id;
                var currentPatientTextBoxControl = ASPxClientTextBox.Cast(currentPatientTextBox);

                // try find textbox again

                if (currentPatientTextBoxControl !== undefined) {
                    currentPatientTextBoxControl.SetText(currentItem.PatientDetails);

                    var Reason = "ReasonComboBox_" + id;
                    var ReasonTextBox = ASPxClientComboBox.Cast(Reason);
                    ReasonTextBox.SetValue(currentItem.ReasonID);
                }
            }
        }
    }

    if (s.cpGeneratedItemsError) {
        delete s.cpGeneratedItemsError;
        // handle error 
        alert("error please contact ICT support");

    }
}


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

function AuditDetails_Click(s, e) {
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
function AuditValidate(s, e) {
    var isValid1 = ASPxClientEdit.ValidateGroup("CaseNoteVal");
    if (!isValid1) return false;
}




function Complete_Click(s, e) {
    // check form and custom validation CheckAllCountsValidation()
    if (ASPxClientEdit.ValidateEditorsInContainerById("CaseNoteAvailabilityUnAvailabilityPopupContainer", "CaseNoteVal", true) && CheckAllCountsValidation() && !CaseNoteAvailabilityAuditRecordsGridView.InCallback()) {

        let arr = [];
        var UnavailableCaseNoteCount1 = txtUnavailableCaseNoteCount.GetValue();

        for (var i = 1; i <= UnavailableCaseNoteCount1; i++) {

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








//}



//BodyMassIndexCalculatedLabel.SetText(valueToSet);
//document.getElementById('HiddenBMIValue').value = valueToSet;

