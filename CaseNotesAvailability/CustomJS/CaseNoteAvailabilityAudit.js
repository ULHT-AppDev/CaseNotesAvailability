function onDismissalDateChanged(s, e)
{
    var contactLayoutGroup = CaseNoteAvailabilityAuditRecordsGridView.GetEditFormLayoutItemOrGroup("groupContactInfo");
    var isContactLayoutGroupVisible = contactLayoutGroup.GetVisible();
    //s.SetText(isContactLayoutGroupVisible ? "Show Details..." : "Hide Details");
    contactLayoutGroup.SetVisible(!isContactLayoutGroupVisible);
}
function AuditorView_ClientClick(s, e, ClinicCode, AuditClinicAnswerId,AuditID)
{
    //ASPxPopupControl
    if (!CaseNoteAvailabilityUnAvailabilityCallbackPanel.InCallback())
    {

        let callbackString = {
        ClinicCode: ClinicCode,
        AuditClinicAnswerId: AuditClinicAnswerId,
        AuditID:AuditID
    };

CaseNoteAvailabilityUnAvailabilityCallbackPanel.PerformCallback(JSON.stringify(callbackString));
CaseNoteAvailabilityUnAvailabilityPopup.Show();
    }

        }

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

  

    var jsonArray = JSON.stringify(arr);

    CompleteCallback.PerformCallback(jsonArray);
}

function CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback(s, e)
{

}


function UnavailableCaseNoteCount_SelectedIndexChanged(s, e)
{

    var Count = UnavailableCaseNoteCount.GetValue();
    if (Count > 0)
    {
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

