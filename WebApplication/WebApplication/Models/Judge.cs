using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebApplication.Models
{
    public class Judge
    {
        [Display(Name = "ID")]
        public int JudgeID { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string JudgeName { get; set; }
        public string Salutation { get; set; }
        [Display(Name = "Area of Interest ID")]
        public int AreaInterestID { get; set; }
        [Display(Name = "Area of Interest")]
        public string Name { get; set; }
        [Display(Name = "E-mail Address")]
        [EmailAddress]
        // Custom Validation Attribute for checking email address exists
        [ValidateJudgeEmail]
        [Required(ErrorMessage = "Please enter an email address!")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters!")]
        public string EmailAddr { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters!")]
        public string Password { get; set; }
        [Display(Name = "Assigned Competition")]
        public List<Competition> AssignedCompetition { get; set; }
        [Display(Name = "Area of Interest")]
        public string AreaInterestName { get; set; }
    }
}
