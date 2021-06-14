using HerosCompanyApi.Models;
using HerosCompanyApi.Models.DataModels;
using HerosCompanyApi.Models.DTOs;
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

        public HerosController(HerosCompanyDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeroDTO>>> GetHeros()
        {
            var herosList = await  _dbContext.Heroes.Select(h => ToHeroDTO(h)).ToListAsync();
            if (!herosList.Any())
                return NotFound();

            return Ok(herosList);
        }

        // GET api/<HerosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HerosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HerosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HerosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private static HeroDTO ToHeroDTO(Hero hero)
        {
            return new HeroDTO(hero.HeroId,
                               hero.Name,
                               hero.Ability,
                               hero.StartedAt,
                               hero.SuitColors,
                               hero.StartingPower,
                               hero.CurrentPower,
                               hero.LastTimeTrained,
                               hero.Trainers);
        }
    }
}
