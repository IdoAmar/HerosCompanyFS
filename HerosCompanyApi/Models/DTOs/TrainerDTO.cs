using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models.DTOs
{
    public record TrainerDTO(
        Guid TrainerId,
        string TrainerUserName);
}
