using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CompetitionCompetitorViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }
        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Area of Interest")]
        public string AreaInterest { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Result Released Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ResultReleasedDate { get; set; }
        public List<CompetitionCompetitorViewModel> competitionList { get; set; }
        public List<Criteria> criteriaList { get; set; }

        public List<CompetitionCompetitorViewModel> competitorList { get; set; }
        public CompetitionCompetitorViewModel()
        {
            competitionList = new List<CompetitionCompetitorViewModel>();
            competitorList = new List<CompetitionCompetitorViewModel>();
            criteriaList = new List<Criteria>();
        }
    }
}
