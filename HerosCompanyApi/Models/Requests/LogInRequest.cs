using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models
{
    public class LogInRequest
    {
        [Required]
        public string TrainerUserName { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.*\d)(?=.*[A-Z])(?=.*\W).*$")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
