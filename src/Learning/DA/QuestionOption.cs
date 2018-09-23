using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class QuestionOption
    {
        public int Id { get; set; }
        
        [Required]
        public Question Question { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }

        public ICollection<GivenAnswer> GivenAnswers { get; set; }
    }
}