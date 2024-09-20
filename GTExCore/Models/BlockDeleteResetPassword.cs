using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Models
{
    public class BlockDeleteResetPassword
    {
        public bool CanResetPassword { get; set; }
        public bool CanBlockUser { get; set; }
        public bool CanDeleteUser { get; set; }
        public int UserType { get; set; }

    }
}