﻿// <auto-generated />
using System;
using HerosCompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HerosCompanyApi.Migrations
{
    [DbContext(typeof(HerosCompanyDBContext))]
    [Migration("20210613200052_RenameHeroField")]
    partial class RenameHeroField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HerosCompanyApi.Models.DataModels.Hero", b =>
                {
                    b.Property<Guid>("HeroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ability")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CurrentPower")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartedAt")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<double>("StartingPower")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<string>("SuitColors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HeroId");

                    b.ToTable("Heros");
                });

            modelBuilder.Entity("HerosCompanyApi.Models.DataModels.TrainerHero", b =>
                {
                    b.Property<Guid>("TrainerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HeroId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TrainerId", "HeroId");

                    b.HasIndex("HeroId");

                    b.ToTable("TrainerHero");
                });

            modelBuilder.Entity("HerosCompanyApi.Models.Trainer", b =>
                {
                    b.Property<Guid>("TrainerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("TrainerUserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrainerId");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("HerosCompanyApi.Models.DataModels.TrainerHero", b =>
                {
                    b.HasOne("HerosCompanyApi.Models.DataModels.Hero", "Hero")
                        .WithMany("TrainerHeroes")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HerosCompanyApi.Models.Trainer", "Trainer")
                        .WithMany("TrainerHeroes")
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hero");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("HerosCompanyApi.Models.DataModels.Hero", b =>
                {
                    b.Navigation("TrainerHeroes");
                });

            modelBuilder.Entity("HerosCompanyApi.Models.Trainer", b =>
                {
                    b.Navigation("TrainerHeroes");
                });
#pragma warning restore 612, 618
        }
    }
}