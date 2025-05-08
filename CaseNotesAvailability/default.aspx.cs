using BLL;
using BusinessObjects;
using DevExpress.Web;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.XtraRichEdit.Commands;
using Login;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BusinessObjects.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace CaseNotesAvailability
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //protected void Btn_Click(object sender, EventArgs e)
        //{
        //    window.history.replaceState("SearchResult", "Search result", window.location.href + "?" + queryStringArray.join('&'));
        //}

        protected void HealthRecordsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //e.Parameters = new Dictionary<string, object>();
            if (e.Parameters != null) // select patient referrals & details sent from patient search grid
            {
                string eventArgument = e.Parameters;
                if (!string.IsNullOrEmpty(eventArgument))
                {
                    AuditDeleteBO DeleteCaseNote = Newtonsoft.Json.JsonConvert.DeserializeObject<AuditDeleteBO>(e.Parameters);

                    int casenote = DeleteCaseNote.AuditID;
                    BLL.AuditBLL.DeleteAudit(casenote, CookieHelper.GetCookieSessionID());

                    HealthRecordsGridView.JSProperties["cpDeleted"] = true;

                }
            }
        }


        protected void AuditorView_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = HealthRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID", "DueByDate", "SpecialtyID", "SiteID" }) as object[];

            if (values != null)
            {
                string AuditID = values[0]?.ToString() ?? "";
                string StatusID = values[1]?.ToString() ?? "";
                string date = values[2]?.ToString() ?? "";
                string Speciality = values[3]?.ToString() ?? "";
                string Site = values[4]?.ToString() ?? "";

                int status = (int)values[1];

                if (!String.IsNullOrEmpty(AuditID))
                {
                    AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
                }

                //if (!String.IsNullOrEmpty(StatusID))
                //{
                //  StatusID = HttpUtility.JavaScriptStringEncode(StatusID);

                switch (status)
                {
                    case (byte)Enums.AuditStatus.NotStarted:
                        //btn.Text = "Not Started";
                        btn.Text = "Start Audit";
                        btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AuditorView_Click(s, e, '{0}'); }}", values[0]);
                        break;
                    case (byte)Enums.AuditStatus.InProgress:
                        btn.Text = "Continue Audit";
                        btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AuditorView_Click(s, e, '{0}'); }}", values[0]);
                        break;
                    case (byte)Enums.AuditStatus.PendingHRreview:
                        btn.Text = "Action Review";
                        if (CookieHelper.GetCookieRoleID() == (byte) UserRoles.HRManagers)
                        {
                            //btn.Text = "Pending HR review";
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ Send_for_review(s, e, '{0}'); }}", values[0]);
                        }
                        else
                        {
                            btn.Visible = false;
                        }
                        break;
                    case (byte)Enums.AuditStatus.Completed:
                        btn.Text = "Completed";
                        btn.Visible = false;
                        //btn.ClientSideEvents.Click = String.Format("function(s, e) {{ Send_for_review(s, e, '{0}'); }}", values[0]);
                        break;
                    case (byte)Enums.AuditStatus.Reviewed:
                        btn.Text = "Reviewed";
                        btn.Visible = false;
                       break;

                }
            }


            //

            // btn.Click += new System.EventHandler(this.Button_Click);

            //}
            //else
            //{
            //    btn.Visible = false;
            //}
        }
        protected void PageHeaderLabel_Init(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                ASPxLabel lbl = sender as ASPxLabel;
                lbl.Text = $"Audit";
            }
        }

        protected void EditUserButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = HealthRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

            if (values != null)
            {
                string AuditID = values[0]?.ToString() ?? "";
                //string StatusID = values[1]?.ToString() ?? "";
                int status = (int)values[1];

                if (!String.IsNullOrEmpty(AuditID))
                {
                    AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
                }

                //  if (!String.IsNullOrEmpty(StatusID))
                // {
                //StatusID = HttpUtility.JavaScriptStringEncode(StatusID);

                switch (status)
                {
                    case (byte)Enums.AuditStatus.NotStarted:
                        //btn.Text = "Not Started";
                        btn.Text = "Edit";
                        btn.ClientSideEvents.Click = String.Format("function(s, e) {{ EditRow_Click(s, e, '{0}', '{1}'); }}", values[0], container.VisibleIndex);
                        break;
                    //case (byte)Enums.AuditStatus.InProgress:
                    //    //btn.Text = "In Progress";
                    //    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], status);
                    //    btn.Visible = false;
                    //    break;
                    //case (byte)Enums.AuditStatus.PendingHRreview:
                    //    //btn.Text = "Pending HR review";
                    //    btn.Text = "Review audit";
                    //    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], status);
                    //    break;
                    //case (byte)Enums.AuditStatus.Completed:
                    //    //btn.Text = "Completed";
                    //    btn.Visible = false;
                    //    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], status);
                    //    break;
                    default:
                        btn.Visible = false;
                        //btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], status);
                        break;

                }
                //}




                // btn.Click += new System.EventHandler(this.Button_Click);

            }
            else
            {
                btn.Visible = false;
            }
        }

        protected void NewRef_Init(object sender, EventArgs e)
        {

            ASPxButton btn = sender as ASPxButton;
            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ NewRef_Init(s, e); }}");

        }
        protected void RoleControlGridView_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.ButtonID == "DeleteUserButton")
            {
                int userID = Convert.ToInt32(HealthRecordsGridView.GetRowValues(e.VisibleIndex, "UserID"));

                if (userID == CookieHelper.GetCookieUserID())
                {
                    e.Enabled = false;
                }
            }
        }


        protected void DeleteUserButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = HealthRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

            if (values != null)
            {
                string AuditID = values[0]?.ToString() ?? "";
                //string StatusID = values[1]?.ToString() ?? "";
                int status = (int)values[1];


                if (!String.IsNullOrEmpty(AuditID))
                {
                    AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
                }

                //if (!String.IsNullOrEmpty(StatusID))
                //{
                //  StatusID = HttpUtility.JavaScriptStringEncode(StatusID);
                switch (status)
                {
                    case (byte)Enums.AuditStatus.NotStarted:
                        btn.Text = "Delete";
                        btn.ClientSideEvents.Click = String.Format("function(s, e) {{ DeleteButton_Click(s, e, '{0}' ); }}", container.VisibleIndex);
                        break;
                    default:
                        btn.Visible = false;
                        break;

                }
            }
            //btn.Click += Btn_Click;


        }

        private void Btn_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = HealthRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

            BLL.AuditBLL.DeleteAudit(Convert.ToInt32(values[0]), CookieHelper.GetCookieSessionID());

            ClientScript.RegisterStartupScript
            (GetType(), Guid.NewGuid().ToString(), "DeleteRow_Click();", true);
        }

        protected void AuditRow_Updating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            Dictionary<string, string> oldVals = e.OldValues.Cast<DictionaryEntry>()
                                               .ToDictionary(k => k.Key == null ? "" : k.Key.ToString(), v => v.Value == null ? "" : v.Value.ToString());
            Dictionary<string, string> newVals = e.NewValues.Cast<DictionaryEntry>()
                                                .ToDictionary(k => k.Key == null ? "" : k.Key.ToString(), v => v.Value == null ? "" : v.Value.ToString());

            Dictionary<string, string> valDiff = oldVals.Where(x => newVals[x.Key] != x.Value).ToDictionary(x => x.Key, x => x.Value);

            if (valDiff.Count == 0)
            {
                grid.JSProperties["cpNoUpdateMade"] = true;
                e.Cancel = true;
                grid.CancelEdit();
            }
            else
            {
                e.Cancel = false;
            }

        }
        protected void HealthRecordsView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }

        protected void GetSpeciality_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }
        protected void GetSites_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }
        protected void Status_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }
        protected void Audit_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var obj = e.InputParameters["Audit"] as AuditBO;
            e.InputParameters.Add("SessionID", CookieHelper.GetCookieSessionID());
            short userID = Login.CookieHelper.GetCookieUserID();
            obj.CreatedByUserID = userID;
        }

        protected void Audit_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var obj = e.InputParameters["Audit"] as AuditBO;
            //e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
            short userID = Login.CookieHelper.GetCookieUserID();
            obj.CreatedByUserID = userID;
            //obj.Date = DateTime.Now;
            obj.StatusID = 1;
            obj.SessionID = CookieHelper.GetCookieSessionID();
            //e.InputParameters.Add("SessionID", CookieHelper.GetCookieSessionID());
        }

        protected void HealthRecordsGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            GridViewLayoutGroup group = grid.EditFormLayoutProperties.FindItemOrGroupByName("FieldGroup") as GridViewLayoutGroup;
            group.Caption = "Create new Audit";
            grid.SettingsCommandButton.UpdateButton.Text = "Create new Audit";

            // hide audit column on new row
            GridViewColumnLayoutItem auditCol = grid.EditFormLayoutProperties.FindItemOrGroupByName("AuditID") as GridViewColumnLayoutItem;
            auditCol.Visible = false;

        }

        protected void HealthRecordsGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            GridViewLayoutGroup group = grid.EditFormLayoutProperties.FindItemOrGroupByName("FieldGroup") as GridViewLayoutGroup;
            group.Caption = $"Update Audit (ID: {e.EditingKeyValue})";
            grid.SettingsCommandButton.UpdateButton.Text = "Update Audit";
        }

        protected void ClinicCodesHelpLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;
            string lbltext = "To add a clinic code below type the clinic code then press comma \",\". Blank entries are not allowed.";
            lbl.Text = HelperClasses.NotificationHelper.CreateNotificationAlert(HelperClasses.NotificationHelper.NotificationType.Information, lbltext, false, false);
        }

        protected void HealthRecordsGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Date" || e.Column.FieldName == "DueByDate")
            {
                var date = e.Editor as ASPxDateEdit;
                date.MinDate = DateTime.Now.Date;
            }
        }

        protected void HealthRecordsGridView_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            HealthRecordsGridView.JSProperties["cpUpdated"] = true;
        }

        protected void HealthRecordsGridView_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            HealthRecordsGridView.JSProperties["cpUpdated"] = true;
        }
    }
}