using HerosCompanyApi.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models
{
    public class Trainer
    {
        public Guid TrainerId { get; set; }
        public string TrainerUserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }

        public ICollection<Hero> Heros { get; set; }
        public List<TrainerHero> TrainerHeroes { get; set; }
    }
}
