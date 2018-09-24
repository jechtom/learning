using Learning.DA;
using Learning.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly AppDbContext _context;

        public UserController(IOptions<AppSettings> appSettingsOptions, AppDbContext context)
        {
            _appSettingsOptions = appSettingsOptions ?? throw new ArgumentNullException(nameof(appSettingsOptions));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        public ActionResult Profile([FromBody]ProfileModel profile)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Token == profile.Token);
            if (token == null) return BadRequest();

            if (token.ActivationDate == null) token.ActivationDate = DateTime.Now;
            token.Name = profile.Name ?? token.Name;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public ActionResult Submit([FromBody]SubmitModel model)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Token == model.Token);
            if (token == null) return BadRequest();

            GivenAnswer answer = _context.GivenAnswers.SingleOrDefault(ga => ga.Owner.Id == token.Id && ga.Question.Id == model.Id);
            if (answer != null) return BadRequest(); // already answered

            Question question = _context.Questions.Single(q => q.Id == model.Id);
            int[] answersIds = _context.QuestionOptions.Where(qo => qo.Question.Id == question.Id).Select(qo => qo.Id).ToArray();

            answer = new GivenAnswer()
            {
                Question = question,
                Owner = token,
                SelectedQuestionOptions = model.Answers.Where(a => a.IsSelected && answersIds.Contains(a.Id))
                    .Select(a => new GivenAnswerToQuestionOption()
                    {
                        QuestionOptionId = a.Id
                    }).ToList()
            };

            _context.GivenAnswers.Add(answer);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public ActionResult Status([FromBody]StatusRequestModel model)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Token == model.Token);
            if (token == null) return BadRequest();

            var query = _context.QuestionsGroups.Where(qg => qg.IsEnabled && qg.Questions.Any());

            query = query
                .Include($"{nameof(QuestionsGroup.Questions)}")
                .Include($"{nameof(QuestionsGroup.Questions)}.{nameof(Question.Options)}");



            var preData = query
                .Select(qg => new
                {
                    Group = qg,
                    GroupItems = qg.Questions.Select(q => new
                    {
                        Question = q,
                        IsAnswered = _context.GivenAnswers
                            .Any(ga => ga.Owner == token && ga.Question.Id == q.Id),
                        AnswersIds = _context.GivenAnswers
                            .Where(ga => ga.Owner == token && ga.Question.Id == q.Id)
                            .SelectMany(ga => ga.SelectedQuestionOptions.Select(gao => gao.QuestionOptionId))
                            .ToArray()
                    })
                })
                .ToArray();

             var result = preData.Select(qg => new
                {
                    Name = qg.Group.Name,
                    Questions = qg.GroupItems.Select(q =>
                    new {
                        Id = q.Question.Id,
                        Content = q.Question.Content,
                        CanAnswer = !q.IsAnswered,
                        Options = q.Question.Options.Select(o => new
                        {
                            Content = o.Content,
                            IsSelected = q.IsAnswered && q.AnswersIds.Contains(o.Id),
                            IsCorrect = q.IsAnswered && o.IsCorrect,
                            Id = o.Id
                        }).OrderBy(o => Guid.NewGuid()).ToArray()
                    })
                });

            return Ok(new { Groups = result });
        }


    }
}
