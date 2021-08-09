using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class AreaInterestViewModel
    {
        public List<AreaInterest> areaofinterestnameList { get; set; }
        public List<Competition> competitionList { get; set; }
        public AreaInterestViewModel()
        {
            areaofinterestnameList = new List<AreaInterest>();
            competitionList = new List<Competition>();
        }
    }
}
