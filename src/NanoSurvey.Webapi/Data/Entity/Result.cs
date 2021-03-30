using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanoSurvey.Data.Entity
{
    public class Result
    {
        [Required]
        public int? IdInterview { get; set; }

        [Required]
        public int? IdQuestion { get; set; }

        [Required]
        public int? IdAnswer { get; set; }
    }
}