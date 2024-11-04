var alertTimeOut;

function CustomAlertPopupCloseButton_Click(s, e) {
    CustomAlertPopup.Hide();
}

/**
 * @param {number} messageType - Type of alert 0 = info, 1 = success, 2 = error, 3 = custom (include custom header)
 * @param {string} message - Message you want to show the end user in the content
 * @param {string} customHeaderMessage - Message you want to show the end user in the header only if 3 (custom) is chosen
 */
function SetAndShowAlert(messageType, message, customHeaderMessage) {

    var width = $(window).width();
    var height = $(window).height() + $(window).scrollTop();
    let toastPopup = null;

    switch (messageType) {

        case 0: // information toast

            // hide current if two actions quickly done
            if (UserToastNotification.IsVisible()) {
                clearTimeout(alertTimeOut);
                UserToastNotification.Hide();
            }

            toastPopup = $(".UserNotificationToastPopup");

            if (toastPopup.hasClass("SuccessToast")) {
                toastPopup.removeClass('SuccessToast');
            }

            if (!toastPopup.hasClass("InformationToast")) {
                toastPopup.addClass('InformationToast');
            }

            UserToastNotificationIconLabel.SetText("<i class='fas fa-info-circle'></i>");
            UserToastPopupHeaderLabel.SetText("Information");
            UserToastPopupContentLabel.SetText(message);

            setTimeout(function () { //Delay showing the alert because the page might scroll
                UserToastNotification.ShowAtPos(width, height);
            }, 100);

            break;

        case 1: // success toast

            // hide current if two actions quickly done
            if (UserToastNotification.IsVisible()) {
                clearTimeout(alertTimeOut);
                UserToastNotification.Hide();
            }

            toastPopup = $(".UserNotificationToastPopup");

            if (toastPopup.hasClass("InformationToast")) {
                toastPopup.removeClass('InformationToast');
            }

            if (!toastPopup.hasClass("SuccessToast")) {
                toastPopup.addClass('SuccessToast');
            }

            UserToastNotificationIconLabel.SetText("<i class='fas fa-check-circle'></i>");
            UserToastPopupHeaderLabel.SetText("Success");
            UserToastPopupContentLabel.SetText(message);

            setTimeout(function () { //Delay showing the alert because the page might scroll
                UserToastNotification.ShowAtPos(width, height);
            }, 100);

            break;

        case 2: // error
            CustomAlertPopupHeaderLabel.SetText("Error");
            CustomAlertPopupMessage.SetText(message + " " + "Please contact the IT Support/Service desk to raise this issue.");
            CustomAlertPopup.Show();
            break;

        case 3: // custom with custom header
            CustomAlertPopupHeaderLabel.SetText(customHeaderMessage);
            CustomAlertPopupMessage.SetText(message);
            CustomAlertPopup.Show();
            break;



    }

}
//once shown hide after 10 seconds
function OnAlertShown(s, e) {
    alertTimeOut = setTimeout(function () {
        UserToastNotification.Hide();
    }, 10000);
}

//hide notification on scroll overide
$(document).on('scroll', function () {
    if (typeof UserToastNotification !== "undefined" && UserToastNotification.IsVisible()) {
        clearTimeout(alertTimeOut);
        UserToastNotification.Hide();
    }
});
