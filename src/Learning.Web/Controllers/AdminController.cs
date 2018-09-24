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
    public class AdminController : Controller
    {
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly AppDbContext _context;

        public AdminController(IOptions<AppSettings> appSettingsOptions, AppDbContext context)
        {
            _appSettingsOptions = appSettingsOptions ?? throw new ArgumentNullException(nameof(appSettingsOptions));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        public ActionResult Update(string apiKey, [FromBody]UpdateModel model)
        {
            if(_appSettingsOptions.Value.ApiKey != apiKey)
            {
                return Forbid();
            }

            // update
            foreach (var q in model.Questions)
            {
                UpdateQuestion(q);
            }

            _context.SaveChanges();

            return Ok();
        }

        private void UpdateQuestion(UpdateModel.QuestionModel questionModel)
        {
            var questionEntity = _context.Questions
                .Include(q => q.Options)
                .SingleOrDefault(q => q.ExternalId == questionModel.Name);

            bool isNew = questionEntity == null;

            // already answered - can't update
            if (!isNew && _context.GivenAnswers.Any(a => a.Question.Id == questionEntity.Id)) return;

            if (isNew)
            {
                questionEntity = new Question()
                {
                    ExternalId = questionModel.Name,
                    Options = new List<QuestionOption>()
                };

                _context.Questions.Add(questionEntity);
            }

            questionEntity.Type = Enum.Parse<QuestionType>(questionModel.Type.Single());
            questionEntity.Content = questionModel.Question.Single();

            var options =
                questionModel.Bad.Select(b => new { correct = false, content = b })
                .Concat(questionModel.Ok.Select(b => new { correct = true, content = b }))
                .ToArray();

            var notUpdatedOptions = new HashSet<QuestionOption>(questionEntity.Options);
            foreach (var opt in options)
            {
                var optEntity = notUpdatedOptions.FirstOrDefault(o => o.Content == opt.content);
                if(optEntity != null)
                {
                    optEntity.IsCorrect = opt.correct;
                    notUpdatedOptions.Remove(optEntity);
                }
                else
                {
                    questionEntity.Options.Add(new QuestionOption()
                    {
                        IsCorrect = opt.correct,
                        Content = opt.content
                    });
                }
            }
            foreach (var toDelete in notUpdatedOptions)
            {
                questionEntity.Options.Remove(toDelete);
            }
        }
    }
}
