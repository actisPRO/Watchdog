﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Watchdog.Bot;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230404204058_UsageStatistics_ValueTypeLong")]
    partial class UsageStatistics_ValueTypeLong
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Watchdog.Bot.Models.Database.Guild", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DatabaseEntryCreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("current_timestamp");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.GuildParameter", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name", "GuildId");

                    b.HasIndex("GuildId");

                    b.ToTable("GuildParameters");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.ModerationLogEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ExecutorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TargetId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("ValidUntil")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("ModerationLog");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.Parameter", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.UsageStatistic", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Key", "GuildId", "Date");

                    b.HasIndex("GuildId");

                    b.ToTable("UsageStatistics");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.GuildParameter", b =>
                {
                    b.HasOne("Watchdog.Bot.Models.Database.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Watchdog.Bot.Models.Database.Parameter", "Parameter")
                        .WithMany()
                        .HasForeignKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Parameter");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.ModerationLogEntry", b =>
                {
                    b.HasOne("Watchdog.Bot.Models.Database.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.Database.UsageStatistic", b =>
                {
                    b.HasOne("Watchdog.Bot.Models.Database.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });
#pragma warning restore 612, 618
        }
    }
}
