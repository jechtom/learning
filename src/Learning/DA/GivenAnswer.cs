using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class GivenAnswer
    {
        public int Id { get; set; }

        [Required]
        public AccessToken Owner { get; set; }

        [Required]
        public Question Question { get; set; }

        public ICollection<GivenAnswerToQuestionOption> SelectedQuestionOptions { get; set; }
    }
}