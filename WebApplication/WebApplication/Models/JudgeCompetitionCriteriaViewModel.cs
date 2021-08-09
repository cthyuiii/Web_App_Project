using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class JudgeCompetitionCriteriaViewModel
    {
        public List<CompetitionViewModel> competitionList { get; set; }
        public List<Criteria> criteriaList { get; set; }
        public int TWeightage { get; set; }
        public bool IsComplete { get; set; }

        public JudgeCompetitionCriteriaViewModel()
        {
            competitionList = new List<CompetitionViewModel>();
            criteriaList = new List<Criteria>();
        }
    }
}
