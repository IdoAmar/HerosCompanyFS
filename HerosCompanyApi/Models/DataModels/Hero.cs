using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models.DataModels
{
    public class Hero
    {
        public Guid HeroId { get; set; }
        public string Name { get; set; }
        public string Ability { get; set; }
        public DateTime? StartedAt { get; set; }
        public string SuitColors { get; set; }
        public double StartingPower { get; set; }
        public double CurrentPower { get; set; }
        public DateTime? LastTimeTrained { get; set; }
        public int LastTimeTrainingAmount { get; set; }

        public ICollection<Trainer> Trainers { get; set; }
        public List<TrainerHero> TrainerHeroes { get; set; }

    }
}
