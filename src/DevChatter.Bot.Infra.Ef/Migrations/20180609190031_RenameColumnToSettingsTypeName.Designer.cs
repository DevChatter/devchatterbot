﻿// <auto-generated />
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20180609190031_RenameColumnToSettingsTypeName")]
    partial class RenameColumnToSettingsTypeName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DevChatter.Bot.Core.Commands.SimpleCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommandText");

                    b.Property<string>("HelpText");

                    b.Property<int>("RoleRequired");

                    b.Property<string>("StaticResponse");

                    b.HasKey("Id");

                    b.ToTable("SimpleCommands");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.ChatUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<int?>("Role");

                    b.Property<int>("Tokens");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ChatUsers");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.CommandSettingsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("SettingsTypeName");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("CommandSettings");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.CommandUsageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatClientUsed");

                    b.Property<string>("CommandWord");

                    b.Property<DateTimeOffset>("DateTimeUsed");

                    b.Property<string>("FullTypeName");

                    b.Property<string>("UserDisplayName");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("CommandUsages");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.CommandWordEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommandWord");

                    b.Property<string>("FullTypeName");

                    b.Property<bool>("IsPrimary");

                    b.HasKey("Id");

                    b.HasIndex("CommandWord");

                    b.ToTable("CommandWords");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.HangmanWord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.ToTable("HangmanWords");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.IntervalMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DelayInMinutes");

                    b.Property<string>("MessageText");

                    b.HasKey("Id");

                    b.ToTable("IntervalMessages");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.QuizQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CorrectAnswer");

                    b.Property<string>("Hint1");

                    b.Property<string>("Hint2");

                    b.Property<string>("MainQuestion");

                    b.Property<string>("WrongAnswer1");

                    b.Property<string>("WrongAnswer2");

                    b.Property<string>("WrongAnswer3");

                    b.HasKey("Id");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.QuoteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<string>("Author");

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("QuoteId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("QuoteId");

                    b.ToTable("QuoteEntities");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.ScheduleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("ExampleDateTime");

                    b.HasKey("Id");

                    b.ToTable("ScheduleEntities");
                });

            modelBuilder.Entity("DevChatter.Bot.Core.Data.Model.StreamerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChannelName");

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("TimesShoutedOut");

                    b.HasKey("Id");

                    b.ToTable("Streamers");
                });
#pragma warning restore 612, 618
        }
    }
}
