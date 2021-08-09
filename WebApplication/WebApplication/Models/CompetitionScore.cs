using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class CompetitionScore
    {
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }
        public int Score { get; set; }
        [Display(Name = "Last Edit Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateTimeLastEdit { get; set; }
    }
}
