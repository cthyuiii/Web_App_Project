using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CriteriaScoreViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Name of Criteria")]
        public string CriteriaName { get; set; }

        [Display(Name = "Criterion Score")]
        public int Score { get; set; }
    }
}
