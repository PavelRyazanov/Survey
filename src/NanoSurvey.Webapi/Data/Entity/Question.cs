using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanoSurvey.Data.Entity
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? IdSurvey { get; set; }
        [ForeignKey(nameof(IdSurvey))]
        public Survey Parent { get; set; }


        [Required]
        public int? Order { get; set; }

        [MaxLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

        [Required]
        public bool? IsRequired { get; set; }

        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}