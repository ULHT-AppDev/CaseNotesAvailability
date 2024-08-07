using BusinessObjects;
using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace CaseNotesAvailability
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
            if(e.Column.FieldName == "Date" || e.Column.FieldName == "DueByDate")
            {
                var date = e.Editor as ASPxDateEdit;
                date.MinDate = DateTime.Now.Date;
            }
        }
    }
}