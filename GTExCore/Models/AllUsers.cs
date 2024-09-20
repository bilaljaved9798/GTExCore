using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Models
{
    public class AllUsers
    {
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}