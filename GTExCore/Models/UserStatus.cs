using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class UserStatus
    {
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public bool Loggedin { get; set; }
        public bool GTLogin { get; set; }
        public long CurrentLoggedInID { get; set; }
    }
}