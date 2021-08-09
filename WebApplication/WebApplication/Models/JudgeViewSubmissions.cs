using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class JudgeViewSubmissions
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }
        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }
        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }
        [Display(Name = "File Submitted")]
        public string FileSubmitted { get; set; }
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }
        [Display(Name = "Criteria Name")]
        public string CriteriaName { get; set; }
        [Display(Name = "Area of Interest")]
        public string AreaInterest { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Score")]
        [Required(ErrorMessage = "Please Enter A Score")]
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10")]
        public int Score { get; set; }
        public string FilePath { get; set; }
        [Display(Name = "Result Released Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime? ResultReleasedDate { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Last Edit Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateTimeLastEdit { get; set; }
        public int TWeightage { get; set; }
        public List<Criteria> criteriaList { get; set; }
        public List<Criteria> critCheckList { get; set; }
        public List<JudgeViewSubmissions> scoreList { get; set; }
        public List<CompetitionCompetitorViewModel> competitorList { get; set; }
        public List<JudgeViewSubmissions> competitionList { get; set; }
        public List<JudgeViewSubmissions> VMCheckList { get; set; }
        public JudgeViewSubmissions()
        {
            competitorList = new List<CompetitionCompetitorViewModel>();
            scoreList = new List<JudgeViewSubmissions>();
            criteriaList = new List<Criteria>();
            competitionList = new List<JudgeViewSubmissions>();
            VMCheckList = new List<JudgeViewSubmissions>();
            critCheckList = new List<Criteria>();
        }
    }
}
