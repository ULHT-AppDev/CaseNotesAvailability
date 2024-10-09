using BLL;
using BusinessObjects;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Web;
using DevExpress.Web.Internal.Dialogs;
using DevExpress.XtraRichEdit.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CaseNotesAvailability
{
    public partial class _CaseNoteAvailabilityAudit : System.Web.UI.Page
    {
        // in memory 
        private int CASAuditId;
        private string Speciality;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            int lAuditId = Convert.ToInt32(Request.QueryString["AuditID"]);
            AuditClinicAnswersBLL loginBLL = new AuditClinicAnswersBLL();
            SetAuditID(lAuditId);
            string Speciality = Request.QueryString["Speciality"];

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SetAuditID(int auditID)
        {
            this.CASAuditId = auditID;
        }

        protected void AuditorView_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = CaseNoteAvailabilityAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "ClinicCode", "AuditClinicAnswersID" }) as object[];

            if (values != null)
            {
                string ClinicCode = values[0]?.ToString() ?? "";
                string AuditID = values[1]?.ToString() ?? "";


                //if (!String.IsNullOrEmpty(AuditID))
                //{
                //    AuditID = HttpUtility.JavaScriptStringEncode(AuditID);
                //}

                btn.ClientSideEvents.Click = String.Format("function(s, e) {{ AuditorView_ClientClick(s, e, '{0}', '{1}'); }}", ClinicCode, AuditID);
                //btn.Click += new System.EventHandler(this.Button_Click);

            }
            else
            {
                btn.Visible = false;
            }
        }

        protected void CaseNoteAvailabilityUnAvailabilityCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter != null) // select patient referrals & details sent from patient search grid
            {
                UnAvailabilityCallbackBO obj = Newtonsoft.Json.JsonConvert.DeserializeObject<UnAvailabilityCallbackBO>(e.Parameter);

                string ClinicCode = obj.ClinicCode;    
                int AuditClinicAnswerId = obj.AuditClinicAnswerId;
                getAuditClinicAnswer(AuditClinicAnswerId);
            }

        }

        protected void CreateFormDynamically_CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && int.TryParse(e.Parameter, out int iterations))
            {

                List<ReasonUnavailableBO> UnAvailableReason = new List<ReasonUnavailableBO>();
                UnAvailableReason = new BLL.UnavailableCaseNotesBLL().GetUnAvailableReasons();


                //ASPxFormLayout form = new ASPxFormLayout()
                //{
                //    AlignItemCaptions = true,
                //    Width = Unit.Percentage(100)
                //};
                //form.ID = "UnavailabilityFormLayout";
                //form.ClientInstanceName= "UnavailabilityFormLayout";

                for (int i = 1; i < (iterations + 1); i++)
                {

                    LayoutGroup lg = new LayoutGroup()
                    {
                        Caption = $"Details for Patient {i}",
                        GroupBoxDecoration = GroupBoxDecoration.Box,
                        AlignItemCaptions = true,
                    };

                    LayoutItem nameItem = new LayoutItem()
                    {
                        Caption = $"Patient Name"
                    };

                    ASPxTextBox patientNameTextBox = new ASPxTextBox();
                    patientNameTextBox.ID = $"PatientNameTextBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
                    patientNameTextBox.ClientInstanceName = $"PatientNameTextBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created

                    // add in validation settings and stuff here like below one (look at combo below)

                    nameItem.Controls.Add(patientNameTextBox); // add control 

                    LayoutItem reasonItem = new LayoutItem()
                    {
                        Caption = $"Reason for unavailable"
                    };

                    ASPxComboBox comboBox = new ASPxComboBox();
                    comboBox.ID = $"ReasonComboBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
                    comboBox.ClientInstanceName = $"ReasonComboBox_{i}"; // id needs to be unique so can get value in js when submitting as dynamically created
                    comboBox.DataSource = UnAvailableReason; // datasource here etc
                    comboBox.TextField = "ReasonText"; // whatever it is here
                    comboBox.ValueField = "ReasonUnavailableID"; // whatever it is here
                    comboBox.DataBind();

                    comboBox.ValidationSettings.RequiredField.IsRequired = true;
                    comboBox.ValidationSettings.RequiredField.ErrorText = "Field is required";
                    comboBox.ValidationSettings.Display = Display.Dynamic;
                    comboBox.ValidationSettings.ValidationGroup = ""; // IMPORTANT to give a validation group to the submit button and all editors to have the same validation group.

                    reasonItem.Controls.Add(comboBox); // add the control

                    // add layoutitems to group
                    lg.Items.Add(nameItem);
                    lg.Items.Add(reasonItem);

                    // add group to form
                    UnavailabilityFormLayout.Items.Add(lg);
                }

                // finally add form to page (div)
                // DynamicFormContainer.Controls.Add(form);

            }

        }


        private void getAuditClinicAnswer(int rowID)
        {
            AuditClinicAnswersBO FullAuditClincAnswer = new AuditClinicAnswersBO();
            FullAuditClincAnswer = new BLL.AuditClinicAnswersBLL().GetAuditClinicAnswer(rowID);
            //TextBox1.Text = FullAuditClincAnswer[0].ClinicCode;
            lblClinicCode1.Text = FullAuditClincAnswer.ClinicCode;
            txtAuditClinicAnswerId.Value = FullAuditClincAnswer.AuditClinicAnswersID;
            txtNumAppointments.Text = FullAuditClincAnswer.NumberOfAppointmentsAllocated.ToString();
            txtStartCount.Text = FullAuditClincAnswer.CaseNotesAvailableStartCount.ToString();
            txtTempNotesCount.Text = FullAuditClincAnswer.TemporaryNotesCount.ToString();
            //txtCaseNoteCount.Text = FullAuditClincAnswer[0]..ToString();
        }

        protected void ChooseUserButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = CaseNoteAvailabilityAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];


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
                            //btn.Text = "Not Started";
                            btn.Text = "Edit";
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ EditRow_Click(s, e, '{0}', '{1}'); }}", values[0], container.VisibleIndex);
                            break;
                        case "2":
                            //btn.Text = "In Progress";
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
                            btn.Visible = false;
                            break;
                        case "3":
                            //btn.Text = "Pending HR review";
                            btn.Text = "Review audit";
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
                            break;
                        case "4":
                            //btn.Text = "Completed";
                            btn.Visible = false;
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
                            break;
                        default:
                            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ ChooseUserButton_Click(s, e, '{0}', '{1}'); }}", values[0], StatusID);
                            break;

                    }
                }




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

        protected void ChooseUserButton1_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            GridViewDataItemTemplateContainer container = btn.NamingContainer as GridViewDataItemTemplateContainer;

            object[] values = CaseNoteAvailabilityAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

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

            object[] values = CaseNoteAvailabilityAuditRecordsGridView.GetRowValues(container.VisibleIndex, new string[] { "AuditID", "StatusID" }) as object[];

            BLL.AuditBLL.DeleteAudit(Convert.ToInt32(values[0]), values[1].ToString());

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
            lbl.Text = $"In Progress Audit: ID <span class='MainColour'>{CASAuditId}</span>";
            //Speciality
        }
        protected void CasenoteLabel_Init(object sender, EventArgs e)
        {
            ASPxLabel lbl = sender as ASPxLabel;
            lbl.Text = $"In Progress Audit: ID <span class='MainColour'>{Speciality}</span>";

            //Speciality
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

        protected void CompleteCallback_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter != null) // select patient referrals & details sent from patient search grid
            {
                string eventArgument = e.Parameter;
                if (!string.IsNullOrEmpty(eventArgument))
                {
                    // Deserialize the JSON string back to an array
                    var myArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[][]>(eventArgument);

                    // Use the array on server side
                    List<UnavailableCaseNotesBO> UnAvailabelCaseNotes = new List<UnavailableCaseNotesBO>();
                    foreach (string[] item in myArray)
                    {
                        UnavailableCaseNotesBO UnAvailable = new UnavailableCaseNotesBO();
                        UnAvailable.AuditClinicAnswersID = Convert.ToInt32(txtAuditClinicAnswerId.Value);
                        UnAvailable.PatientDetails = item[0];
                        UnAvailable.ReasonUnavailableID = Convert.ToInt32(item[1]);
                        UnAvailabelCaseNotes.Add(UnAvailable);
                    }
                }
            }
        }

        //protected void CaseNoteAvailabilityUnAvailabilityPopup_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    FindAllControls(CaseNoteAvailabilityUnAvailabilityPopup);
        //}
        private void FindAllControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is ASPxTextBox)
                {
                    ASPxTextBox textBox = (ASPxTextBox)ctrl;
                    string value = textBox.Text;
                    // Do something with the value, e.g., display or process it
                }
                else if (ctrl.HasControls())
                {
                    // Recursively search through the child controls
                    FindAllControls(ctrl);
                }
            }
        }

        protected void CompleteButton_Init(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            btn.ClientSideEvents.Click = String.Format("function(s, e) {{ Complete_Click(s, e); }}");

        }
    }
}