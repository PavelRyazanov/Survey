using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NanoSurvey.Data;
using NanoSurvey.Data.Entity;
using Xunit;

namespace NanoSurvey.Webapi.Test
{
    public class BaseControllerTests : IClassFixture<SurveyWebApplicationFactory>
    {
        protected readonly SurveyWebApplicationFactory _factory;

        public BaseControllerTests(SurveyWebApplicationFactory factory)
        {
            _factory = factory;
        }

        protected SurveyDbContext GetDbContext()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<SurveyDbContext>();
            return context;
        }

        protected Survey GetRandomSurvey(SurveyDbContext dbContext)
        {
            var index = RNGCryptoServiceProvider.GetInt32(0, Seed.QuantitySurveys);
            var survey = dbContext.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .Single(s => s.Id == index);
            return survey;
        }

        protected Question GetRandomSurveyQuestion(Survey survey)
        {
            var skipItems = RNGCryptoServiceProvider.GetInt32(0, survey.Questions.Count);
            var question = survey.Questions.Where(q => q.IdSurvey == survey.Id).Skip(skipItems).First();
            return question;
        }
    }
}
