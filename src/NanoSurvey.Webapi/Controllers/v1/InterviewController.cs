

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanoSurvey.Data;
using NanoSurvey.Data.Repository;
using NanoSurvey.Webapi.Models;

namespace NanoSurvey.Webapi.Controllers
{

    [Route("/v1/interview")]
    public class InterviewController : Controller
    {
        private IInterviewRepository _interviewRepository;
        private IQuestionRepository _questionRepository;

        public InterviewController(IQuestionRepository questionRepository, IInterviewRepository interviewRepository)
        {
            _interviewRepository = interviewRepository;
            _questionRepository = questionRepository;
        }

        [HttpPost("{idInterview:int}/question/{idQuestion:int}")]
        public async Task<IActionResult> Post([FromRoute] AddAnswerParameter p)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new ValidationError(ModelState)) { StatusCode = (int)HttpStatusCode.BadRequest };

            await _interviewRepository.AddAnswer(p.idInterview.Value, p.idQuestion.Value, p.answers);
            var questionId = await _questionRepository.GetNextQuestionId(p.idQuestion.Value);

            return Json(questionId);
        }
    }
}