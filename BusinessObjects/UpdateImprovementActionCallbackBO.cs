using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UpdateImprovementActionCallbackBO
    {
        public int ActionID { get; set; }
        public int AuditClinicAnswersID { get; set; }
        public List<ImprovementDetailsCallbackBO> ImprovementDetailsDS { get; set; }
        public List<ActionDetailsCallbackBO> ActionPointsDS { get; set; }

    }
}
