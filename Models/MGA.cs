using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContractingPlatform.Models
{
    public class MGA
    {
        public int MgaId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"[0-9]{10}")] 
        public string Phone { get; set; }
    }
}