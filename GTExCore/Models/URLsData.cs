using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTExCore.Models
{
    public partial class URLsData
    {
        public int ID { get; set; }
        [StringLength(300)]
        public string URLForData { get; set; }
        [StringLength(50)]
        public string EventType { get; set; }
        [StringLength(100)]
        public string Scd { get; set; }
        [StringLength(50)]
        public string GetDataFrom { get; set; }
    }
}
