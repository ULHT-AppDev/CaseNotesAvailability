using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public  class SingleAuditBO
    {
        public int AuditID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public String Specialty { get; set; }
        public String Site { get; set; }
        public int StatusID { get; set; }

    }
}
