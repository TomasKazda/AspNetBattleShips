﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BattleShips.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BattleShips.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //if (!Database.GetService<IRelationalDatabaseCreator>().Exists())
            //    Database.Migrate(); 
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GamePiece> GamePieces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>().Property(p => p.GameCreatedAt).HasDefaultValue<DateTime>(DateTime.UtcNow);
        }
    }
}
