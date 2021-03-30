using System;
using NanoSurvey.Data;
using NanoSurvey.Data.Entity;

namespace NanoSurvey.Webapi.Test
{
    public static class Seed
    {
        public const int QuantitySurveys = 1000;
        public const int QuantityQuestionsPerSurvey = 15;
        public const int QuantityAnswersPerQuestion = 10;

        public static void SeedDb(SurveyDbContext dbContext)
        {
            int surveyIndex = 0;
            int questionIndex = 0;
            int answerIndex = 0;

            for (int i = 0; i < QuantitySurveys; i++)
            {
                var survey = new Survey()
                {
                    Created = DateTimeOffset.Now,
                    Description = RandomStringGenerator.GenerateRandomString(250),
                    Title = RandomStringGenerator.GenerateRandomString(60),
                    Id = ++surveyIndex,
                };

                for (int j = 0; j < QuantityQuestionsPerSurvey; j++)
                {
                    var question = new Question()
                    {
                        Id = ++questionIndex,
                        IdSurvey = survey.Id,
                        IsRequired = false,
                        Order = j,
                        Parent = survey,
                        Text = RandomStringGenerator.GenerateRandomString(45),
                    };

                    survey.Questions.Add(question);
                    dbContext.Questions.Add(question);

                    for (int z = 0; z < QuantityAnswersPerQuestion; z++)
                    {
                        var answer = new Answer()
                        {
                            Id = ++answerIndex,
                            Order = z,
                            IdQuestion = question.Id,
                            QuestionParent = question,
                            Text = RandomStringGenerator.GenerateRandomString(75),
                        };

                        question.Answers.Add(answer);
                        dbContext.Answers.Add(answer);
                    }
                }

                dbContext.Surveys.Add(survey);
            }
        }
    }
}
