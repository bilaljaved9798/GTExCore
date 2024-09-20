using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.ViewModel
{
    public static class CricketAPIConfig
    {
        public static string AccessKey { get; set; } = "";
        public static string SecretKey { get; set; }
        public static string AppID { get; set; }
        public static string DeviceID { get; set; }
        public static string SessionKey { get; set; }
    }
    public class AuthDescription
    {
        public bool status { get; set; }
        public string version { get; set; }
        public int status_code { get; set; }
        public string expires { get; set; }
        public Auth auth { get; set; }
        public object Etag { get; set; }
        public string cache_key { get; set; }
    }
    public class Auth
    {
        public string access_token { get; set; }
        public double expires { get; set; }
    }
}