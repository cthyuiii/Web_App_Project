using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CompetitionJudge
    {
        [Display(Name = "ID")]
        public int CompetitionID { get; set; }
        [Display(Name ="Judge ID")]
        public int JudgeID { get; set; }
    }
}
