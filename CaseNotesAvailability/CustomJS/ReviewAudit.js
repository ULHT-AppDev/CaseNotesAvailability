/// <reference path="../devexpress-web.d.ts" />

function onDismissalDateChanged(s, e) {
    var contactLayoutGroup = ReviewAuditRecordsGridView.GetEditFormLayoutItemOrGroup("groupContactInfo");
    var isContactLayoutGroupVisible = contactLayoutGroup.GetVisible();
    //s.SetText(isContactLayoutGroupVisible ? "Show Details..." : "Hide Details");
    contactLayoutGroup.SetVisible(!isContactLayoutGroupVisible);
}

function AuditorView_ClientClick(s, e, ClinicCode, AuditClinicAnswerId, AuditID, nIndex) {
    //ASPxPopupControl
    /* if (!CaseNoteAvailabilityUnAvailabilityCallbackPanel.InCallback())
     {
 
         let callbackString = {
         ClinicCode: ClinicCode,
         AuditClinicAnswerId: AuditClinicAnswerId,
         AuditID:AuditID
     };*/
    ReviewAuditRecordsGridView.StartEditRow(nIndex);
    //CaseNoteAvailabilityUnAvailabilityCallbackPanel.PerformCallback(JSON.stringify(callbackString));
    //CaseNoteAvailabilityUnAvailabilityPopup.Show();
    // }

}

let currentClinicCode = null;
let reviewSender = null;

function AddImpDetails_ClientClick(s, e, ClinicCode,reviewSender1) {

    currentClinicCode = ClinicCode;
   // reviewSender = 1; // this would need adding - send from
   reviewSender = reviewSender1;

    if (reviewSender === 1) {

        AddReviewFormLayout.GetItemByName("ImprovementDetails").SetVisible(true);
        AddReviewFormLayout.GetItemByName("ActionPointsReview").SetVisible(false);

    } else if (reviewSender === 2) {
        AddReviewFormLayout.GetItemByName("ActionPointsReview").SetVisible(true);
        AddReviewFormLayout.GetItemByName("ImprovementDetails").SetVisible(false);

    }



    AddReviewPopup.Show();
    
    //ReviewAuditClinicsGridView.AddNewRow();

}

let InMemoryImprovementDetailsDS = []; // define in memory array
let InMemoryImprovementDetailsCounter = 0;
let InMemoryActionDetailsDS = []; 
let InMemoryActionDetailsCounter = 0;

function SubmitReviewButton_Click(s, e) {

    if (reviewSender == 1) {

        if (ASPxClientEdit.AreEditorsValid("AddReviewContainer", "SubmitReview", false) && !ImprovementDetailsGridView.InCallback()) {

           // var comboBox = ASPxClientControl.GetControlCollection().GetByName("UnavailableReasonComboBox");

            let newItem = {
                RequiresImprovementDetailsID: InMemoryImprovementDetailsCounter++,
                ImprovementDetailID: ImpReasonComboBox.GetText(),
                Comment: ReviewCommentMemo.GetText()                
            };

            InMemoryImprovementDetailsDS.push(newItem);

            ImprovementDetailsGridView.PerformCallback(JSON.stringify(InMemoryImprovementDetailsDS));
        }


    } else if (reviewSender == 2) {
        // send to other grid
        if (ASPxClientEdit.AreEditorsValid("AddReviewContainer", "SubmitReview", false) && !ActionPointDetailsGridView.InCallback()) {

           // var comboBox = ASPxClientControl.GetControlCollection().GetByName("UnavailableReasonComboBox");

            let newItem = {
                RequiresImprovementDetailsID: InMemoryActionDetailsCounter++,
                ImprovementDetailID: ActReasonComboBox.GetText(),
                Comment: ReviewCommentMemo.GetText()                
            };

            InMemoryActionDetailsDS.push(newItem);

            ActionPointDetailsGridView.PerformCallback(JSON.stringify(InMemoryActionDetailsDS));
    }

}
}
function CloseReviewButton_click(s, e) 
{
    AddReviewPopup.Hide();
      
}


function DeleteImprovementReview_Click(s, e, id) {

    if (!ImprovementDetailsGridView.InCallback()) {

        // would add in a popup here to confirm removal etc 
        InMemoryImprovementDetailsDS = InMemoryImprovementDetailsDS.filter(item => item.RequiresImprovementDetailsID != id); // no type match on check - could change this

        ImprovementDetailsGridView.PerformCallback(JSON.stringify(InMemoryImprovementDetailsDS));
    }
}
    function DeleteActionReview_Click(s, e, id) {

    if (!ActionPointDetailsGridView.InCallback()) {

        // would add in a popup here to confirm removal etc 
        InMemoryActionDetailsDS = InMemoryActionDetailsDS.filter(item => item.RequiresImprovementDetailsID != id); // no type match on check - could change this

        ActionPointDetailsGridView.PerformCallback(JSON.stringify(InMemoryActionDetailsDS));
    }

}

