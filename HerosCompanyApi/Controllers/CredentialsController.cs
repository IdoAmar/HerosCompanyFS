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


namespace HerosCompanyApi.Controllers
{
    [Authorize]
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
                BadRequest();
            }

            Trainer existingTrainer =  _dbContext.Trainers.Where(t => t.TrainerUserName == registerRequest.TrainerUserName)
                                                          .FirstOrDefault();

            if (existingTrainer is not null)
            {
                return Conflict();
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

            string token = _accessTokenGenerator.GenerateToken(newTrainer);
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(TrainerToDTO(newTrainer));
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

            string token = _accessTokenGenerator.GenerateToken(existingTrainer);
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(TrainerToDTO(existingTrainer));
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        private TrainerDTO TrainerToDTO(Trainer trainer)
        {
            return new TrainerDTO(trainer.TrainerId, trainer.TrainerUserName);
        }
    }
}
