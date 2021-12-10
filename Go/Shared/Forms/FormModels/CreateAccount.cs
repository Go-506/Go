using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using Go.Shared.Models.MongoDB;
using Go.Shared.Models;

namespace Go.Shared.Forms.FormModels
{
    public class CreateAccount
    {
        [Required]
        [UniqueName]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [UniqueEmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
    // Custom Validation attribute that checks the db and makes sure that the primary key username is unique.
    public class UniqueName: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = (string)value;
            IMongoCollection<IUser> coll = Globals.DB.GetCollection<IUser>("users");
            IUser alreadyThere = coll.Find(x => x.name.Equals(name)).FirstOrDefault();
            if (alreadyThere == null)
            {
                return ValidationResult.Success;
            } else
            {
                return new ValidationResult("Username must be unique.");
            }

        }
    }

    public class UniqueEmailAddress: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = (string)value;
            IMongoCollection<IUser> coll = Globals.DB.GetCollection<IUser>("users");
            IUser alreadyThere = coll.Find(x => x.email.Equals(email)).FirstOrDefault();
            if (alreadyThere == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Email must be unique.");
            }

        }
    }
}
