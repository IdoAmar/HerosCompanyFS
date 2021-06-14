using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models.DTOs
{
    public record HeroDTO(
        Guid HeroId,
        string Name,
        string Ability,
        DateTime? StartedAt,
        string SuitColors,
        double StartingPower,
        double CurrentPower,
        DateTime? LastTimeTrained,
        ICollection<Trainer> Trainers);
}
