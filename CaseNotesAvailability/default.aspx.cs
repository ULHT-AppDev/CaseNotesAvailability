using System;
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
        protected void Audit_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            //var obj = e.InputParameters["Application"] as ApplicationBO;
            //short test = Login.CookieHelper.GetCookieUserID();
            //obj.EditedByUserID = (byte)test;
            //obj.DateLastEdited = DateTime.Now;
        }

        protected void Audit_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            //var obj = e.InputParameters["Application"] as ApplicationBO;
            //short test = Login.CookieHelper.GetCookieUserID();
            //obj.CreatedByUserID = (byte)test;
            //obj.DateCreated = DateTime.Now;
        }

    }
}