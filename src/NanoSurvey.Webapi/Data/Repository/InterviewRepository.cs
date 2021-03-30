using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NanoSurvey.Webapi.Models;

namespace NanoSurvey.Data.Repository
{
    public interface IInterviewRepository
    {
        Task<bool> AnswerExist(int idInterview, int idQuestion);
        Task AddAnswer(int idInterview, int idQuestion, ICollection<int> idsAnswer);
    }

    public class InterviewRepository : IInterviewRepository
    {
        SurveyDbContext _dbContext;
        public InterviewRepository(SurveyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAnswer(int idInterview, int idQuestion, ICollection<int> idsAnswer)
        {
            foreach(var idAnswer in idsAnswer)
            {
                _dbContext.Results.Add(new Entity.Result() {
                    IdInterview = idInterview,
                    IdQuestion = idQuestion,
                    IdAnswer = idAnswer,
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> AnswerExist(int idInterview, int idQuestion)
        {
            return _dbContext.Results
                .Where(r => 
                    r.IdInterview == idInterview
                    && r.IdQuestion == idQuestion)
                .AnyAsync();
        }
    }
}