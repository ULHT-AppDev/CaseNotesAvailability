using Login.LoginObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;


namespace Login
{
    public partial class Login : System.Web.UI.Page
    {
        public string VersionNumber { get => ConfigurationManager.AppSettings["VersionNumber"].Replace(".", String.Empty); }

        private Nullable<DateTime> StartTimeCheck = null;
        private bool DbConnected = true;


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string loginPageText = ConfigurationManager.AppSettings["LoginPageTitle"];

                if (!(ConfigurationManager.AppSettings["Environment"] == "Live"))
                {
                    loginPageText += " - <span class='redText'>Development Version</span>";
                }

                LoginPageTitle.Text = loginPageText;
                Page.Title = string.Format("Login - {0}", ConfigurationManager.AppSettings["LoginPageTitle"]);
                VersionNumberText.Text = String.Format("Version {0}", ConfigurationManager.AppSettings["VersionNumber"]);
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // remove cache from form fields on login page
            // "The state information is invalid for this page and might be corrupted"
            // if user refreshes or hits back button on firefox https://yourtahir.wordpress.com/2008/06/26/the-state-information-is-invalid-for-this-page-and-might-be-corrupted/
            if (Request.Browser.Type.Contains("Firefox")) // replace with your check
            {
                Response.Cache.SetNoStore();
            }

            //reset view on load
            ReturnViewToNormal();

            DateTime? appMaintenanceStartTime = (DateTime?)HttpContext.Current.Application["MaintenanceStartTime"];
            ScheduledMaintenanceDiv.Visible = true;
            List<NoticeObject> noticeObjectList = new List<NoticeObject>();
            NoticeObject noticeObjectDisplay = null;
            DateTime currentNotice;
            DateTime? earliestNotice = null;
            ConnectionOutcome connectionOutcomeObject = new ConnectionOutcome();

            // loop through connection strings to check if database's are online
            foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
            {
                if (connection.Name != "LocalSqlServer")
                {
                    connectionOutcomeObject = IsDBAvailable(connection.ConnectionString, connection.ProviderName);

                    if (connectionOutcomeObject.ConnectionSuccess == false)
                    {
                        // one or more database failed the test show connection error message
                        DbConnected = false;
                    }

                    // add notices if they exist
                    if (connectionOutcomeObject.NoticeObjList != null)
                    {

                        // add all maintenance notices to a list
                        foreach (NoticeObject noticeObject in connectionOutcomeObject.NoticeObjList)
                        {
                            noticeObjectList.Add(noticeObject);
                        }

                        // if a maintenance notice exists then do the following
                        if (noticeObjectList != null)
                        {
                            foreach (NoticeObject noticeObj in noticeObjectList)
                            {
                                currentNotice = noticeObj.StartDateTime;

                                // set the earliest notice as the one to be displayed
                                if ((earliestNotice == null) || (currentNotice < earliestNotice))
                                {
                                    earliestNotice = currentNotice;
                                    noticeObjectDisplay = noticeObj;
                                }
                            }
                        }
                    }
                }
            }

            const string timeFormat = "dd-MMM-yyyy HH:mm";
            bool showPopup = false;
            string modalHeader = "Planned Maintenance Notice";
            string modalContent = null;
            string scheduledMaintenanceLabelText = null;
            string scheduledMaintenanceLabelColor = "redText";
            bool showSystemUnavailable = false;

            // determine what to do regarding maintenance notices if any

