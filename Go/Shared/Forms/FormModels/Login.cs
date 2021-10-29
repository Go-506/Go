using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using Go.Shared.Models.MongoDB;

namespace Go.Shared.Forms.FormModels
{
    // Allows easy input validation for the login form.
    // Because both username and password are [Required], either field being empty will make the form invalid.
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
