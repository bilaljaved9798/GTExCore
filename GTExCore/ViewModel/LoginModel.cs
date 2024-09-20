using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTCore.ViewModel
{
    public class LoginModel
    {
        public string Username { get; set; }
        public bool isHCEUser { get; set; }
        public string Registration_Number { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        public string Origin { get; set; }
        //public Users Users;
        public string ErrorMessage;
        public bool PasswordExpired { get; set; }
    }  
}
