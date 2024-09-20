using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class APIConfigData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Appkey { get; set; }
        public string SessionKey { get; set; }
        public string APIURL { get; set; }
        public string PoundRate { get; set; }
    }
}