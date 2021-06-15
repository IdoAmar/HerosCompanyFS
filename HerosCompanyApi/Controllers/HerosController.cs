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
using ServiceStack.Host;


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

        //controller to handle get all heros request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeroDTO>>> GetHeros()
        {
            TrainerDTO askingTrainer = _tokenGenerator.ReadToken(HttpContext.User);

            var herosList = await _dbContext.Heroes
                .Include(h => h.Trainers)
                .ToListAsync();

            if (!herosList.Any())
            {
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, "There are no heros available");
            }
            //attaching is trainable property to the heros
            var herosDTOList = herosList.Select(h => ToHeroDTO(h, IsHeroTrainable(h, askingTrainer)));

            return Ok(herosDTOList);
        }

        //controller to handle create hero
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

        // controller to handle train hero by id
        [HttpPatch("train/{id}")]
        public async Task<ActionResult<HeroDTO>> TrainHero(Guid id)
        {
            Hero heroToTrain = _dbContext.Heroes.Include(h => h.Trainers).Where(h => h.HeroId == id).FirstOrDefault();

            TrainerDTO askingTrainer = _tokenGenerator.ReadToken(HttpContext.User);

            //if hero doesnt exist
            if (heroToTrain is null)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.BadRequest, "This hero does not exist");
            }

            var isTrainableByAskingTrainer = heroToTrain.Trainers.Where(t => t.TrainerId == askingTrainer.TrainerId)
                                                                 .FirstOrDefault();
            //if trainer is not allowed to train the hero
            if (isTrainableByAskingTrainer is null)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "This trainer is not allowed to train this hero");
            }


            //if hero just started
            if (heroToTrain.LastTimeTrained is null)
            {
                heroToTrain.LastTimeTrained = DateTime.Now;
                heroToTrain.LastTimeTrainingAmount += 1;
                heroToTrain.CurrentPower += HeroPowerGained();
                await _dbContext.SaveChangesAsync();
                return Ok(ToHeroDTO(heroToTrain, true));
            }

            if (heroToTrain.LastTimeTrainingAmount >= 5)
            {
                //check if a day passed since lastTimeTrained to reset the training amount
                if ((DateTime.Now - heroToTrain.LastTimeTrained).Value.TotalDays > 1)
                {
                    heroToTrain.LastTimeTrained = DateTime.Now;
                    heroToTrain.LastTimeTrainingAmount = 1;
                    heroToTrain.CurrentPower += HeroPowerGained();
                    await _dbContext.SaveChangesAsync();
                    return Ok(ToHeroDTO(heroToTrain, true));
                }
                else
                {
                    throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Hero is exhausted, try again tommrow");

                }
            }
            //if hero is trainable
            heroToTrain.LastTimeTrainingAmount += 1;
            heroToTrain.CurrentPower += HeroPowerGained();
            await _dbContext.SaveChangesAsync();

            var heroDTO = ToHeroDTO(heroToTrain, heroToTrain.LastTimeTrainingAmount == 5 ? false : true);

            return Ok(heroDTO);
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
                               trainable);
        }

        private static double HeroPowerGained()
        {
            Random rng = new Random();
            return rng.NextDouble() / 10;
        }

        private static TrainerDTO TrainerToDTO(Trainer trainer)
        {

            return new TrainerDTO(trainer.TrainerId, trainer.TrainerUserName);
        }

        // method to check if hero is trainable by a trainer
        private static bool IsHeroTrainable(Hero hero, TrainerDTO askingTrainer)
        {
            if (!hero.Trainers.Select(t => TrainerToDTO(t)).Contains(askingTrainer))
            {
                return false;
            }
            if (hero.LastTimeTrainingAmount >= 5)
            {
                if ((DateTime.Now - hero.LastTimeTrained).Value.TotalDays > 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
