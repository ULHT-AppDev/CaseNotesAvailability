using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

namespace Login
{
    public class CookieHelper
    {
        public static void SetNewCookie()
        {
            // user is now in the system, renew every request
            try
            {
                Nullable<DateTime> appMaintenanceStartTime = (DateTime?)HttpContext.Current.Application["MaintenanceStartTime"];
                DateTime currentTime = System.DateTime.Now;
                DateTime expiration = currentTime.AddMinutes(HttpContext.Current.Session.Timeout); // default session time + current time
                bool isBypass = GetCookieIsBypass(); //get last userdata
                bool addCookie = true;

                // check if there is no bypass active and if a maintenance notice time exists
                if (isBypass == false && appMaintenanceStartTime != null)
                {
                    DateTime maintenanceStartTime = Convert.ToDateTime(appMaintenanceStartTime);

                    // has current time gone past the scheduled time?
                    if (maintenanceStartTime < currentTime)
                    {
                        //current time has gone past the scheduled - kick user out
                        FormsAuthentication.SignOut();
                        addCookie = false; // dont add a new cookie

                        Redirect(FormsAuthentication.LoginUrl);
                    }
                    else
                    {
                        //check what the time difference is and if is under the session timeout time then add the custom minutes
                        int timeDifference = Convert.ToInt32((maintenanceStartTime - currentTime).TotalMinutes);

                        if (timeDifference > HttpContext.Current.Session.Timeout)
                        {
                            expiration = currentTime.AddMinutes(HttpContext.Current.Session.Timeout);
                        }
                        else
                        {
                            expiration = maintenanceStartTime.AddSeconds(-1);
                        }
                    }
                }

                if (addCookie)
                {
                    //get current cookie
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    //create new cookie with old data and new expiration
                    FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, expiration, true, ticket.UserData, ticket.CookiePath);
                    cookie.Value = FormsAuthentication.Encrypt(newTicket);
                    cookie.Expires = newTicket.Expiration;
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                ex.Source = "CookieHelper.cs - SetNewCookie";
                LoginLogHelper.LogErrorNoSession(ex);
                throw ex;
            }
        }

        public static void SetLoginCookie(string username, string domain, CookieUserData userdata, DateTime expireTime)
        {
            try
            {
                string jsonUserData = JsonConvert.SerializeObject(userdata);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, expireTime, false, jsonUserData);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Current.Response.Cookies.Set(authCookie);
            }
            catch (Exception ex)
            {
                ex.Source = "CookieHelper.cs - SetLoginCookie";
                throw ex; // will catch the logging back on login.aspx
            }

            if (LoginConfig.Logging_LogUserRoleWhenLoggedIn)
            {
                LoginLogHelper.InsertUserLoggedInLog(userdata.SessionID, userdata.RoleID, userdata.UserID);
            }

            System.Diagnostics.Debug.WriteLine("Login Cookie Set");
            Redirect(FormsAuthentication.DefaultUrl);
        }

        private static void Redirect(string redirectUrl)
        {
            //Redirect
            var page = (System.Web.UI.Page)HttpContext.Current.Handler;

            if (page != null)
            {
                if (page.IsCallback)
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(redirectUrl);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(redirectUrl);
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                try
                {
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(redirectUrl);
                }
                catch
                {
                    HttpContext.Current.Response.Redirect(redirectUrl);
                    HttpContext.Current.Response.End();
                }
            }
        }


        public static CookieUserData GetUserCookieData()
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                CookieUserData cookieData = JsonConvert.DeserializeObject<CookieUserData>(ticket.UserData);
                return cookieData;
            }
            catch (Exception ex)
            {
                ex.Source = "CookieHelper.cs - GetUserCookieData";
                LoginLogHelper.LogErrorNoSession(ex);
                throw ex;
            }
        }

        public static bool GetCookieIsBypass()
        {
            try
            {
                CookieUserData userDataArray = GetUserCookieData();
                return userDataArray.IsAdminBypass;
            }
            catch
            {
                //something went wrong assume not admin
                return false;
            }
        }

        public static DateTime GetCookieExpiration()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            return ticket.Expiration;
        }

        public static int GetCookieSessionID() => Convert.ToInt32(GetUserCookieData().SessionID);

        public static string GetCookieUserName() => GetUserCookieData().Username;

        public static string GetCookieFirstName() => GetUserCookieData().Forename;

        public static string GetCookieLastName() => GetUserCookieData().Surname;

        public static string GetCookieFullName()
        {
            var data = GetUserCookieData();
            return String.Format("{0} {1}", data.Forename, data.Surname);
        }

        public static string GetCookieFullNameWithUserName()
        {
            CookieUserData data = GetUserCookieData();
            return String.Format("{0} {1} ({2})", data.Forename, data.Surname, data.Username);
        }

        public static Guid GetCookieUserGuid() => GetUserCookieData().UserGuid;

        public static string GetCookieEmailAddress() => GetUserCookieData().EmailAddress;

        public static string GetCookieTelephoneNumber() => GetUserCookieData().ContactNumber;

        public static short GetCookieUserID() => Convert.ToInt16(GetUserCookieData().UserID);

        public static byte GetCookieRoleID() => (byte)GetUserCookieData().RoleID;

        public static List<short> GetCookieRights() => GetUserCookieData().Rights;
    }
}