using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CompetitionJudgeViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }
        [Display(Name = "Competition Name")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters!")]
        public string CompetitionName { get; set; }
        [Display(Name = "Judge ID")]
        public int JudgeID { get; set; }
        [Display(Name = "Judge Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string JudgeName { get; set; }
    }
}
