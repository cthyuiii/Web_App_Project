using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class AreaInterest
    {
        [Display(Name = "ID")]
        public int AreaInterestID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters!")]
        public string Name { get; set; }
    }
}
