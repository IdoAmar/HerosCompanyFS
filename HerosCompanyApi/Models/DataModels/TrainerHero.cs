using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models.DataModels
{
    public class TrainerHero
    {
        public Guid TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public Guid HeroId { get; set; }
        public Hero Hero { get; set; }
    }
}
