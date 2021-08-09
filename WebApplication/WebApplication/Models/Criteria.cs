    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Criteria
    {
        [Display(Name ="Criteria ID")]
        public int CriteriaID { get; set; }
        [Display(Name ="Competition ID")]
        public int CompetitionID { get; set; }
        [Display(Name ="Name of Criteria")]
        [Required(ErrorMessage ="Please enter a name!")]
        [StringLength(50,ErrorMessage = "Criteria name cannot exceed 50 characters!")]
        public string CriteriaName { get; set; }
        [Display(Name ="Weightage")]
        [Required(ErrorMessage ="Please enter a weightage value (1 to 100)")]
        public int Weightage { get; set; }
    }
}
