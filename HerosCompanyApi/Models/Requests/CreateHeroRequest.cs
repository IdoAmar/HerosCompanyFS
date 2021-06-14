using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models.Requests
{
    public class CreateHeroRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Ability { get; set; }

        [Required]
        public string SuitColors { get; set; }

        [Required]
        public double StartingPower { get; set; }

        [Required]
        public ICollection<string> TrainerNames { get; set; }
    }
}
