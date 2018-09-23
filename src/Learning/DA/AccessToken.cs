using System;
using System.ComponentModel.DataAnnotations;

namespace Learning.DA
{
    public class AccessToken
    {
        public int Id { get; set; }

        [Required]
        public string Token{ get; set; }
        
        public DateTime? ActivationDate { get; set; }

        public string Name { get; set; }
    }
}