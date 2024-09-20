using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class SP_GetTVLinks_Result
    {
        public int ID { get; set; }
        public string tvlink { get; set; }
        public string scorecard { get; set; }
        public string EventID { get; set; }
    }
}