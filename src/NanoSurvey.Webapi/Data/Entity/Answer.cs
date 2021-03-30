using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanoSurvey.Data.Entity
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? IdQuestion { get; set; }
        [ForeignKey(nameof(IdQuestion))]
        public Question QuestionParent { get; set; }

        [Required]
        public int? Order { get; set; }

        [MaxLength(150)]
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }
}