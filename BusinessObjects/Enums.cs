﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Enums
    {
        public enum UserRoles
        {
            NotSet = -1,
            Admin = 0,
        }
        public enum AuditStatus
        {
            NotStarted = 1,
            InProgress = 2,
            PendingHRreview = 3,
            Completed = 4
        }
    }
}
