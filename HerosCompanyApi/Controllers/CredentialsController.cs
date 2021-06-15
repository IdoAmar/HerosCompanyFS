using HerosCompanyApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HerosCompanyApi.Services.TokenGenerators;
using HerosCompanyApi.Services.Encrypters;
using HerosCompanyApi.Models.DTOs;
using Microsoft.AspNetCore.Cors;

namespace HerosCompanyApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsController : ControllerBase
    {
        private readonly HerosCompanyDBContext _dbContext;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly IPasswordHasherService _hasherService;

        public CredentialsController( AccessTokenGenerator accessTokenGenerator, HerosCompanyDBContext dBContext, IPasswordHasherService hasherService)
        {
            _dbContext = dBContext;
            _accessTokenGenerator = accessTokenGenerator;
            _hasherService = hasherService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest("Please enter username and password with one number capital letter and special character");
            }

            Trainer existingTrainer =  _dbContext.Trainers.Where(t => t.TrainerUserName == registerRequest.TrainerUserName)
                                                          .FirstOrDefault();

            if (existingTrainer is not null)
            {
                return Conflict("User name already exists");
            }

            var PasswordAndSalt = _hasherService.HashPassword(registerRequest.Password);

            Trainer newTrainer = new Trainer()
            {
                TrainerUserName = registerRequest.TrainerUserName,
                Password = PasswordAndSalt.hashedPassword,
                Salt = PasswordAndSalt.salt
            };

            var entry =_dbContext.Trainers.Add(newTrainer);
            await _dbContext.SaveChangesAsync();

            var trainerDTO = TrainerToDTO(newTrainer);

            string token = _accessTokenGenerator.GenerateToken(trainerDTO);
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(trainerDTO);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInRequest logInRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingTrainer = _dbContext.Trainers.Where(t => t.TrainerUserName == logInRequest.TrainerUserName).FirstOrDefault();

            if (existingTrainer is null)
            {
                return Unauthorized();
            }


            if (_hasherService.VerifyPassword(logInRequest.Password, existingTrainer.Salt, existingTrainer.Password))
            {
                return Unauthorized();
            }

            var trainerDTO = TrainerToDTO(existingTrainer);

            string token = _accessTokenGenerator.GenerateToken(trainerDTO);
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(trainerDTO);
        }

        private TrainerDTO TrainerToDTO(Trainer trainer)
        {
          
            return new TrainerDTO(trainer.TrainerId, trainer.TrainerUserName);
        }
    }
}
