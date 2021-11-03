using System;
using System.ComponentModel.DataAnnotations;
namespace Go.Shared.Forms.FormModels
{
    public class BoardOptions
    {
        [Required]
        public String dimensions { get; set; } = "19";
    }
}
