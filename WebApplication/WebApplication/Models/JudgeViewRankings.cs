using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class JudgeViewRankings
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Competitor ID")]
        public int CompetitorID { get; set; }
        [Display(Name = "Competition Name")]
        public string CompetitionName { get; set; }
        [Display(Name = "Criteria ID")]
        public int CriteriaID { get; set; }
        [Display(Name = "Criteria Name")]
        public string CriteriaName { get; set; }

        [Display(Name = "File name")]
        [StringLength(255, ErrorMessage = "Filename cannot exceed 255 characters!")]
        public string FileSubmitted { get; set; }

        [Display(Name = "Date of file upload")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateTimeFileUpload { get; set; }

        [StringLength(255, ErrorMessage = "Appeal cannot exceed 255 characters!")]
        public string Appeal { get; set; }
        public int Scores { get; set; }
        [Display(Name ="Total Mark")]
        public double totalMark { get; set; }
        [Display(Name = "Result Released Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime? ResultReleasedDate { get; set; }

        public int? VoteCount { get; set; }
        public int Weightage { get; set; }

        public int? Ranking { get; set; }
        public bool markSet { get; set; }
        public List<JudgeViewRankings> rankingDetails { get; set; }
        public List<JudgeViewRankings> scoresList { get; set; }
        public List<JudgeViewRankings> weightageList { get; set; }
        public JudgeViewRankings()
        {
            rankingDetails = new List<JudgeViewRankings>();
            scoresList = new List<JudgeViewRankings>();
            weightageList = new List<JudgeViewRankings>();
        }
    }
}
