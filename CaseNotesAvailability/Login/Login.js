const LoginTypeEnum = Object.freeze({ "Login": 1, "ContinueRole": 2 });
let firstInitialised = false;

function OnLoginClick(s, e) {

    if (ASPxClientEdit.AreEditorsValid()) {
        if (FullLoginErrorLabel.GetText() !== "") {
            FullLoginErrorLabel.SetText(""); //remove if has text
        }

        LoginLoadingPanel.Show();
        LoginButton.SetEnabled(false);
        FullLoginCallbackPanel.PerformCallback(LoginTypeEnum.Login);
    }
    else {
        FullLoginErrorLabel.SetText("<i class='fa fa-exclamation-triangle'></i> Login information is required.");
        $('#LoginDiv').effect("shake");
    }
}

function OnPageControlsInit(s, e) {
    if (ShowNoticeHiddenField.Contains("ShowPopup")) {
        let showPopup = ShowNoticeHiddenField.Get("ShowPopup");

        if (showPopup) {
            showModal();
        }

        ShowNoticeHiddenField.Remove("ShowPopup");
    }
}

//for <a> etc
function showModal() {
    $('#myModal').modal('show');
}

function ConfirmRoleClick(s, e) {
    if (ASPxClientEdit.AreEditorsValid("RoleSelect")) {
        ConfirmRoleButton.SetEnabled(false);
        RoleSelectGoBackButton.SetEnabled(false);
        let selectedRoleID = RoleChoiceCombobox.GetSelectedItem().value;
        FullLoginCallbackPanel.PerformCallback(LoginTypeEnum.ContinueRole + "|" + selectedRoleID);
        RolesLoginLoadingPanel.Show();
    }
    else {
        $('#RoleChoiceContainer').effect("shake");
    }
}

function FullLoginCallbackPanelEndCallback(s, e) {

    LoginLoadingPanel.Hide();
    RolesLoginLoadingPanel.Hide();

    if (s.cpError === true) {
        delete s.cpError;
        LoginButton.SetEnabled(true);
        ConfirmRoleButton.SetEnabled(true);
        RoleSelectGoBackButton.SetEnabled(true);
        $('#LoginDiv').effect("shake");
    }

    if (s.cpMultipleRoles) {
        delete s.cpMultipleRoles;
        ShowHideRoles('#RoleContent', '#LoginDiv');
    }
}

function OnGoBackClick(s, e) {
    //var g = ASPxClientComboBox.Cast();
    RoleChoiceCombobox.SetIsValid(true);
    ShowHideRoles('#LoginDiv', '#RoleContent');
}

function ShowHideRoles(elementToShow, elementToHide) {
    $(elementToHide).hide("slide", { direction: "left" }, 200);

    setTimeout(function () {
        $(elementToShow).show("slide", { direction: "left" }, 200);
    }, 250);
}


function OnControlsInitialised(s, e) {

    if (!firstInitialised) {
        var ua = window.navigator.userAgent;
        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            // IE 11 => return version number
            //var rv = ua.indexOf('rv:');
            //return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
            BetterExperiencePopup.Show();
        }
    }

    firstInitialised = true;
}