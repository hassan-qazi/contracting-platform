using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContractingPlatform.Models
{
    public class Advisor: IAdvisor
    {
        public int AdvisorId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        //[RegularExpression(@"^[0-9]*$")]
        [RegularExpression(@"[0-9]{10}")]        
        public string Phone { get; set; }
        public string Health { get; set; }

        public string getHealth() {
            Random rnd = new Random();
            int probability  = rnd.Next(1, 11); // random number in the range 1 to 10 inclusive
            return probability < 4 ? "Red" : "Green"; // Health is Red when we get either a 1,
                                                      // 2 or 3 otherwise its Green
        }

        public Advisor() {
            this.Health = getHealth();
        }
    }

    public interface IAdvisor {
        string getHealth();
    }


    
}