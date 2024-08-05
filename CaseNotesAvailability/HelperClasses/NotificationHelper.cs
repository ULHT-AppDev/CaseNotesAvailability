using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelperClasses
{
    public static class NotificationHelper
    {
        public enum NotificationType
        {
            Information = 1,
            Warning = 2,
            Error = 3
        }

        public static string CreateNotificationAlert(NotificationType notificationType, string message, bool marginTop, bool marginBottom, bool widthOneHundred = false)
        {
            string mt = marginTop ? " mt-3 " : "";
            string mb = marginBottom ? " mb-3 " : "";
            string containerClass = "";
            string labelClass = "";
            string flexClass = widthOneHundred ? "d-flex " : "d-inline-flex "; //d-flex will span 100% of container
            string iconType = "";

            switch (notificationType)
            {
                case NotificationType.Information:
                    containerClass = " InformationContainer ";
                    labelClass = "InformationLabel";
                    iconType = "fas fa-info-circle";
                    break;

                case NotificationType.Warning:
                    containerClass = " WarningContainer ";
                    labelClass = "WarningLabel";
                    iconType = "fas fa-exclamation-circle";
                    break;

                case NotificationType.Error:
                    containerClass = " ErrorContainer ";
                    labelClass = "ErrorLabel";
                    iconType = "fas fa-exclamation-triangle";
                    break;
            }

            // create html notification
            return $"<div class='{flexClass}align-items-center{containerClass}{mt}{mb}p-2'><div class='px-2'><i class='{iconType} {labelClass} LabelIcon'></i></div><div class='px-2'><span class='{labelClass}'>{message}</span></div></div>";
        }
    }
}