using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NanoSurvey.Webapi.Models;

namespace NanoSurvey.Data.Repository
{
    public interface IQuestionRepository
    {
        Task<Question> GetQuestion(int idSurvey, int idQuestion);
        Task<int?> GetNextQuestionId(int idQuestion);
    }

    public class QuestionRepository : IQuestionRepository
    {
        SurveyDbContext _dbContext;
        public QuestionRepository(SurveyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> GetNextQuestionId(int idQuestion)
        {
            var questionInfo = _dbContext.Questions
                .Where(q => q.Id == idQuestion)
                .Select(q => new {
                    q.IdSurvey,
                    q.Order
                })
                .SingleOrDefault();
            
            var question = await _dbContext.Questions.Where(q =>
                q.IdSurvey == questionInfo.IdSurvey
                && q.Order > questionInfo.Order
            )
            .OrderBy(q => q.Order)
            .FirstOrDefaultAsync();

            return question?.Id;
        }

        public async Task<Question> GetQuestion(int idSurvey, int idQuestion)
        {
            var request = await _dbContext.Questions
                .Include(q => q.Answers)
                .Where(q => q.IdSurvey == idSurvey && q.Id == idQuestion)
                .SingleOrDefaultAsync();

            if (request is null)
                return null;

            return new Question()
            {
                Id = request.Id,
                Text = request.Text,
                Answers = request.Answers.Select(a => new Answer() {
                    Id = a.Id,
                    Text = a.Text
                }).ToList()
            };
        }
    }
}