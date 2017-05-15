using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HexaDot.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string Name => $"{FirstName} {LastName}";

        public string PendingNewEmail { get; set; }

        public IEnumerable<ValidationResult> ValidateProfileCompleteness()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (!EmailConfirmed)
            {
                validationResults.Add(new ValidationResult("Verify your email address", new[] { nameof(Email) }));
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                validationResults.Add(new ValidationResult("Enter your first name", new[] { nameof(FirstName) }));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                validationResults.Add(new ValidationResult("Enter your last name", new[] { nameof(LastName) }));
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                validationResults.Add(new ValidationResult("Add a mobile phone number", new[] { nameof(PhoneNumber) }));
            }
            else if (!PhoneNumberConfirmed)
            {
                validationResults.Add(new ValidationResult("Confirm your mobile phone number", new[] { nameof(PhoneNumberConfirmed) }));
            }
            return validationResults;
        }

        public bool IsProfileComplete()
        {
            return !ValidateProfileCompleteness().Any();
        }
    }
}
