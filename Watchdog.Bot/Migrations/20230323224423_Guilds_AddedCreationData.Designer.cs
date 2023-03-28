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
    [Migration("20230323224423_Guilds_AddedCreationData")]
    partial class Guilds_AddedCreationData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Watchdog.Bot.Models.Guild", b =>
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

            modelBuilder.Entity("Watchdog.Bot.Models.GuildParameter", b =>
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

            modelBuilder.Entity("Watchdog.Bot.Models.Parameter", b =>
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

            modelBuilder.Entity("Watchdog.Bot.Models.Permission", b =>
                {
                    b.Property<string>("RestrictedAction")
                        .HasColumnType("text");

                    b.Property<long>("RequiredPermission")
                        .HasColumnType("bigint");

                    b.HasKey("RestrictedAction");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.PermissionOverride", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("RestrictedAction")
                        .HasColumnType("text");

                    b.Property<long>("Override")
                        .HasColumnType("bigint");

                    b.HasKey("GuildId", "RestrictedAction");

                    b.HasIndex("RestrictedAction");

                    b.ToTable("PermissionOverrides");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.GuildParameter", b =>
                {
                    b.HasOne("Watchdog.Bot.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Watchdog.Bot.Models.Parameter", "Parameter")
                        .WithMany()
                        .HasForeignKey("Name")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Parameter");
                });

            modelBuilder.Entity("Watchdog.Bot.Models.PermissionOverride", b =>
                {
                    b.HasOne("Watchdog.Bot.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Watchdog.Bot.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("RestrictedAction")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Permission");
                });
#pragma warning restore 612, 618
        }
    }
}