            // check if database is connected
            if (DbConnected)
            {
                // check for notice - should be normal path
                if (noticeObjectDisplay == null)
                {
                    // *1 (see comment at bottom page)

                    ScheduledMaintenanceDiv.Visible = false;

                    if (!IsPostBack)
                    {
                        if (User.Identity.Name != "")
                        {
                            if (Page.IsCallback)
                            {
                                DevExpress.Web.ASPxWebControl.RedirectOnCallback(FormsAuthentication.DefaultUrl);
                            }
                            else
                            {
                                Response.Redirect(FormsAuthentication.DefaultUrl);
                                Response.End();
                            }
                        }
                    }
                }
                else
                {
                    // notice exists
                    double startDiff = (noticeObjectDisplay.StartDateTime - System.DateTime.Now).TotalMinutes;
                    int warningMinutes = Convert.ToInt32(startDiff);
                    TimeSpan warningMinutesTimeSpan = TimeSpan.FromMinutes(warningMinutes);
                    bool addHours = false;
                    bool addHoursSCharacter = false;
                    string warningTimeString;

                    if (warningMinutesTimeSpan.Hours != 0)
                    {
                        addHours = true;

                        if (warningMinutesTimeSpan.Hours != 1)
                        {
                            addHoursSCharacter = true;
                        }
                    }

                    if (addHours == true)
                    {
                        warningTimeString = string.Format("{0} hour{1} {2} minutes", warningMinutesTimeSpan.Hours, (addHoursSCharacter ? "s" : ""), warningMinutesTimeSpan.Minutes);
                    }
                    else
                    {
                        warningTimeString = string.Format("{0} minutes", warningMinutesTimeSpan.Minutes);
                    }

                    // check how to format page
                    if (startDiff <= 0)
                    {
                        showPopup = true;
                        modalContent = string.Format("<strong class='blueText font-Large'>Planned maintenance is ongoing</strong>{0}{0}Start: {1}{0}Expected End: <strong>{2}</strong>", "<br />", noticeObjectDisplay.StartDateTime.ToString(timeFormat), noticeObjectDisplay.ExpectedEndDateTime.ToString(timeFormat));
                        scheduledMaintenanceLabelText = "<i class='fas fa-plug font-Medium'></i> <span class='ps-2'>Planned maintenance is underway - <strong>You will not be able to log in</strong><a href='javascript:showModal()'><br />See more details</a></span>";
                        showSystemUnavailable = true;
                    }
                    else if (startDiff > 0 && startDiff <= noticeObjectDisplay.WarningMinutes)
                    {
                        showPopup = true;
                        modalContent = string.Format("<strong class='orangeText font-Large'>Planned maintenance in the next {3}.</strong>{0}{0}This system will be unavailable as detailed below:{0}{0}Start: <strong>{1}</strong>{0}Expected End: {2}{0}{0}<span class='blueText'>Please note that the system will become unavailable without further notice during the above time period so please make sure any changes are saved prior to the start time.</span>", "<br />", noticeObjectDisplay.StartDateTime.ToString(timeFormat), noticeObjectDisplay.ExpectedEndDateTime.ToString(timeFormat), warningTimeString);
                        scheduledMaintenanceLabelText = string.Format("<i class='fas fa-plug font-Medium'></i> <span class='ps-2'>Planned maintenance is scheduled in <strong>{0}</strong> <a href='javascript:showModal()'><br />See more details</a></span>", warningTimeString);
                    }
                    else
                    {
                        modalContent = string.Format("This system will be unavailable as detailed below:{0}{0}Start: <strong>{1}</strong>{0}Expected End: {2}{0}{0}<span class='blueText'>Please note that the system will become unavailable without further notice during the above time period so please make sure any changes are saved prior to the start time.</span>", "<br />", noticeObjectDisplay.StartDateTime.ToString(timeFormat), noticeObjectDisplay.ExpectedEndDateTime.ToString(timeFormat));
                        scheduledMaintenanceLabelText = string.Format("<i class='fas fa-info-circle font-Medium'></i><span class='ps-2'>Planned maintenance is scheduled for <strong>{0}</strong> <a href='javascript:showModal()'><br />See more details</a></span>", noticeObjectDisplay.StartDateTime.ToString(timeFormat));
                    }

                    // NB: Orange if greater than session timeout; otherwise red (regardless of warning period)
                    if (showSystemUnavailable == false)
                    {
                        if (startDiff > Session.Timeout)
                        {
                            scheduledMaintenanceLabelColor = "orangeText";
                        }
                    }

                    StartTimeCheck = Convert.ToDateTime(noticeObjectDisplay.StartDateTime);
                }
            }
            else
            {
                string supportLink = LoginHelper.GetSupportLink();

                // check for notice
                if (noticeObjectDisplay == null)
                {
                    showPopup = true;
                    modalHeader = "Unexpected Error";
                    modalContent = $"An unplanned system error has occurred. Please try again in a few minutes and report the problem to the {supportLink} if it continues.";
                    scheduledMaintenanceLabelText = "<i class='fas fa-exclamation'></i><span class='ps-2'>Unplanned system error occurred <a href='javascript:showModal()'><br />See more details</a></span>";
                    showSystemUnavailable = true;
                }
                else
                {
                    // notice exists but an unplanned error has also occurred

                    // get start time of notice as may be down because of the planned maintenance
                    StartTimeCheck = Convert.ToDateTime(noticeObjectDisplay.StartDateTime);

                    if (StartTimeCheck > System.DateTime.Now)
                    {
                        showPopup = true;
                        modalHeader = "Unexpected Error";
                        modalContent = $"An unplanned system error has occurred <strong>before the start of the scheduled planned maintanence</strong>. Please try again in a few minutes and report the problem to the {supportLink} if it continues.";
                        scheduledMaintenanceLabelText = "<i class='fas fa-exclamation'></i><span class='ps-2'>Unplanned system error occurred <a href='javascript:showModal()'><br />See more details</a></span>";
                        showSystemUnavailable = true;
                    }
                    else
                    {
                        showPopup = true;
                        modalContent = string.Format("<strong class='blueText font-Large'>Planned maintenance is ongoing</strong>{0}{0}Start: {1}{0}Expected End: <strong>{2}</strong>", "<br />", noticeObjectDisplay.StartDateTime.ToString(timeFormat), noticeObjectDisplay.ExpectedEndDateTime.ToString(timeFormat));
                        scheduledMaintenanceLabelText = "<i class='fas fa-plug font-Medium'></i> <span class='ps-2'>Planned maintenance is underway - <strong>You will not be able to log in</strong><a href='javascript:showModal()'><br />See more details</a></span>";
                        showSystemUnavailable = true;
                    }
                }
            }

