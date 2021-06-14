using HerosCompanyApi.Models;
using HerosCompanyApi.Models.DataModels;
using HerosCompanyApi.Models.DTOs;
using HerosCompanyApi.Models.Requests;
using HerosCompanyApi.Services.TokenGenerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HerosCompanyApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HerosController : ControllerBase
    {
        private readonly HerosCompanyDBContext _dbContext;
        private readonly AccessTokenGenerator _tokenGenerator;

        public HerosController(HerosCompanyDBContext dBContext, AccessTokenGenerator tokenGenerator)
        {
            _dbContext = dBContext;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeroDTO>>> GetHeros()
        {
            TrainerDTO askingTrainer = _tokenGenerator.ReadToken(HttpContext.User);

            var herosList = await _dbContext.Heroes
                .Include(h => h.Trainers)
                .ToListAsync();

            if (!herosList.Any())
                return NotFound();

            var herosDTOList = herosList.Select(h => ToHeroDTO(h, h.Trainers.Select(t => TrainerToDTO(t)).Contains(askingTrainer)));

            return Ok(herosDTOList);
        }

        [HttpPost]
        public async Task<ActionResult<Hero>> CreateHero([FromBody] CreateHeroRequest createRequest)
        {

            List<Trainer> trainers = _dbContext.Trainers
               .Include(t => t.Heros)
               .Where(t => createRequest.TrainerNames.Contains(t.TrainerUserName))
               .ToList();

            Hero heroToAdd = new Hero
            {
                Name = createRequest.Name,
                Ability = createRequest.Ability,
                StartedAt = DateTime.Now,
                SuitColors = createRequest.SuitColors,
                StartingPower = createRequest.StartingPower,
                CurrentPower = createRequest.StartingPower,
                Trainers = trainers,
            };

            _dbContext.Heroes.Add(heroToAdd);

            foreach (var trainer in trainers)
            {
                trainer.Heros.Add(heroToAdd);
            }

         

            await _dbContext.SaveChangesAsync();

            return Created(nameof(GetHeros), ToHeroDTO(heroToAdd));
        }

        [HttpPatch("train/{id}")]
        public async Task<ActionResult<HeroDTO>> TrainHero(Guid id)
        {
            Hero heroToTrain = _dbContext.Heroes.Include(h => h.Trainers).Where(h => h.HeroId == id).FirstOrDefault();
            var test = _dbContext.Heroes.FirstOrDefault(s => s.HeroId == id);

            TrainerDTO askingTrainer = _tokenGenerator.ReadToken(HttpContext.User);

            //if hero doesnt exist
            if (heroToTrain is null)
            {
                return BadRequest();
            }

            var isTrainableByAskingTrainer = heroToTrain.Trainers.Where(t => t.TrainerId == askingTrainer.TrainerId)
                                                                 .FirstOrDefault();
            //if trainer is not allowed to train the hero
            if (isTrainableByAskingTrainer is null)
            {
                return Forbid();
            }


            //if hero just started
            if (heroToTrain.LastTimeTrained is null)
            {
                heroToTrain.LastTimeTrained = DateTime.Now;
                heroToTrain.LastTimeTrainingAmount += 1;
                heroToTrain.CurrentPower += HeroPowerGained();
                await _dbContext.SaveChangesAsync();
                return Ok(ToHeroDTO(heroToTrain,true));
            }

            if (heroToTrain.LastTimeTrainingAmount >= 5)
            {
                //check if a day passed since lastTimeTrained
                if ((DateTime.Now - heroToTrain.LastTimeTrained).Value.TotalDays > 1)
                {
                    heroToTrain.LastTimeTrained = DateTime.Now;
                    heroToTrain.LastTimeTrainingAmount = 1;
                    heroToTrain.CurrentPower += HeroPowerGained();
                    await _dbContext.SaveChangesAsync();
                    return Ok(ToHeroDTO(heroToTrain,true));
                }
                else
                {
                    return BadRequest();
                }
            }

            heroToTrain.LastTimeTrainingAmount += 1;
            heroToTrain.CurrentPower += HeroPowerGained();
            await _dbContext.SaveChangesAsync();
            return Ok(ToHeroDTO(heroToTrain,true));
        }

       

        private static HeroDTO ToHeroDTO(Hero hero, bool trainable = false)
        {
            return new HeroDTO(hero.HeroId,
                               hero.Name,
                               hero.Ability,
                               hero.StartedAt,
                               hero.SuitColors,
                               hero.StartingPower,
                               hero.CurrentPower,
                               hero.LastTimeTrained,
                               trainable);
        }

        private static double HeroPowerGained()
        {
            Random rng = new Random();
            return rng.NextDouble() / 10;
        }

        private TrainerDTO TrainerToDTO(Trainer trainer)
        {

            return new TrainerDTO(trainer.TrainerId, trainer.TrainerUserName);
        }
    }
}
