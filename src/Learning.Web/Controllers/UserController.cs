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
        public ActionResult Status([FromBody]StatusRequestModel model)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Token == model.Token);
            if (token == null) return BadRequest();

            var query = _context.QuestionsGroups.Where(qg => qg.IsEnabled && qg.Questions.Any());

            query = query
                .Include($"{nameof(QuestionsGroup.Questions)}")
                .Include($"{nameof(QuestionsGroup.Questions)}.{nameof(Question.Options)}");

            

            var result = query
                .Select(qg => new
                {
                    Group = qg,
                    GroupItems = qg.Questions.Select(q => new
                    {
                        Question = q,
                        Answer = _context.GivenAnswers.SingleOrDefault(ga => ga.Owner == token && ga.Question.Id == q.Id)
                    })
                })
                .ToArray()
                .Select(qg => new
                {
                    Name = qg.Group.Name,
                    Questions = qg.GroupItems.Select(q =>
                    new {
                        Id = q.Question.Id,
                        Content = q.Question.Content,
                        CanAnswer = q.Answer == null,
                        Options = q.Question.Options.Select(o => new
                        {
                            Content = o.Content,
                            Id = o.Id
                        }).OrderBy(o => Guid.NewGuid()).ToArray()
                    })
                });

            return Ok(new { Groups = result });
        }


    }
}
