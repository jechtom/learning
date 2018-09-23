using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.DA
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AccessToken> Tokens { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionsGroup> QuestionsGroups { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<GivenAnswer> GivenAnswers { get; set; }
        public DbSet<GivenAnswerToQuestionOption> GivenAnswerToQuestionOption { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GivenAnswerToQuestionOption>()
                .HasKey(t => new { t.GivenAnswerId, t.QuestionOptionId });

            // delete given answer options items when deleting given answer
            modelBuilder.Entity<GivenAnswer>()
                .HasMany(t => t.SelectedQuestionOptions)
                .WithOne(t => t.GivenAnswer)
                .OnDelete(DeleteBehavior.Cascade);

            // don't delete given answers if option is deleted
            modelBuilder.Entity<QuestionOption>()
                .HasMany(t => t.GivenAnswers)
                .WithOne(t => t.QuestionOption)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
