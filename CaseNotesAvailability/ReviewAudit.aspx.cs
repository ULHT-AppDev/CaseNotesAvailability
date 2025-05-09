using BLL;
using BusinessObjects;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Web;
using DevExpress.Web.Internal.Dialogs;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraRichEdit.Commands;
using Login;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static BusinessObjects.Enums;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Net.Mime.MediaTypeNames;

namespace ReviewAudit
{
    public partial class _ReviewAudit : System.Web.UI.Page
    {
        // in memory 
        private int CASAuditId;
        private string Speciality;
        private DateTime AuditDate;
        private string Specialty;
        private string AuditSite;
        //public int StatusID { get; set; }
        private string ClinicCode;
        private string AuditClinicAnswersID;


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!(CookieHelper.GetCookieRoleID() == (byte)UserRoles.HRManagers)) //If the user does not have the right to view this page, we redirect
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
                Response.End();
            }
            int lAuditId = Convert.ToInt32(Request.QueryString["AuditID"]);

            SetAuditID(lAuditId);
            //Speciality = Request.QueryString["Speciality"];
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetAuditID(int auditID)
        {
            this.CASAuditId = auditID;
            SingleAuditBO SelectedAudit = new SingleAuditBO();
            SelectedAudit = new BLL.UnavailableCaseNotesBLL().SelectedAudit(auditID);
            Speciality = SelectedAudit.Specialty;
            AuditDate = SelectedAudit.Date ?? DateTime.MinValue;
            Specialty = SelectedAudit.Specialty;
            AuditSite = SelectedAudit.Site;

        }

        protected void AddImpDetails_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewEditFormTemplateContainer container = btn.NamingContainer as GridViewEditFormTemplateContainer;

            object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "ClinicCode", "AuditID" }) as object[];

            if (values != null)
            {
                ClinicCode = values[0]?.ToString() ?? "";
                btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AddImpDetails_ClientClick(s, e, '{0}',{1}); }}", ClinicCode, '1');
            }
        }

        protected void AddActionPoint_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewEditFormTemplateContainer container = btn.NamingContainer as GridViewEditFormTemplateContainer;

            object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "ClinicCode", "AuditID" }) as object[];

            if (values != null)
            {
                ClinicCode = values[0]?.ToString() ?? "";
                btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AddImpDetails_ClientClick(s, e, '{0}',{1}); }}", ClinicCode, 2);
            }
        }

        protected void AuditorView_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "ClinicCode", "AuditClinicAnswersID", "AuditID", "IsReviewed" }) as object[];

            if (values != null)
            {
                string ClinicCode = values[0]?.ToString() ?? "";
                string AuditClinicAnswersID = values[1]?.ToString() ?? "";
                string AuditID = values[2]?.ToString() ?? "";
                bool isReviewed = Convert.ToBoolean(values[3]?.ToString() ?? "");
                switch (isReviewed)
                {
                    case true:
                        btn.Visible = false;
                        break;
                    default:
                        btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AuditorView_ClientClick(s, e, '{0}', '{1}','{2}','{3}'); }}", ClinicCode, AuditClinicAnswersID, AuditID, container.VisibleIndex);
                        break;
                }
            }
            else
            {
                btn.Visible = false;
            }
        }

        //protected void CaseNoteAvailabilityUnAvailabilityCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != null) // select patient referrals & details sent from patient search grid
        //    {
        //        UnAvailabilityCallbackBO obj = Newtonsoft.Json.JsonConvert.DeserializeObject<UnAvailabilityCallbackBO>(e.Parameter);

        //        string ClinicCode = obj.ClinicCode;    
        //        int AuditClinicAnswerId = obj.AuditClinicAnswerId;
        //        int AuditID = obj.AuditID;
        //        txtAuditId.Value = AuditID;
        //        getAuditClinicAnswer(AuditClinicAnswerId);
        //    }
        //}

        //protected void CreateFormDynamically_CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != null && int.TryParse(e.Parameter, out int iterations))
        //    {

        //        List<ReasonUnavailableBO> UnAvailableReason = new List<ReasonUnavailableBO>();
        //        UnAvailableReason = new BLL.UnavailableCaseNotesBLL().GetUnAvailableReasons();
        //        //ASPxFormLayout form = new ASPxFormLayout()
        //        //{
        //        //    AlignItemCaptions = true,
        //        //    Width = Unit.Percentage(100)
        //        //};
        //        //form.ID = "UnavailabilityFormLayout";
        //        //form.ClientInstanceName= "UnavailabilityFormLayout";

        //        for (int i = 1; i < (iterations + 1); i++)
        //        {

        //            LayoutGroup lg = new LayoutGroup()
        //            {
        //                Caption = $"Details for Patient {i}",
        //                GroupBoxDecoration = GroupBoxDecoration.Box,
        //                AlignItemCaptions = true,
        //            };

        //            LayoutItem nameItem = new LayoutItem()
        //            {
        //                Caption = $"Patient Name"
        //            };

        //            ASPxTextBox patientNameTextBox = new ASPxTextBox();
        //            patientNameTextBox.ID = $"PatientNameTextBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
        //            patientNameTextBox.ClientInstanceName = $"PatientNameTextBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created

        //            // add in validation settings and stuff here like below one (look at combo below)

        //            nameItem.Controls.Add(patientNameTextBox); // add control 

        //            LayoutItem reasonItem = new LayoutItem()
        //            {
        //                Caption = $"Reason for unavailable"
        //            };

        //            ASPxComboBox comboBox = new ASPxComboBox();
        //            comboBox.ID = $"ReasonComboBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
        //            comboBox.ClientInstanceName = $"ReasonComboBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
        //            comboBox.DataSource = UnAvailableReason; // datasource here etc
        //            comboBox.TextField = "ReasonText"; // whatever it is here
        //            comboBox.ValueField = "ReasonUnavailableID"; // whatever it is here
        //            comboBox.DataBind();

        //            comboBox.ValidationSettings.RequiredField.IsRequired = true;
        //            comboBox.ValidationSettings.RequiredField.ErrorText = "Field is required";
        //            comboBox.ValidationSettings.Display = Display.Dynamic;
        //            comboBox.ValidationSettings.ValidationGroup = ""; // IMPORTANT to give a validation group to the submit button and all editors to have the same validation group.

        //            reasonItem.Controls.Add(comboBox); // add the control

        //            // add layoutitems to group
        //            lg.Items.Add(nameItem);
        //            lg.Items.Add(reasonItem);

        //            // add group to form
        //            UnavailabilityFormLayout.Items.Add(lg);
        //        }

        //        // finally add form to page (div)
        //        // DynamicFormContainer.Controls.Add(form);

        //    }

        //}

        // - comment
        //private void getAuditClinicAnswer(int rowID)
        //{
        //    AuditClinicAnswersBO FullAuditClincAnswer = new AuditClinicAnswersBO();
        //    FullAuditClincAnswer = new BLL.AuditClinicAnswersBLL().GetAuditClinicAnswer(rowID);
        //    //TextBox1.Text = FullAuditClincAnswer[0].ClinicCode;
        //    //txtClinicCode.Text = FullAuditClincAnswer.ClinicCode;
        //    //            txtAuditClinicAnswerId.Value = FullAuditClincAnswer.AuditClinicAnswersID;
        //    //txtNumAppointments.Text = FullAuditClincAnswer.NumberOfAppointmentsAllocated.ToString();
        //    //txtStartCount.Text = FullAuditClincAnswer.CaseNotesAvailableStartCount.ToString();
        //    //txtTempNotesCount.Text = FullAuditClincAnswer.TemporaryNotesCount.ToString();
        //    //txtCaseNoteCount.Text = FullAuditClincAnswer[0]..ToString();
        //}
        // - comment
        //protected void EditUserButton_Init(object sender, EventArgs e)
        //{
        //    ASPxButton btn = sender as ASPxButton;
        //    GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

        //    object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];


        //    if (values != null)
        //    {
        //        string AuditID = values[0]?.ToString() ?? "";
        //        string StatusID = values[1]?.ToString() ?? "";


        //        if (!String.IsNullOrEmpty(AuditID))
        //        {
        //            AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
        //        }

        //        if (!String.IsNullOrEmpty(StatusID))
        //        {
        //            StatusID = HttpUtility.JavaScriptStringEncode(StatusID);

        //            switch (StatusID)
        //            {
        //                case "1":
        //                    //btn.Text = "Not Started";
        //                    btn.Text = "Edit";
        //                    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ EditRow_Click(s, e, '{0}', '{1}'); }}", values[0], container.VisibleIndex);
        //                    break;
        //                case "2":
        //                    //btn.Text = "In Progress";
        //                    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
        //                    btn.Visible = false;
        //                    break;
        //                case "3":
        //                    //btn.Text = "Pending HR review";
        //                    btn.Text = "Review audit";
        //                    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
        //                    break;
        //                case "4":
        //                    //btn.Text = "Completed";
        //                    btn.Visible = false;
        //                    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
        //                    break;
        //                default:
        //                    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
        //                    break;

        //            }
        //        }




        //        // btn.Click += new System.EventHandler(this.Button_Click);

        //    }
        //    else
        //    {
        //        btn.Visible = false;
        //    }
        //}

        protected void NewRef_Init(object sender, EventArgs e)
        {

            ASPxButton btn = sender as ASPxButton;
            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ NewRef_Init(s, e); }}");

        }

        protected void ChooseUserButton1_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

            if (values != null)
            {
                string AuditID = values[0]?.ToString() ?? "";
                string StatusID = values[1]?.ToString() ?? "";


                if (!String.IsNullOrEmpty(AuditID))
                {
                    AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
                }

                if (!String.IsNullOrEmpty(StatusID))
                {
                    StatusID = HttpUtility.JavaScriptStringEncode(StatusID);
                    switch (StatusID)
                    {
                        case "1":
                            btn.Text = "Delete";
                            //btn.ClientSideEvents.Click = String.Format("function(s, e) {{ DeleteRow_Click(s, e, '{0}', '{1}'); }}", values[0], container.VisibleIndex);
                            break;
                        default:
                            btn.Visible = false;
                            break;

                    }
                }
                btn.Click += Btn_Click;

                //btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton1_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
            }
            else
            {
                btn.Visible = false;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

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

                // test 
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

        protected void ReviewAuditRecordsGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            GridViewLayoutGroup group = grid.EditFormLayoutProperties.FindItemOrGroupByName("FieldGroup") as GridViewLayoutGroup;
            group.Caption = "Create new Audit";
            grid.SettingsCommandButton.UpdateButton.Text = "Create new Audit";

            // hide audit column on new row
            //GridViewColumnLayoutItem auditCol = grid.EditFormLayoutProperties.FindItemOrGroupByName("AuditID") as GridViewColumnLayoutItem;
            //auditCol.Visible = false;

        }

        protected void ReviewAuditRecordsGridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
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

        protected void ReviewAuditRecordsGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
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
                fireDateEditor.ClientEnabled = !ReviewAuditRecordsGridView.IsNewRowEditing;
                //fireDateEditor.ClearButton.DisplayMode = ClearButtonDisplayMode.OnHover;
                fireDateEditor.ClientSideEvents.ValueChanged = "onDismissalDateChanged";

            }
        }

        protected void ReviewAuditRecordsView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            int lAuditId = Convert.ToInt32(Request.QueryString["AuditID"]);
            e.InputParameters["CSAAuditId"] = lAuditId;
            e.InputParameters["SessionID"] = CookieHelper.GetCookieSessionID();
        }

        protected void UnavailableCasenotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //ASPxGridView gridView = sender as ASPxGridView;

            //object[] values = gridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];
            //e.InputParameters["CSAAuditId"] = CASAuditId;
        }
        protected void GoToButton_Init(object sender, EventArgs e)
        {
        }
        protected void GoToLoadingPanel_Init(object sender, EventArgs e)
        {
        }
        protected void DefaultPageTitleLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;
            lbl.Text = $"Reviewing Audit: ID <span class='MainColour'>{CASAuditId}</span>";
            //Speciality
        }
        protected void ClinicCodeLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;

            GridViewEditFormTemplateContainer container = lbl.NamingContainer as GridViewEditFormTemplateContainer;


            var code = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, "ClinicCode");

            if (code != null)
            {
                ClinicCode = code?.ToString() ?? "Error getting clinic code";
                lbl.Text = $"Add review for clinic code: <strong class='MainColour'>{ClinicCode}</strong>";
            }


        }
        protected void CompleteAuditReview_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;

            GridViewEditFormTemplateContainer container = btn.NamingContainer as GridViewEditFormTemplateContainer;

            var code = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, "AuditClinicAnswersID");

            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ CompleteAuditReview_Click(s, e, '{0}'); }}", code);

        }


        //protected void LabelRead_Init(object sender, EventArgs e)
        //{
        //    ASPxLabel lbl = sender as ASPxLabel;

        //    GridViewEditFormTemplateContainer container = lbl.NamingContainer as GridViewEditFormTemplateContainer;


        //    var code = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, "ClinicCode");

        //    if (code != null)
        //    {
        //        ClinicCode = code?.ToString() ?? "Error getting clinic code";
        //        lbl.Text = ClinicCode;
        //    }


        //}
        //protected void LblAuditClinicAnswersID_Init(object sender, EventArgs e)
        //{
        //    ASPxLabel lbl = sender as ASPxLabel;

        //    GridViewEditFormTemplateContainer container = lbl.NamingContainer as GridViewEditFormTemplateContainer;


        //    var code = ReviewAuditRecordsGridView.GetRowValues(container.VisibleIndex, "AuditClinicAnswersID");

        //    if (code != null)
        //    {
        //        AuditClinicAnswersID = code?.ToString() ?? "Error getting clinic code";
        //        lbl.Text = AuditClinicAnswersID;
        //    }


        //}


        protected void CasenoteLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl1 = sender as ASPxLabel;
            lbl1.Text = $"<span class='ReviewingCaption'>Audit Date:</span><span class='ReviewingItemDetails'>{AuditDate.ToShortDateString()}</span>";

            //Speciality
        }
        protected void lblSpeciality_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl2 = sender as ASPxLabel;
            lbl2.Text = $"<span class='ReviewingCaption'>Speciality:</span><span class='ReviewingItemDetails'>{Speciality}</span>";
        }
        protected void lblSite_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl2 = sender as ASPxLabel;
            lbl2.Text = $"<span class='ReviewingCaption'>Site:</span><span class='ReviewingItemDetails'>{AuditSite}</span>";
        }

        //protected void CompleteButton_Click(object sender, EventArgs e)
        //{
        //    //AuditClinicAnswersBO ClinicAns = new AuditClinicAnswersBO();
        //    //ClinicAns.AuditID = Convert.ToInt32(lblClinicCode1.Text);
        //    //ClinicAns.CaseNotesAvailableStartCount = Convert.ToInt32(txtStartCount.Text);
        //    //ClinicAns.NumberOfAppointmentsAllocated = Convert.ToInt32(txtNumAppointments.Text);
        //    //ClinicAns.TemporaryNotesCount = Convert.ToInt32(txtTempNotesCount.Text);


        //    /*
        //                foreach (LayoutItemBase item in UnavailabilityFormLayout.Items)
        //                {
        //                    if (item is LayoutItem)
        //                    {
        //                        LayoutItem layoutItem = (LayoutItem)item;
        //                        Control control = layoutItem.GetNestedControl();

        //                        if (control != null)
        //                        {
        //                            // Check the type of the control and process accordingly
        //                            if (control is ASPxTextBox)
        //                            {
        //                                ASPxTextBox textBox = (ASPxTextBox)control;
        //                                string textValue = textBox.Text;
        //                                // Do something with the text value
        //                            }
        //                            else if (control is ASPxComboBox)
        //                            {
        //                                ASPxComboBox comboBox = (ASPxComboBox)control;
        //                                string selectedValue = comboBox.SelectedItem?.Value.ToString();
        //                                // Do something with the selected value
        //                            }

        //                        }
        //                    }
        //                }

        //                */
        //    // Find the ASPxCallbackPanel on the page
        //    //ASPxCallbackPanel callbackPanel = FindControl("myCallbackPanel") as ASPxCallbackPanel;

        //    //if (callbackPanel != null)
        //    //{
        //    // Find the form layout inside the callback panel

        //}




        //protected void ButtonSubmit_Click(object sender, EventArgs e)
        //{
        //    Dictionary<String, List<String>> CatValues = new Dictionary<String, List<String>>();
        //    foreach (var item in UnavailabilityFormLayout.Items)
        //        if (item is LayoutGroupBase)
        //            (item as LayoutGroupBase).ForEach(BaseItem => GetNestedControls(BaseItem, BaseItem.Caption, CatValues));
        //}

        private void ReadLayoutItemControls(LayoutItemBase layoutItem)
        {
            if (layoutItem is LayoutItem)
            {
                LayoutItem item = (LayoutItem)layoutItem;

                // Get the control within the layout item (e.g., ASPxTextBox, ASPxComboBox, etc.)
                if (item.Controls.Count > 0)
                {
                    var control = item.Controls[0]; // Assuming one control per LayoutItem

                    // Example: Checking for specific control types
                    if (control is ASPxTextBox)
                    {
                        ASPxTextBox textBox = (ASPxTextBox)control;
                        string text = textBox.Text;
                        // Perform operations with the textBox value
                    }
                    else if (control is ASPxComboBox)
                    {
                        ASPxComboBox comboBox = (ASPxComboBox)control;
                        string selectedItem = comboBox.SelectedItem.Text;
                        // Perform operations with the comboBox value
                    }
                    // Handle other control types similarly
                }
            }
        }

        //protected void CompleteCallback_Callback(object source, CallbackEventArgs e)
        //{
        //    //if (e.Parameter != null) // select patient referrals & details sent from patient search grid
        //    //{
        //    //    string eventArgument = e.Parameter;
        //    //    if (!string.IsNullOrEmpty(eventArgument))
        //    //    {
        //    //        // Deserialize the JSON string back to an array
        //    //        List<CompleteCallbackBO> myArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CompleteCallbackBO>>(e.Parameter);
        //    //        // Use the array on server side
        //    //        List<UnavailableCaseNotesBO> UnAvailabelCaseNotes = new List<UnavailableCaseNotesBO>();
        //    //        foreach (CompleteCallbackBO item in myArray)
        //    //        {
        //    //            UnavailableCaseNotesBO UnAvailable = new UnavailableCaseNotesBO();
        //    //            UnAvailable.AuditClinicAnswersID = Convert.ToInt32(txtAuditClinicAnswerId.Value);
        //    //            UnAvailable.PatientDetails = item.PatientDetails;
        //    //            UnAvailable.ReasonUnavailableID = Convert.ToInt32(item.ReasonID);
        //    //            UnAvailable.IsActive = true;
        //    //            UnAvailabelCaseNotes.Add(UnAvailable);
        //    //        }

        //    //        AuditClinicAnswersBO AuditClinicAnswers = new AuditClinicAnswersBO();
        //    //        AuditClinicAnswers.AuditClinicAnswersID = Convert.ToInt32(txtAuditClinicAnswerId.Value);
        //    //        AuditClinicAnswers.AuditID = Convert.ToInt32(txtAuditId.Value);
        //    //        AuditClinicAnswers.ClinicCode = txtClinicCode.Text;
        //    //        AuditClinicAnswers.NumberOfAppointmentsAllocated = Convert.ToInt32(txtNumAppointments.Value);
        //    //        AuditClinicAnswers.CaseNotesAvailableStartCount = Convert.ToInt32(txtStartCount.Value);
        //    //        AuditClinicAnswers.TemporaryNotesCount = Convert.ToInt32(txtTempNotesCount.Value);

        //    //        bool update =  new BLL.AuditBLL().SaveCaseNoteAvailability(AuditClinicAnswers);
        //    //        if (update) {
        //    //            bool update1 = new BLL.AuditBLL().InsertUnAvailableCaseNoteAvailability(UnAvailabelCaseNotes);
        //    //        }

        //    //    }
        //    //}
        //}

        //protected void CaseNoteAvailabilityUnAvailabilityPopup_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    FindAllControls(CaseNoteAvailabilityUnAvailabilityPopup);
        //}
        //private void FindAllControls(Control parent)
        //{
        //    foreach (Control ctrl in parent.Controls)
        //    {
        //        if (ctrl is ASPxTextBox)
        //        {
        //            ASPxTextBox textBox = (ASPxTextBox)ctrl;
        //            string value = textBox.Text;
        //            // Do something with the value, e.g., display or process it
        //        }
        //        else if (ctrl.HasControls())
        //        {
        //            // Recursively search through the child controls
        //            FindAllControls(ctrl);
        //        }
        //    }
        //}

        //protected void CompleteButton_Init(object sender, EventArgs e)
        //{
        //    ASPxButton btn = sender as ASPxButton;
        //    btn.ClientSideEvents.Click = String.Format("function(s, e) {{ Complete_Click(s, e); }}");

        //}

        protected void ReviewAuditRecordsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            //e.Parameters = new Dictionary<string, object>();
            if (e.Parameters != null) // select patient referrals & details sent from patient search grid
            {
                string eventArgument = e.Parameters;
                if (!string.IsNullOrEmpty(eventArgument))
                {
                    UpdateImprovementActionCallbackBO UpdateImprovementAction = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateImprovementActionCallbackBO>(e.Parameters);

                    int AnswerID = UpdateImprovementAction.AuditClinicAnswersID;
                    if (UpdateImprovementAction.ActionPointsDS != null || UpdateImprovementAction.ImprovementDetailsDS != null)
                    {
                        short userID = Login.CookieHelper.GetCookieUserID();
                        bool update = new ReviewAuditBLL().UpdateImprovementActionDetails(userID, UpdateImprovementAction);
                        //bool AllReviewed = new ReviewAuditBLL().CheckandUpdateAuditStatus(UpdateImprovementAction.AuditClinicAnswersID);
                        //if (AllReviewed)
                        //{

                        //}
                        //else
                        //{
                        //    grid.JSProperties["cpPopupUpdated"] = true;
                        //}
                        grid.JSProperties["cpPopupUpdated"] = true;
                        //grid.DataSource = new ReviewAuditBLL().GetAuditClinicAnswers();
                        grid.CancelEdit();
                    }
                    else
                    {

                    }
                }
            }
        }

        protected void ImprovementDetailsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                List<ImprovementDetailsCallbackBO> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImprovementDetailsCallbackBO>>(e.Parameters);

                ASPxGridView grid = sender as ASPxGridView;

                if (list != null && list.Any())
                {
                    grid.JSProperties["cpShowGrid"] = true;
                    grid.DataSource = list;
                }
                else
                {
                    grid.JSProperties["cpHideGrid"] = true;
                    grid.DataSource = null;
                }

                grid.DataBind();

                grid.JSProperties["cpDataBound"] = true;

            }
        }


        protected void ActionPointDetailsGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters != null)
            {
                List<ImprovementDetailsCallbackBO> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImprovementDetailsCallbackBO>>(e.Parameters);

                ASPxGridView grid = sender as ASPxGridView;

                if (list != null && list.Any())
                {
                    grid.JSProperties["cpShowGrid"] = true;
                    grid.DataSource = list;
                }
                else
                {
                    grid.JSProperties["cpHideGrid"] = true;
                    grid.DataSource = null;
                }

                grid.DataBind();

                grid.JSProperties["cpDataBound"] = true;

            }
        }



        protected void DeleteImpReviewButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            ASPxGridView grid = btn.NamingContainer.NamingContainer.NamingContainer as ASPxGridView;

            var id = grid.GetRowValues(container.VisibleIndex, "RequiresImprovementDetailsID")?.ToString() ?? "";

            btn.ClientSideEvents.Click = $"function(s, e) {{ DeleteImprovementReview_Click(s, e, '{id}') }}";

        }

        protected void DeleteActionReviewButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            ASPxGridView grid = btn.NamingContainer.NamingContainer.NamingContainer as ASPxGridView;

            var id = grid.GetRowValues(container.VisibleIndex, "RequiresImprovementDetailsID")?.ToString() ?? "";

            btn.ClientSideEvents.Click = $"function(s, e) {{ DeleteActionReview_Click(s, e, '{id}') }}";

        }

        //protected void btnComplete_Click(object sender, EventArgs e)
        //{
        //    //    ASPxGridView parentGrid = (ASPxGridView)sender;
        //    //    //ASPxGridView parentGrid = (ASPxGridView)Parent.FindControl("ReviewAuditRecordsGridView");

        //    //    //ASPxGridView parentGrid = (ASPxGridView)sender; // This refers to the ASPxGridView1 (parent grid)
        //    //    //GridViewEditFormTemplateContainer editForm = (GridViewEditFormTemplateContainer)parentGrid.EditForm;

        //    //    //ASPxGridView childGrid = (ASPxGridView)editForm.FindControl("ASPxGridView2");






        //    //    //GridViewEditFormTemplateContainer editTemplateContainer =
        //    //    //                                            parentGrid.FindEditFormTemplateControl("ImprovementDetailsGridView") as GridViewEditFormTemplateContainer;

        //    //    //if (editTemplateContainer != null)
        //    //    //{
        //    //    //    ASPxGridView childGrid = editTemplateContainer.FindControl("ImprovementDetailsGridView") as ASPxGridView;

        //    //    //    if (childGrid != null)
        //    //    //    {


        //    //    //    }
        //    //    //}

        //}

        protected void CompleteCallback_Callback(object source, CallbackEventArgs e)
        {


        }

        //protected void AddImpDetails_Click(object sender, EventArgs e)
        //{
        //    ReviewAuditRecordsGridView.AddNewRow();
        //    //ReviewAuditRecordsGridView

        //}


        //protected void ReviewAuditClinicsGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    //GridView view = sender as GridView;
        //    ////view.Columns["ClinicCode"]. = ClinicCode;
        //    //view.SetFocusedRowCellValue("ClinicCode", ClinicCode);
        //    ReviewAuditClinicsGridView.SetRowCellValue(e.RowHandle, ClinicCode", ClinicCode);

        //    //view.SetRowCellValue(e.RowHandle, view.Columns[0], 1);
        //    //view.SetRowCellValue(e.RowHandle, view.Columns[1], 2);
        //    //view.SetRowCellValue(e.RowHandle, view.Columns[2], 3);
        //}

        protected void Complete_Click(object sender, EventArgs e)
        {
            // Find the clicked button

            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;
            ASPxGridView gridViewRow = btn.NamingContainer.NamingContainer.NamingContainer as ASPxGridView;


            //var button = (ASPxButton)sender;

            //// Locate the parent GridView row for the clicked button
            //var gridViewRow = (GridViewRow)button.NamingContainer as GridViewDataItemTemplateContainer; 
            //GridViewDataItemTemplateContainer container = button.NamingContainer as GridViewDataItemTemplateContainer;


            // Locate the child ASPxGridView from the current row
            var childGridView = (ASPxGridView)gridViewRow.FindControl("ImprovementDetailsGridView");

            if (childGridView != null)
            {
                var dataList = new List<ImprovementDetailsCallbackBO>();

                // Loop through visible rows
                foreach (var rowIndex in Enumerable.Range(0, childGridView.VisibleRowCount))
                {
                    var row = childGridView.GetRow(rowIndex);

                    if (row != null)
                    {
                        dataList.Add(new ImprovementDetailsCallbackBO
                        {
                            //Column1 = row.Cells[0].Text,
                            //Column2 = row.Cells[1].Text,
                            //Column3 = row.Cells[2].Text,
                        });
                    }
                }

                // Process extracted data
                //foreach (var item in dataList)
                //{
                //    Debug.WriteLine($"Column1: {item.Column1}, Column2: {item.Column2}, Column3: {item.Column3}");
                //}
            }


        }


    }
}