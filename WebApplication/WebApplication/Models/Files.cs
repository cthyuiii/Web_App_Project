using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Files
    {
        public int CompetitionID { get; set; }

        public string Extension { get; set; }

        public string FileName { get; set; }

        public int CompetitorID { get; set; }

    }
}
