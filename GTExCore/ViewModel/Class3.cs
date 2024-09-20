using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.ViewModel
{
    public class APIResponse
    {
        public bool status { get; set; }
        public string version { get; set; }
        public int status_code { get; set; }
        public Object data { get; set; }
        public object Etag { get; set; }
        public string cache_key { get; set; }
        public string matchkey { get; set; }
    }
}