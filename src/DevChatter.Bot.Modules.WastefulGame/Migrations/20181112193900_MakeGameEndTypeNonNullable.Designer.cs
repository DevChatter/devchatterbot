﻿// <auto-generated />
using System;
using DevChatter.Bot.Modules.WastefulGame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    [DbContext(typeof(GameDataContext))]
    [Migration("20181112193900_MakeGameEndTypeNonNullable")]
    partial class MakeGameEndTypeNonNullable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.GameEndRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("EndType")
                        .IsRequired();

                    b.Property<int>("LevelNumber");

                    b.Property<int>("Points");

                    b.Property<int?>("SurvivorId");

                    b.HasKey("Id");

                    b.HasIndex("SurvivorId");

                    b.ToTable("GameEndRecords");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("SurvivorId");

                    b.Property<int>("Uses");

                    b.HasKey("Id");

                    b.HasIndex("SurvivorId");

                    b.ToTable("InventoryItem");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.Survivor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<long>("Money");

                    b.Property<int?>("TeamId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Survivors");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.GameEndRecord", b =>
                {
                    b.HasOne("DevChatter.Bot.Modules.WastefulGame.Model.Survivor", "Survivor")
                        .WithMany("GameEndRecords")
                        .HasForeignKey("SurvivorId");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.InventoryItem", b =>
                {
                    b.HasOne("DevChatter.Bot.Modules.WastefulGame.Model.Survivor", "Survivor")
                        .WithMany("InventoryItems")
                        .HasForeignKey("SurvivorId");
                });

            modelBuilder.Entity("DevChatter.Bot.Modules.WastefulGame.Model.Survivor", b =>
                {
                    b.HasOne("DevChatter.Bot.Modules.WastefulGame.Model.Team", "Team")
                        .WithMany("Members")
                        .HasForeignKey("TeamId");
                });
#pragma warning restore 612, 618
        }
    }
}