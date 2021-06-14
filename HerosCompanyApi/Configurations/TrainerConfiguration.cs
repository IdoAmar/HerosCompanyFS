using HerosCompanyApi.Models;
using HerosCompanyApi.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {

            builder.HasKey(t => t.TrainerId);

            builder.Property(t => t.TrainerUserName)
                .IsRequired();

            builder.Property(t => t.Password)
                .IsRequired();

            builder.Property(t => t.Salt)
                .IsRequired();

            builder.HasMany(t => t.Heros)
                .WithMany(h => h.Trainers)
                .UsingEntity<TrainerHero>(

                    th => th.HasOne(th => th.Hero)
                            .WithMany(t => t.TrainerHeroes)
                            .HasForeignKey(th => th.HeroId),

                    th => th.HasOne(th => th.Trainer)
                            .WithMany(t => t.TrainerHeroes)
                            .HasForeignKey(th => th.TrainerId),

                    th => th.HasKey(th => new { th.TrainerId, th.HeroId })
                );
        }
    }
}
