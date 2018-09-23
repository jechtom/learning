using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class GivenAnswerToQuestionOption
    {
        [Required]
        public GivenAnswer GivenAnswer { get; set; }

        public int GivenAnswerId { get; set; }

        [Required]
        public QuestionOption QuestionOption { get; set; }

        public int QuestionOptionId { get; set; }
    }
}