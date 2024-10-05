using System;
using System.Collections.Generic;

namespace RoleWebApi.Models
{
    public partial class Userrole
    {
        public int RoleId { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
