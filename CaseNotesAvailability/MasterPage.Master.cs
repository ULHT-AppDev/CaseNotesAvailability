using DevExpress.Web;
using Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BusinessObjects.Enums;

namespace CaseNotesAvailability
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        // used for dynamic js when updating the client
        public string VersionNumber { get => ConfigurationManager.AppSettings["VersionNumber"].Replace(".", String.Empty); }

        protected void Page_Init(object sender, EventArgs e)
        {
            SetLoginLabels();
            AppHeaderLink.HRef = FormsAuthentication.DefaultUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetLoginLabels()
        {
            try
            {
                LoggedInUser.Text = $"Welcome, <strong>{CookieHelper.GetCookieFirstName()}</strong> ({CookieHelper.GetCookieUserName()})";
            }
            catch
            {
                LoggedInUser.Text = $"Welcome, <strong>Development</strong> (dev)";
            }
        }

        protected void LoggoutButton_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            if (Login.LoginConfig.Database_RequireDBForLogin)
            {
                new BLL.LogsBLL().EndUserSession(CookieHelper.GetCookieSessionID());
            }

            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        protected void AppHeaderLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;
            string loginPageText = ConfigurationManager.AppSettings["LoginPageTitle"];

            if (!(ConfigurationManager.AppSettings["Environment"] == "Live"))
            {
                loginPageText += " - <span class='redText'>Development Version</span>";
            }

            lbl.Text = loginPageText;
        }

        protected void MainAppMenu_Init(object sender, EventArgs e)
        {
            ASPxMenu menu = sender as ASPxMenu;

            //switch (CookieHelper.GetCookieRoleID())
            //{
            //    case (byte)UserRoles.Admin:
            //        // open admin menu for admins
            //        menu.Items.FindByName("Admin").Visible = true;

            //        break;
            //}
        }

    }
}