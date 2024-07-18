using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class ErrorLog
    {
        public ErrorLog(Exception ex)
        {
            ExType = ex.GetType().ToString().Length > 100 ? ex.GetType().ToString().Substring(0, 99) : ex.GetType().ToString();
            Message = ex.Message.Length > 250 ? ex.Message.Substring(0, 249) : ex.Message.ToString();
            StackTrace = ex.StackTrace.Length > 2000 ? ex.StackTrace.Substring(0, 1999) : ex.StackTrace;
            Source = ex.Source.Length > 100 ? ex.Source.Substring(0, 99) : ex.Source;
            LoggedTime = System.DateTime.Now;
        }

        public int? SessionID { get; set; }
        public string ExType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public DateTime LoggedTime { get; set; }
    }
}