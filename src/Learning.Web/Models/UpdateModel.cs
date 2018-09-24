using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Web.Models
{
    public class UpdateModel
    {
        public QuestionModel[] Questions { get; set; }

        public class QuestionModel
        {
            public string Name { get; set; }
            public string[] Type { get; set; }
            public string[] Question { get; set; }
            public string[] Ok { get; set; }
            public string[] Bad { get; set; }
        }
    }
}