            // format page with options
            FormatPageOptions(scheduledMaintenanceLabelText, scheduledMaintenanceLabelColor, showPopup, modalHeader, modalContent, showSystemUnavailable);

            // check whether application variable needs updating check if either is null
            if (appMaintenanceStartTime == null && StartTimeCheck == null)
            {
            }
            else
            {
                bool updateMaintenanceStartTime = false;

                if ((appMaintenanceStartTime == null) != (StartTimeCheck == null))
                {
                    // one or other value set
                    updateMaintenanceStartTime = true;
                }
                else if (appMaintenanceStartTime != null && StartTimeCheck != null && appMaintenanceStartTime != StartTimeCheck)
                {
                    // both values set; but values differ
                    updateMaintenanceStartTime = true;
                }

                // add/update application variable
                if (updateMaintenanceStartTime)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application["MaintenanceStartTime"] = StartTimeCheck;
                    System.Web.HttpContext.Current.Application.UnLock();
                }
            }
        }

        private ConnectionOutcome IsDBAvailable(string connectionString, string provider)
        {
            ConnectionOutcome connectionOutcomeObject = new ConnectionOutcome();
            string plannedConnectionString = "";

            if (provider == "System.Data.EntityClient")
            {
                using (EntityConnection connection = new EntityConnection(connectionString))
                {
                    connectionString = new EntityConnectionStringBuilder(connectionString).ProviderConnectionString;
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (!LoginHelper.GetPlannedMaintenanceExclusionList().Contains(connection.Database.ToUpper()))
                {
                    plannedConnectionString = connection.ConnectionString;
                }

                try
                {
                    connection.Open();
                    connectionOutcomeObject.ConnectionSuccess = true;
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connectionOutcomeObject.ConnectionSuccess = false;
                    LoginLogHelper.LogErrorNoSession(ex);
                    Debug.WriteLine(ex.InnerException);
                }
                catch (Exception ex2)
                {
                    connectionOutcomeObject.ConnectionSuccess = false;
                    LoginLogHelper.LogErrorNoSession(ex2);
                    Debug.WriteLine(ex2.InnerException);
                }
            }


            // check current connections servers planned maintainance db table
            if (plannedConnectionString != "")
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(plannedConnectionString);
                scsb.InitialCatalog = "PlannedMaintenance";
                plannedConnectionString = scsb.ConnectionString;
                using (SqlConnection plannedConnection = new SqlConnection(plannedConnectionString))
                {
                    try
                    {
                        plannedConnection.Open();

                        var loginUrl = Page.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];

                        using (SqlCommand cmd = new SqlCommand("dbo.GetActiveNotices", plannedConnection))
                        {

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            SqlParameter param = new SqlParameter()
                            {
                                SqlDbType = SqlDbType.VarChar,
                                ParameterName = "@LoginPage",
                                Value = loginUrl,
                                Precision = 100
                            };


                            cmd.Parameters.Add(param);

                            using (SqlDataReader r = cmd.ExecuteReader())
                            {
                                NoticeObject nObj;

                                while (r.Read())
                                {
                                    nObj = new NoticeObject();
                                    nObj.WarningMinutes = r.GetByte(1);
                                    nObj.StartDateTime = r.GetDateTime(2);
                                    nObj.ExpectedEndDateTime = r.GetDateTime(3);
                                    if (connectionOutcomeObject.NoticeObjList == null)
                                        connectionOutcomeObject.NoticeObjList = new List<NoticeObject>();

                                    connectionOutcomeObject.NoticeObjList.Add(nObj);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoginLogHelper.LogErrorNoSession(ex);
                        Debug.WriteLine(ex.InnerException);
                    }
                    finally
                    {
                        if (plannedConnection.State == System.Data.ConnectionState.Open)
                        {
                            plannedConnection.Close();
                        }
                    }
                }
            }

            return connectionOutcomeObject;
        }

        protected void FullLoginCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            LoginType loginType = (LoginType)Convert.ToByte(param[0]);

            if (loginType == LoginType.Login)
            {
                try
                {
                    LoginButtonClick();
                }
                catch (ConfigurationErrorsException ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.ConfigError);
                    Debug.WriteLine("Custom config error | Login click");
                }
                catch (PrincipalException ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.ActiveDirectoryError);
                    Debug.WriteLine("AD Error | Login click");
                }
                catch (CreateSessionIDException ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.UnableToCreateSessionID);
                    Debug.WriteLine("Unable To Create Session ID | Login click");
                }
                catch (Exception ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.UnknownError);
                    Debug.WriteLine("Unkown Error | Login click");
                }
            }
            else if (loginType == LoginType.ContinueWithRole)
            {
                try
                {
                    byte roleID = Convert.ToByte(param[1]);
                    ContinueWithRoleClick(roleID);
                }
                catch (CreateSessionIDException ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.UnableToCreateSessionID);
                    Debug.WriteLine("Unable To Create Session ID | Login click");
                }
                catch (Exception ex)
                {
                    LoginLogHelper.LogErrorNoSession(ex);
                    DisplayLoginErrors(LoginErrorType.UnknownError);
                    Debug.WriteLine("Unkown Error | Role select");
                }
            }
        }

        protected void LoginButtonClick()
        {
            using (HostingEnvironment.Impersonate())
            {
                // -- Set if using Role Permissions -- //
                bool requireDBForLogin = LoginHelper.GetRequireDBForLogin();

                // -- Set if Maintenance started -- //
                bool maintenanceStarted = false;

                if (StartTimeCheck != null && StartTimeCheck < DateTime.Now)
                {
                    maintenanceStarted = true;
                }

                // -- Check if DB Down now -- //
                SystemDownType downType = SystemDownType.None;

                if (!DbConnected)
                {
                    downType = maintenanceStarted ? SystemDownType.Planned : SystemDownType.Unplanned;

                    if (requireDBForLogin)
                    {
                        DisplayLoginErrors((downType == SystemDownType.Planned) ? LoginErrorType.SystemUnavailableLoginNotAttempted : LoginErrorType.SystemUnavailable);
                        Debug.WriteLine("System Down | Login.aspx");
                        return;
                    }
                }

                //create new credentials object
                LoginCredentials loginCred = new LoginCredentials()
                {
                    Username = FullUsernameTextBox.Text,
                    Password = FullPasswordTextBox.Text,
                    Domain = DomainComboBox.SelectedItem.Text
                };

                bool attemptAdminBypass = false;

                //prepare admin bypass check + correct username
                if (loginCred.Username.Length > 6 && loginCred.Username.Substring(0, 6).ToUpper() == "ADMIN:")
                {
                    attemptAdminBypass = true;
                    loginCred.Username = loginCred.Username.Substring(6);
                }

                // get User Principal
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, loginCred.Domain))
                {
                    using (UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, loginCred.Username))
                    {
                        if (userPrincipal == null)
                        {
                            DisplayLoginErrors(LoginErrorType.UserPrincipalNotFound); //account not found could return error numbers here [PRE-CHECK]
                            Debug.WriteLine("Credentials incorrect when checking AD");
                            return;
                        }

                        //admin bypass check
                        if (attemptAdminBypass)
                        {
                            loginCred.IsBypass = LoginHelper.CheckUserAgainstConfigGroups(userPrincipal, LoginHelper.GetLoginGroups(true));

                            if (loginCred.IsBypass == false)
                            {
                                DisplayLoginErrors(LoginErrorType.CredentialsIncorrect);
                                Debug.WriteLine("Tried Admin bypass - Not allowed to admin bypass not in group");
                                return;
                            }
                        }

                        //check if an admin and if maintenance start time has passed current time
                        if (loginCred.IsBypass == false && maintenanceStarted)
                        {
                            // not allowed in db is down
                            DisplayLoginErrors(LoginErrorType.SystemUnavailableLoginNotAttempted);
                            Debug.WriteLine("Maintenance Started - login not counted");
                            return;
                        }

                        BasicAccountDetails basicAccountDetails = LoginHelper.IsUserAuthenticatedAndAuthorisedInActiveDirectory(userPrincipal, loginCred);

                        if (basicAccountDetails == null)
                        {
                            DisplayLoginErrors(LoginErrorType.CredentialsIncorrect); //account not found could return error numbers here [RE-CHECK]
                            return;
                        }
                        else if (!basicAccountDetails.Authenticated)
                        {
                            DisplayLoginErrors(LoginErrorType.CredentialsIncorrect);
                            return;
                        }
                        else if (!basicAccountDetails.Authorised)
                        {
                            DisplayLoginErrors(LoginErrorType.NotAuthorisedToAccessBecauseGroup);
                            return;
                        }

                        int? userID = null;
                        byte? roleID = null;
                        LoginRoles loginRoles = null;
                        LoginRights loginRights = null;

                        if (requireDBForLogin)
                        {
                            //check if user is in db for login
                            LoginResult loginResult = LoginHelper.ManageDatabaseUserLogin(basicAccountDetails); // this method checks if user exists, trys to update user details (if config allows), if user doesnt exist will try insert (if config allows)

                            // something went wrong
                            if (loginResult.ErrorType != null)
                            {
                                DisplayLoginErrors((LoginErrorType)loginResult.ErrorType);
                                Debug.WriteLine("Login.aspx | line 579 | Config error ManageDatabaseUserLogin");
                                return;
                            }

                            // if here userID must exist

                            userID = loginResult.ReturnID; // user found or inserted ok

                            if (LoginHelper.UseRolePermissions())
                            {
                                loginRoles = LoginHelper.GetUsersSystemRoles((int)userID);

                                if (loginRoles.RoleList == null || !loginRoles.RoleList.Any())
                                {
                                    DisplayLoginErrors(LoginErrorType.UserInDBButNoRole);
                                    return;
                                }

                                roleID = loginRoles.RoleList[0].Key; // should this be inside first if underneath?

                                if (loginRoles.ErrorType != null) // this isnt needed i dont think
                                {
                                    DisplayLoginErrors((LoginErrorType)loginRoles.ErrorType);
                                    Debug.WriteLine("Login Roles Error");
                                    return;
                                }

                                if (loginRoles.RoleList.Count == 1)
                                {
                                    //loginRights = LoginHelper.GetUserRights(loginRoles.RoleList[0].RoleID);
                                    loginRights = LoginHelper.GetUserRights(loginRoles.RoleList[0].Key);

                                    if (loginRights.ErrorType != null)
                                    {
                                        DisplayLoginErrors((LoginErrorType)loginRights.ErrorType);
                                        return;
                                    }
                                }
                                else if (loginRoles.RoleList.Count > 1)
                                {
                                    //multiple roles exist - get user to choose which role they want to use
                                    FullLoginCallbackPanel.JSProperties["cpMultipleRoles"] = true;
                                    //bind combo box here with role ids

                                    string badString = JsonConvert.SerializeObject(basicAccountDetails);

                                    RoleChoiceHiddenField.Set("BAD", badString);
                                    RoleChoiceHiddenField.Set("UserID", userID);
                                    RoleChoiceCombobox.DataSource = loginRoles.RoleList;
                                    RoleChoiceCombobox.DataBind();

                                    return;
                                }
                            }
                        }

                        List<short> rightsList = new List<short>();

                        if (loginRights != null && loginRights.RightList != null)
                        {
                            rightsList = loginRights.RightList;
                        }

                        DateTime expireTime = GetExpireTime(basicAccountDetails.IsBypass);
                        LoginHelper.CreateLoginAuthTicket(basicAccountDetails, roleID, rightsList, userID, expireTime);
                    }
                }
            }
        }

        protected void ContinueWithRoleClick(byte roleID)
        {
            LoginRights loginRights = LoginHelper.GetUserRights(roleID);

            if (loginRights.ErrorType != null)
            {
                DisplayLoginErrors((LoginErrorType)loginRights.ErrorType);
                return;
            }

            int? userID = (int?)RoleChoiceHiddenField.Get("UserID");
            BasicAccountDetails basicAccountDetails = JsonConvert.DeserializeObject<BasicAccountDetails>(RoleChoiceHiddenField.Get("BAD").ToString());

            DateTime expireTime = GetExpireTime(basicAccountDetails.IsBypass);
            LoginHelper.CreateLoginAuthTicket(basicAccountDetails, roleID, loginRights.RightList, userID, expireTime);
        }

        private DateTime GetExpireTime(bool isBypass)
        {
            DateTime expireTime;

            if (isBypass)
            {
                expireTime = DateTime.Now.AddMinutes(Session.Timeout);
            }
            else
            {
                if (StartTimeCheck == null)
                {
                    // add normal timeout
                    expireTime = DateTime.Now.AddMinutes(Session.Timeout);
                }
                else
                {
                    //check for how many minutes to change to
                    double check = ((DateTime)StartTimeCheck - DateTime.Now).TotalMinutes;

                    if (check < HttpContext.Current.Session.Timeout)
                    {
                        double seconds = Math.Floor(((DateTime)StartTimeCheck - DateTime.Now).TotalSeconds);
                        expireTime = DateTime.Now.AddSeconds(seconds);
                    }
                    else
                    {
                        expireTime = DateTime.Now.AddMinutes(Session.Timeout);
                    }
                }
            }

            return expireTime;
        }

        // Page Format
        private void FormatPageOptions(string ScheduledMaintenanceMessage, string ScheduledMaintenanceColour, bool ShowPopup, string ModalHeader, string ModalContent, bool ShowSystemUnavailable)
        {
            if (ScheduledMaintenanceMessage != null)
            {
                ScheduledMaintenanceLabel.Text = ScheduledMaintenanceMessage;
                if (ScheduledMaintenanceColour != null)
                {
                    ScheduledMaintenanceLabel.CssClass = ScheduledMaintenanceColour;
                }
            }

            if (ModalHeader != null)
            {
                ModalHeaderLabel.Text = ModalHeader;
            }

            if (ModalContent != null)
            {
                ModalContenetLabel.Text = ModalContent;
            }

            if (ShowSystemUnavailable)
            {
                LoginButton.Text = "System unavailable";
                LoginButton.RenderMode = DevExpress.Web.ButtonRenderMode.Danger;
                FullUsernameTextBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.Red;
                FullPasswordTextBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.Red;
                DomainComboBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.Red;
            }

            if (ShowPopup)
            {
                ShowNoticeHiddenField.Set("ShowPopup", true);
            }
        }

        private void ReturnViewToNormal()
        {
            LoginButton.Text = "Login";
            LoginButton.RenderMode = DevExpress.Web.ButtonRenderMode.Button;
            FullUsernameTextBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.FromName("#009688");
            FullPasswordTextBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.FromName("#009688");
            DomainComboBox.FocusedStyle.Border.BorderColor = System.Drawing.Color.FromName("#009688");
        }

        // Login Error Text
        private void DisplayLoginErrors(LoginErrorType error)
        {
            string errorMessage = "";
            string supportLink = LoginHelper.GetSupportLink();

            switch (error)
            {
                case LoginErrorType.CredentialsIncorrect:
                case LoginErrorType.UserPrincipalNotFound:
                    errorMessage = $"Either the domain, username or password was incorrect. <br /> Repeated incorrect attempts will result in your account being<strong> locked.</ strong> <br /> If your account is locked contact the {supportLink}";

                    break;

                case LoginErrorType.NotAuthorisedToAccessBecauseUser:
                case LoginErrorType.NotAuthorisedToAccessBecauseGroup:
               // case LoginErrorType.NotAuthorisedToAccessBecauseRole:
                case LoginErrorType.UserDoesNotHaveRoles:

                    errorMessage = $"You are not authorised to access this application. If you think you should be able to access this application please contact the {supportLink}";

                    break;

                case LoginErrorType.SystemUnavailableLoginNotAttempted:

                    errorMessage = $"The system is currently unavailable and your login attempt has been cancelled";

                    break;

                case LoginErrorType.ConfigError:
                case LoginErrorType.UnableToInsertUser:
                case LoginErrorType.UnableToCheckUser:
                case LoginErrorType.RoleDoesNotHaveRights:
                case LoginErrorType.ConfigNotImplemented:
                case LoginErrorType.UnableToGetRights:
                case LoginErrorType.UnableToGetRoles:
                case LoginErrorType.UnableToCreateSessionID:
                case LoginErrorType.ConfigLoginGroups:
                case LoginErrorType.UnableToUpdateUser:
                //case LoginErrorType.RoleInsertFailed:

                    errorMessage = $"Login not possible due to system error, please notify the {supportLink}";

                    break;

                case LoginErrorType.SystemUnavailable:
                case LoginErrorType.ActiveDirectoryError:
                case LoginErrorType.UnknownError:
                default:

                    errorMessage = $"The system is currently unavailable, please try again in a few minutes and contact the {supportLink} if the problem it continues";

                    break;
            }

            ErrorLoginLabel.Text = $"{errorMessage} [Ref: {(byte)error:000}]";
            ErrorShake();


            LoginLogHelper.LogError(error);

        }

        private void ErrorShake()
        {
            //js prop to shake screen
            FullLoginCallbackPanel.JSProperties["cpError"] = true;
        }

    }
}


