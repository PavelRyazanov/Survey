using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanoSurvey.Data.Entity
{
    public class Interview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? IdSurvey { get; set; }
        [ForeignKey(nameof(IdSurvey))]
        public Survey SurveyParent { get; set; }

        [Required]
        public DateTimeOffset? Created { get; set; }

        public ICollection<Result> Results { get; set; } = new HashSet<Result>();

        // Можно собирать данные о респонденте, такие как ФИО, контакты, пол и т.п.
        // Так же опос, который можно пройти один раз, можно добавить информацию о пользователе респонденте с обязательной регистрацией/входом.
        // Для настройки этих параметров нужны флаги в "Survey".
        // В данном случае счетаем, что нам это не нужно.
    }
}