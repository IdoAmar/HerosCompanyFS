using HerosCompanyApi.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models
{
    public class HerosCompanyDBContext : DbContext
    {
        public HerosCompanyDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Hero> Heroes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //applying the models configurations to the database
            builder.ApplyConfigurationsFromAssembly(typeof(Trainer).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(Hero).Assembly);
        }
    }
}
