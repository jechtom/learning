using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.DA
{
    public class AppDbContext : DbContext
    {
        public DbSet<AccessToken> Tokens { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionsGroup> QuestionsGroups { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<GivenAnswer> GivenAnswers { get; set; }
    }

}
