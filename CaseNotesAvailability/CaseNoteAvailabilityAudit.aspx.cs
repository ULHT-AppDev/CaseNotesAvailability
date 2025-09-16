using BusinessObjects;
using DevExpress.Web;
using DevExpress.Web.Internal.Dialogs;
using DevExpress.Xpo.DB;
using Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using static BusinessObjects.Enums;

namespace CaseNotesAvailability
{

    public partial class _CaseNoteAvailabilityAudit : System.Web.UI.Page
    {
        // in memory 
        private int CASAuditId;
        private string Speciality;
        private DateTime AuditDate;
        private string Specialty;
        private string AuditSite;
        //public int StatusID { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ViewState["TextboxCount"] != null)
            {
                GeneratePatientForm((int)ViewState["TextboxCount"]);
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!(CookieHelper.GetCookieRoleID() == (byte)UserRoles.NursingteamUser))      //If the user does not have the right to view this page, we redirect
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);

            }

            bool canParse = int.TryParse(Request.QueryString["AuditID"], out int ID);

            if (!canParse)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);

                // throw to home page with error 
            }
            else
            {
                bool bAuditExist = new BLL.UnavailableCaseNotesBLL().CheckWhetherAuditExist(ID, CookieHelper.GetCookieSessionID());
                if (bAuditExist)
                {
                    SetAuditID(ID);
                }
                else
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl);
                    Response.End();
                }
            }

        }


        protected void Application_Error(object sender, EventArgs e)
        {
            HttpException httpException = Server.GetLastError() as HttpException;
            if (httpException.GetHttpCode() == 404)
                Response.Redirect("/default.aspx");
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SetAuditID(int auditID)
        {
            this.CASAuditId = auditID;
            SingleAuditBO SelectedAudit = new SingleAuditBO();
            SelectedAudit = new BLL.UnavailableCaseNotesBLL().SelectedAudit(auditID, CookieHelper.GetCookieSessionID());
            AuditDate = SelectedAudit.Date ?? DateTime.MinValue;
            Specialty = SelectedAudit.Specialty;
            AuditSite = SelectedAudit.Site;

        }

        protected void CompleteAudit_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = CaseNoteAvailabilityAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "ClinicCode", "AuditClinicAnswersID", "AuditID", "StatusID" }) as object[];

            if (values != null)
            {
                string ClinicCode = values[0]?.ToString() ?? "";
                string AuditClinicAnswersID = values[1]?.ToString() ?? "";
                string AuditID = values[2]?.ToString() ?? "";
                string StatusID = values[3]?.ToString() ?? "";

                if (!String.IsNullOrEmpty(StatusID))
                {
                    StatusID = HttpUtility.JavaScriptStringEncode(StatusID);

                    switch (Convert.ToByte(StatusID))
                    {
                        case (byte)Enums.AuditStatus.Completed:
                            //btn.Text = "Not Started";
                            btn.Visible = false;
                            break;
                        default:
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ CompleteAudit_ClientClick(s, e, '{0}', '{1}','{2}'); }}", ClinicCode, AuditClinicAnswersID, AuditID);
                            break;
                    }


                    //btn.Click += new System.EventHandler(this.Button_Click);

                }
            }
        }

        protected void CaseNoteAvailabilityUnAvailabilityCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter != null) // select patient referrals & details sent from patient search grid
            {
                UnAvailabilityCallbackBO obj = Newtonsoft.Json.JsonConvert.DeserializeObject<UnAvailabilityCallbackBO>(e.Parameter);

                string ClinicCode = obj.ClinicCode;
                int AuditClinicAnswerId = obj.AuditClinicAnswerId;
                int AuditID = obj.AuditID;
                txtAuditId.Value = AuditID;
                getAuditClinicAnswer(AuditClinicAnswerId, CookieHelper.GetCookieSessionID());
            }

        }

        protected void CreateFormDynamically_CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter != null) //&& int.TryParse(e.Parameter, out int iterations))
            {
                DynamicFormBO obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DynamicFormBO>(e.Parameter);

                if (obj.ActionID != 0)
                {

                    int iterations = obj.NumberofRows;

                    if (iterations > 0)
                    {
                        ViewState["TextboxCount"] = iterations;
                        GeneratePatientForm(iterations);

                    }
                }
            }
        }

        private void GeneratePatientForm(int count)
        {
            PageControl.TabPages.Clear();
            int totalItems = count;
            int itemsPerTab = 10;
            int tabCount = (int)Math.Ceiling((double)totalItems / itemsPerTab);

            List<ReasonUnavailableBO> UnAvailableReason = new List<ReasonUnavailableBO>();
            UnAvailableReason = new BLL.UnavailableCaseNotesBLL().GetUnAvailableReasons(CookieHelper.GetCookieSessionID());
            for (int tabIndex = 0; tabIndex < tabCount; tabIndex++)
            {

                TabPage tab = new TabPage();
                tab.Text = "Page " + (tabIndex + 1);

                ASPxFormLayout UnavailabilityFormLayout = new ASPxFormLayout();
                UnavailabilityFormLayout.ID = "formLayout_" + tabIndex;
                UnavailabilityFormLayout.Width = Unit.Percentage(100);
                UnavailabilityFormLayout.ColumnCount = 2;
                for (int i = 1; i < (itemsPerTab + 1); i++)
                {
                    int itemIndex = tabIndex * itemsPerTab + i;
                    if (itemIndex >= totalItems +1) break;
                    LayoutGroup lg = new LayoutGroup()
                    {
                        Caption = $"Details for Patient {itemIndex}",
                        GroupBoxDecoration = GroupBoxDecoration.Box,
                        AlignItemCaptions = true

                    };

                    LayoutItem nameItem = new LayoutItem()
                    {
                        Caption = $"NHS/UNumber",

                    };
                    //PlaceHolder pholder = new PlaceHolder();

                    nameItem.CaptionStyle.Font.Size = System.Web.UI.WebControls.FontUnit.Point(11);
                    ASPxTextBox patientNameTextBox = new ASPxTextBox();
                    patientNameTextBox.ID = $"PatientNameTextBox_{itemIndex}"; // id needs to be unique so can get value in js when submitting as dynamically created
                    patientNameTextBox.ClientInstanceName = $"PatientNameTextBox_{itemIndex}"; // id needs to be unique so can get value in js when submitting as dynamically created
                    patientNameTextBox.MaxLength = 200;
                    // add in validation settings and stuff here like below one (look at combo below)
                    patientNameTextBox.ValidationSettings.RequiredField.IsRequired = true;
                    patientNameTextBox.ValidationSettings.ValidationGroup = "CaseNoteVal1";
                    patientNameTextBox.ValidationSettings.RequiredField.ErrorText = "Field is required";
                    patientNameTextBox.ValidationSettings.Display = Display.Dynamic;
                    // pholder.Controls.Add(patientNameTextBox);
                    nameItem.Controls.Add(patientNameTextBox); // add control 
                    lg.Items.Add(nameItem);

                    LayoutItem reasonItem = new LayoutItem()
                    {
                        Caption = $"Reason for unavailability",
                    };
                    reasonItem.CaptionStyle.Font.Size = System.Web.UI.WebControls.FontUnit.Point(11);
                    ASPxComboBox comboBox = new ASPxComboBox()
                    {
                        ID = $"ReasonComboBox_{itemIndex}", // id needs to be unique so can get value in js when submitting as dynamically created
                        ClientInstanceName = $"ReasonComboBox_{itemIndex}", // id needs to be unique so can get value in js when submitting as dynamically created
                        DataSource = UnAvailableReason, // datasource here etc
                        TextField = "ReasonText", // whatever it is here
                        ValueField = "ReasonUnavailableID" // whatever it is here
                    };

                    comboBox.ValueType = typeof(int);
                    //comboBox.Items.Insert(0, new ListEditItem("-- Select Reason --", null));
                    comboBox.ValidationSettings.RequiredField.IsRequired = true;
                    comboBox.ValidationSettings.ValidationGroup = "CaseNoteVal1"; // IMPORTANT to give a validation group to the submit button and all editors to have the same validation group.
                    comboBox.ValidationSettings.RequiredField.ErrorText = "Field is required";
                    comboBox.ValidationSettings.Display = Display.Dynamic;
                    comboBox.AutoPostBack = false;
                    comboBox.DataBind();
                    reasonItem.Controls.Add(comboBox); // add the control

                    // add layoutitems to group

                    lg.Items.Add(reasonItem);

                    // add group to form
                    UnavailabilityFormLayout.Items.Add(lg);

                }
                tab.Controls.Add(UnavailabilityFormLayout);
                PageControl.TabPages.Add(tab);
            }
        }
        private void getAuditClinicAnswer(int rowID, int SessionID)
        {
            AuditClinicAnswersBO FullAuditClincAnswer = new AuditClinicAnswersBO();
            FullAuditClincAnswer = new BLL.AuditClinicAnswersBLL().GetAuditClinicAnswer(rowID, SessionID);
            //TextBox1.Text = FullAuditClincAnswer[0].ClinicCode;
            txtClinicCode.Text = FullAuditClincAnswer.ClinicCode;
            txtAuditClinicAnswerId.Value = FullAuditClincAnswer.AuditClinicAnswersID;
            txtNumAppointments.Text = FullAuditClincAnswer.NumberOfAppointmentsAllocated.ToString();
            txtStartCount.Text = FullAuditClincAnswer.CaseNotesAvailableStartCount.ToString();
            txtTempNotesCount.Text = FullAuditClincAnswer.TemporaryNotesCount.ToString();
            //txtCaseNoteCount.Text = FullAuditClincAnswer[0]..ToString();
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

        protected void Audit_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var obj = e.InputParameters["Audit"] as AuditBO;
            short userID = Login.CookieHelper.GetCookieUserID();
            obj.CreatedByUserID = userID;
        }

        protected void Audit_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var obj = e.InputParameters["Audit"] as AuditBO;
            short userID = Login.CookieHelper.GetCookieUserID();
            obj.CreatedByUserID = userID;
            //obj.Date = DateTime.Now;
            obj.StatusID = 1;
        }

        protected void CaseNoteAvailabilityAuditRecordsGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            GridViewLayoutGroup group = grid.EditFormLayoutProperties.FindItemOrGroupByName("FieldGroup") as GridViewLayoutGroup;
            group.Caption = "Create new Audit";
            grid.SettingsCommandButton.UpdateButton.Text = "Create new Audit";

            // hide audit column on new row
            GridViewColumnLayoutItem auditCol = grid.EditFormLayoutProperties.FindItemOrGroupByName("AuditID") as GridViewColumnLayoutItem;
            auditCol.Visible = false;

        }

        protected void CaseNoteAvailabilityAuditRecordsGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
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

        protected void CaseNoteAvailabilityAuditRecordsGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName == "Date" || e.Column.FieldName == "DueByDate")
            //{
            //    var date = e.Editor as ASPxDateEdit;
            //    date.MinDate = DateTime.Now.Date;
            //}
            ASPxGridView gridView = sender as ASPxGridView;
            if (e.Column.FieldName == "TemporaryNotesCount")
            {
                ASPxTextBox fireDateEditor = e.Editor as ASPxTextBox;
                fireDateEditor.ClientEnabled = !CaseNoteAvailabilityAuditRecordsGridView.IsNewRowEditing;
                //fireDateEditor.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;
                fireDateEditor.ClientSideEvents.ValueChanged = "onDismissalDateChanged";

            }
        }

        protected void CaseNoteAvailabilityAuditRecordsView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["CSAAuditId"] = CASAuditId;
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }
        protected void UnavailableCasenotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //ASPxGridView gridView = sender as ASPxGridView;

            //object[] values = gridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];
            //e.InputParameters["CSAAuditId"] = CASAuditId;
        }


        protected void DefaultPageTitleLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;
            lbl.Text = $"In Progress Audit: ID <span class='MainColour'>{CASAuditId}</span>";
            //Speciality
        }
        protected void CasenoteLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl1 = sender as ASPxLabel;
            lbl1.Text = $"<span class='AuditDetailsCaption'>Audit Date:</span><span class='AuditDetailsDetail'>{AuditDate.ToShortDateString()}</span>";

            //Speciality
        }
        protected void lblSpeciality_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl2 = sender as ASPxLabel;
            lbl2.Text = $"<span class='AuditDetailsCaption'>Speciality:</span><span class='AuditDetailsDetail'>{Specialty}</span>";
        }
        protected void lblSite_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl2 = sender as ASPxLabel;
            lbl2.Text = $"<span class='AuditDetailsCaption'>Site:</span><span class='AuditDetailsDetail'>{AuditSite}</span>";
        }

        protected void CompleteButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ Complete_Click(s, e); }}");

        }

        protected void CaseNoteAvailabilityAuditRecordsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //e.Parameters = new Dictionary<string, object>();
            if (e.Parameters != null) // select patient referrals & details sent from patient search grid
            {
                string eventArgument = e.Parameters;
                //if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Trim() !="[]")                
                if (e.Parameters != null)
                {
                    // Deserialize the JSON string back to an array
                    AuditClinicAnswersUnAvailableBO AuditClinicAnswers = new AuditClinicAnswersUnAvailableBO();
                    List<CompleteCallbackBO> myArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompleteCallbackBO>>(e.Parameters);
                    // Use the array on server side
                    AuditClinicAnswers.UnavailableList = myArray;                  
                    AuditClinicAnswers.AuditClinicAnswersID = Convert.ToInt32(txtAuditClinicAnswerId.Value);
                    AuditClinicAnswers.AuditID = Convert.ToInt32(txtAuditId.Value);
                    AuditClinicAnswers.ClinicCode = txtClinicCode.Text;
                    AuditClinicAnswers.Totalappointments = Convert.ToInt32(txtTotalAppointments.Value);
                    AuditClinicAnswers.NumberOfAppointmentsAllocated = Convert.ToInt32(txtNumAppointments.Value);
                    AuditClinicAnswers.CaseNotesAvailableStartCount = Convert.ToInt32(txtStartCount.Value);
                    AuditClinicAnswers.TemporaryNotesCount = Convert.ToInt32(txtTempNotesCount.Value);
                    AuditClinicAnswers.UnavailableCount = Convert.ToInt32(txtUnavailableCaseNoteCount.Value);

                    txtAuditClinicAnswerId.Text = "";
                    txtAuditId.Text = "";
                    txtTotalAppointments.Text = "";
                    txtNumAppointments.Text = "";
                    txtStartCount.Text = "";
                    txtTempNotesCount.Text = "";
                    txtUnavailableCaseNoteCount.Text = "";

                    int nreturn = new BLL.AuditClinicAnswersBLL().SaveCaseNoteAvailability(AuditClinicAnswers, CookieHelper.GetCookieSessionID());

                    if (nreturn == 0)
                    {
                        CaseNoteAvailabilityAuditRecordsGridView.JSProperties["cpPopupUpdatedPending"] = true;
                    }
                    else
                    {
                        CaseNoteAvailabilityAuditRecordsGridView.JSProperties["cpPopupUpdated"] = true;
                    }

                }
            }
        }
    }
}
