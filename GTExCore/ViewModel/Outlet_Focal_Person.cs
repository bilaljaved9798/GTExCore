using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTCore.ViewModel
{
    [Table("Outlet_Focal_Person", Schema = "Census")]
    public partial class Outlet_Focal_Person
    {        
        public long Outlet_Focal_Persen_Seq { get; set; }
        public long FK_Census_Outlets_Master_Seq { get; set; }
        [StringLength(200)]
        public string Focal_Persen_Name { get; set; }
        [StringLength(200)]
        public string Contact_number { get; set; }
        [StringLength(200)]
        public string Designation { get; set; }
        [StringLength(200)]
        public string Email_Address { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
        public int? Created_By { get; set; }     
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }      
        public DateTime? Modified_Date { get; set; }
        public bool? is_Active { get; set; }
    }
}
