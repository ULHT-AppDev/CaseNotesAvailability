using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class RequiresImprovementActionPointsBO
    {
        public int RequiresImprovementActionPointID { get; set; }
        public string ClinicCode { get; set; }
        public string ActionPointComment { get; set; }
        public int ReviewedByUserID { get; set; }
        public Nullable<System.DateTime> ReviewedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
