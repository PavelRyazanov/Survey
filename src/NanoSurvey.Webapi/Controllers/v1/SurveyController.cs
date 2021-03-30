

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanoSurvey.Data;
using NanoSurvey.Data.Repository;

namespace NanoSurvey.Webapi.Controllers
{

    [Route("/v1/survey")]
    public class SurveyController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        SurveyDbContext _dbContext;

        public SurveyController(IQuestionRepository questionRepository, SurveyDbContext dbContext)
        {
            _questionRepository = questionRepository;
            _dbContext = dbContext;
        }

        [HttpGet("{idSurvey:int}/question/{idQuestion:int}")]
        public async Task<IActionResult> Get([FromRoute] GetQuestionInfoParameter p)
        {
            var questionInfo = await _questionRepository.GetQuestion(p.idSurvey.Value, p.idQuestion.Value);

            if (questionInfo is null)
                return NotFound();

            return Json(questionInfo);
        }
    }
}