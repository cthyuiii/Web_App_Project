using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Competition
    {
        [Display(Name = "ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Area of Interest ID")]
        public int AreaInterestID { get; set; }

        [Display(Name = "Competition Name")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters!")]
        public string CompetitionName { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Result Released Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ResultReleasedDate { get; set; }
    }
}
