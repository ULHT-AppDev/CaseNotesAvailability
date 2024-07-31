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
        protected void Audit_Updating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //var obj = e.InputParameters["Audit"] as AuditBO;
            //short test = Login.CookieHelper.GetCookieUserID();
            //obj.CreatedByUserID = (byte)test;

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

        protected void Audit_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var obj = e.InputParameters["Audit"] as AuditBO;
            short test = Login.CookieHelper.GetCookieUserID();
            obj.CreatedByUserID = (byte)test;
            //obj.Date = DateTime.Now;
        }

    }
}