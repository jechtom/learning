using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning.Web.Models
{
    public class SubmitModel
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public Answer[] Answers { get; set; }

        public class Answer
        {
            public int Id { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
