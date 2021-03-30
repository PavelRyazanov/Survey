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
    public class SurveyControllerTests : BaseControllerTests
    {
        public SurveyControllerTests(SurveyWebApplicationFactory factory) : base(factory)
        { }

        [Fact]
        public async Task ReturnNotFoundResponseIfSurveyNotExist()
        {
            using var dbContext = GetDbContext();

            var survey = GetRandomSurvey(dbContext);
            var question = GetRandomSurveyQuestion(survey);

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/v1/survey/{Seed.QuantitySurveys + 100}/question/{question.Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnNotFoundResponseIfQuestionNotExist()
        {
            using var dbContext = GetDbContext();

            var survey = GetRandomSurvey(dbContext);
            var question = GetRandomSurveyQuestion(survey);

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/v1/survey/{survey.Id}/question/{question.Id + 1000}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnOkResponseIfSurveyAndQuestionExist()
        {
            using var dbContext = GetDbContext();

            var survey = GetRandomSurvey(dbContext);
            var question = GetRandomSurveyQuestion(survey);

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/v1/survey/{survey.Id}/question/{question.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ValidateThatResponsContainsExpectedData()
        {
            using var dbContext = GetDbContext();

            var survey = GetRandomSurvey(dbContext);
            var question = GetRandomSurveyQuestion(survey);

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/v1/survey/{survey.Id}/question/{question.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseJsonSting = await response.Content.ReadAsStringAsync();

            var responseObject = JsonSerializer.Deserialize<NanoSurvey.Webapi.Models.Question>(responseJsonSting);

            Assert.Equal(responseObject.Id, question.Id);
            Assert.Equal(responseObject.Text, question.Text);

            Assert.Equal(question.Answers.Count, responseObject.Answers.Count);

            foreach (var expectedAnswer in responseObject.Answers)
            {
                Assert.True(question.Answers.Any(a => a.Id == expectedAnswer.Id));
                Assert.True(question.Answers.Any(a => a.Text == expectedAnswer.Text));
            }
        }
    }
}
