using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using NanoSurvey.Data;
using NanoSurvey.Data.Entity;

namespace NanoSurvey.Webapi.Controllers
{
    public class AddAnswerParameter : IValidatableObject
    {
        [BindRequired]
        [FromRoute]
        public int? idInterview { get; set; }

        [BindRequired]
        [FromRoute]
        public int? idQuestion { get; set; }

        [FromBody]
        public int[] answers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: Заменить dbContext на IInterviewRepository и IQuestionRepository
            var dbContext = validationContext.GetService(typeof(SurveyDbContext)) as SurveyDbContext;

            if (dbContext is null)
            {
                yield return new ValidationResult("Неопознанная ошибка", new[] { "__all__" });
                yield break;
            }

            var interview = dbContext.Interviews.SingleOrDefault(i => i.Id == this.idInterview);
            if (interview is null)
                yield return new ValidationResult("Не определен опрос.", new[] { nameof(this.answers) });

            var question = dbContext.Questions.Where(q => 
                q.Id == idQuestion
                && q.IdSurvey == interview.IdSurvey)
                .SingleOrDefault();

            if (question is null)
                yield return new ValidationResult("Не определен вопрос.", new[] { nameof(this.answers) });

            if ((question?.IsRequired ?? false) && this.answers?.Length <= 0)
                yield return new ValidationResult("Не выбран ответ на вопрос.", new[] { nameof(this.answers) });

            var questionAnswers = dbContext.Answers.Where(a =>
                a.IdQuestion == idQuestion
                && this.answers.Contains(a.Id))
                .Select(a => a.Id)
                .ToList();

            var answersNotFromQuestion = questionAnswers.Where(a => !this.answers.Contains(a)).Any();

            if (answersNotFromQuestion)
                yield return new ValidationResult("Не выбран ответ на вопрос.", new[] { nameof(this.answers) });

            var answerExist = dbContext.Results.Any(r => r.IdInterview == idInterview && r.IdQuestion == idQuestion);
            if (answerExist)
                yield return new ValidationResult("Ответ на вопрос был дан ранее.", new[] { nameof(this.idQuestion) });
        }
    }
}