using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models
{
    public class RegisterRequest
    {
        [Required]
        public string TrainerUserName { get; set; }
        
        [Required]
        [RegularExpression(@"^.*(?=.*\d)(?=.*[A-Z])(?=.*\W).*$")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
