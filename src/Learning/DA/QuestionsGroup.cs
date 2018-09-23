using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class QuestionsGroup
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}