using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContractingPlatform.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        [Display(Name = "Entity A")]
        public string EntityA {get; set; }
        [Display(Name = "Entity B")]
        public string EntityB {get; set; }        
    }
}