﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200612224306_Hospitalization")]
    partial class Hospitalization
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("API.Data.Models.Hospitalization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Abilene")
                        .HasColumnType("int");

                    b.Property<int>("Amarillo")
                        .HasColumnType("int");

                    b.Property<int>("Austin")
                        .HasColumnType("int");

                    b.Property<int>("BeltonKilleen")
                        .HasColumnType("int");

                    b.Property<int>("BryanCollegStation")
                        .HasColumnType("int");

                    b.Property<int>("CorpusChristi")
                        .HasColumnType("int");

                    b.Property<int>("Dallas")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ElPaso")
                        .HasColumnType("int");

                    b.Property<int>("Galveston")
                        .HasColumnType("int");

                    b.Property<int>("Houston")
                        .HasColumnType("int");

                    b.Property<int>("Laredo")
                        .HasColumnType("int");

                    b.Property<int>("Lubbock")
                        .HasColumnType("int");

                    b.Property<int>("Lufkin")
                        .HasColumnType("int");

                    b.Property<int>("MidlanOdessa")
                        .HasColumnType("int");

                    b.Property<int>("Paris")
                        .HasColumnType("int");

                    b.Property<int>("RioGrandeValley")
                        .HasColumnType("int");

                    b.Property<int>("SanAngelo")
                        .HasColumnType("int");

                    b.Property<int>("SanAntonio")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.Property<int>("Tyler")
                        .HasColumnType("int");

                    b.Property<int>("Victoria")
                        .HasColumnType("int");

                    b.Property<int>("Waco")
                        .HasColumnType("int");

                    b.Property<int>("Wichita")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hospitalization");
                });

            modelBuilder.Entity("API.Data.Models.PresPoll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CandidateName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CandidateParty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FteGrade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Methodology")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Partisan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pct")
                        .HasColumnType("int");

                    b.Property<int>("PollId")
                        .HasColumnType("int");

                    b.Property<long>("PollsterId")
                        .HasColumnType("bigint");

                    b.Property<long>("PollsterRatingId")
                        .HasColumnType("bigint");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("RaceId")
                        .HasColumnType("int");

                    b.Property<int>("SampleSize")
                        .HasColumnType("int");

                    b.Property<long>("SponsorId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PresPolls");
                });
#pragma warning restore 612, 618
        }
    }
}
