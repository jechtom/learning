using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string ExternalId { get; set; }

        public QuestionsGroup QuestionsGroup { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        public string Content { get; set; }

        public ICollection<QuestionOption> Options { get; set; }

        public ICollection<GivenAnswer> GivenAnswers { get; set; }
    }
}