function ImprovementDetailsGridView_EndCallback(s, e) {

    if (s.cpDataBound) {
        delete s.cpDataBound;
        ASPxClientEdit.ClearGroup("SubmitReview", true);
        AddReviewFormLayout.GetItemByName("ImprovementDetails").SetVisible(false);
        AddReviewFormLayout.GetItemByName("ActionPointsReview").SetVisible(false);
        AddReviewPopup.Hide();
    }

    if (s.cpShowGrid) {
        delete s.cpShowGrid;
        NoImprovementDetailReviewLabel.SetVisible(false);
        ImprovementDetailsGridView.SetVisible(true);
    } else if (s.cpHideGrid) {
        delete s.cpHideGrid;
        ImprovementDetailsGridView.SetVisible(false);
        NoImprovementDetailReviewLabel.SetVisible(true);
        InMemoryImprovementDetailsDS = []; // reset
        InMemoryImprovementDetailsCounter = 0; // reset
    }

}


function AuditDetails_Click(s, e, ClinicCode, AuditClinicAnswerId, AuditID) {
    const relativeURL = 'default.aspx?';
    const absoluteURL = new URL(relativeURL, window.location.href);
    window.location.href = absoluteURL.href;
}

function ReviewAuditRecordsGridView_EndCallBack(s, e) {
    if (s.cpUpdated == true) {
        SetAndShowAlert(1, 'Record Updated Successfully', '');
        delete s.cpUpdated;
    }
    if (s.cpInserted == true) {
        SetAndShowAlert(1, 'Record Inserted Successfully', '');
        delete s.cpInserted;
    }
    if (s.cpPopupUpdated == true) {
        CaseNoteAvailabilityUnAvailabilityPopup.Hide();
        ReviewAuditRecordsGridView.Refresh();
        SetAndShowAlert(1, 'Record Inserted Successfully', '');
        delete s.cpPopupUpdated;
    }

}

function CompleteClient_Click(s, e) 
{
    let CallbackObj = 
    {
        ActionID : 1,
        ClinicCode:LabelRead.GetText();
        ImprovementDetailsDS:InMemoryImprovementDetailsDS,
        ActionPointsDS:InMemoryActionDetailsDS
    }
      
     var jsonArray = JSON.stringify(CallbackObj);

    //CompleteCallback.PerformCallback(jsonArray);
    ReviewAuditRecordsGridView.PerformCallback(jsonArray);

}




function ActionPointDetailsGridView_EndCallback (s, e) {
    if (s.cpDataBound) {
        delete s.cpDataBound;
        ASPxClientEdit.ClearGroup("SubmitReview", true);
        AddReviewFormLayout.GetItemByName("ImprovementDetails").SetVisible(false);
        AddReviewFormLayout.GetItemByName("ActionPointsReview").SetVisible(false);
        AddReviewPopup.Hide();
    }

    if (s.cpShowGrid) {
        delete s.cpShowGrid;
        NoActionPointDetailReviewLabel.SetVisible(false);
        ActionPointDetailsGridView.SetVisible(true);
    } else if (s.cpHideGrid) {
        delete s.cpHideGrid;
        ActionPointDetailsGridView.SetVisible(false);
        NoActionPointDetailReviewLabel.SetVisible(true);
        InMemoryActionDetailsDS = []; 
        InMemoryActionDetailsCounter = 0;
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




function Complete_Click(s, e) {
    let arr = [];
    var UnavailableCaseNoteCount1 = UnavailableCaseNoteCount.GetValue();
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

        //arr[i - 1][0] = PatientNameTextBox.GetText();
        //arr[i - 1][1] = ReasonTextBox.GetValue();

    }

    var jsonArray = JSON.stringify(arr);

    //CompleteCallback.PerformCallback(jsonArray);
    ReviewAuditRecordsGridView.PerformCallback(jsonArray,);
}

function CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback(s, e) {

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
    else {
        CreateFormDynamically_CallbackPanel.SetVisible(false);
    }
}

//BodyMassIndexCalculatedLabel.SetText(valueToSet);
//document.getElementById('HiddenBMIValue').value = valueToSet;

