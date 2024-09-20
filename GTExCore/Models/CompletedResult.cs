using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class CompletedResult
    {
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string MarketBookname { get; set; }
        public string Winnername { get; set; }
        public string EventType { get; set; }
    }
}