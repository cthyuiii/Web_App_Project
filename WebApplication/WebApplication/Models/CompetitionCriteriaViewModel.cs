using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CompetitionCriteriaViewModel
    {
        public List<CompetitionViewModel> competitionList { get; set; }
        public List<Criteria> criteriaList { get; set; }
        public CompetitionCriteriaViewModel()
        {
            competitionList = new List<CompetitionViewModel>();
            criteriaList = new List<Criteria>();
        }

    }
}
