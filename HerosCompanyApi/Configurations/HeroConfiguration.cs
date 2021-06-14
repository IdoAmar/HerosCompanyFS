using HerosCompanyApi.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Configurations
{
    public class HeroConfiguration : IEntityTypeConfiguration<Hero>
    {
        public void Configure(EntityTypeBuilder<Hero> builder)
        {
            builder.ToTable("Heros");

            builder.HasKey(h => h.HeroId);

            builder.Property(h => h.Name)
                .IsRequired();

            builder.Property(h => h.Ability)
                .IsRequired();

            builder.Property(h => h.StartedAt)
                .IsRequired();

            builder.Property(h => h.SuitColors)
                .IsRequired();

            builder.Property(h => h.StartingPower)
                .HasDefaultValue(0);

            builder.Property(h => h.CurrentPower)
                .HasDefaultValue(0);

            builder.Property(h => h.LastTimeTrained);

            builder.Property(h => h.LastTimeTrainingAmount)
                .IsRequired();

            builder.HasMany(h => h.Trainers)
                .WithMany(t => t.Heros)
                .UsingEntity<TrainerHero>(
                th => th.HasOne(th => th.Trainer)
                        .WithMany(t => t.TrainerHeroes)
                        .HasForeignKey(th => th.TrainerId),

                th => th.HasOne(th => th.Hero)
                        .WithMany(h => h.TrainerHeroes)
                        .HasForeignKey(th => th.HeroId),

                th => th.HasKey(th => new { th.TrainerId, th.HeroId })
                );
        }
    }
}
