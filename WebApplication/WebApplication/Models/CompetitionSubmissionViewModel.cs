using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class CompetitionSubmissionViewModel
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "CompetitorID")]
        public int CompetitorID { get; set; }

        [Display(Name = "Competition Name")]
        public string CompetitonName { get; set; }

        public IFormFile filetoUpload { get; set; }

        [Display(Name = "File name")]
        [StringLength(255, ErrorMessage = "Filename cannot exceed 255 characters!")]
        public string FileSubmitted { get; set; }

        [Display(Name = "Date of file upload")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateTimeFileUpload { get; set; }

        [StringLength(255, ErrorMessage = "Appeal cannot exceed 255 characters!")]
        public string Appeal { get; set; }

        public int VoteCount { get; set; }

        public int? Ranking { get; set; }
    }
}
