using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WebApplication.DAL;

namespace WebApplication.Models
{
    public class ValidateCompetitorEmail : ValidationAttribute
    {
        private CompetitorDAL competitorContext = new CompetitorDAL();

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Competitor" model class
            Competitor competitor = (Competitor)validationContext.ObjectInstance;
            // Get the Competitor Id from the competitor instance
            int competitorId = competitor.CompetitorID;

            if (competitorContext.IsEmailExist(email, competitorId))
                // validation failed
                return new ValidationResult
                ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
