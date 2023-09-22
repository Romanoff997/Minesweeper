
using Microsoft.EntityFrameworkCore;
using Minesweeper.Server.Domain.Entities;
using Minesweeper.Server.Models;
using System.Collections.Generic;

namespace Minesweeper.Server.Domain.Repositories
{
    public class MyDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<FieldEntity> Fields { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MinesweeperDB");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldEntity>()
                .HasOne<GameEntity>(d => d.GameEntity)
                .WithMany(p => p.FieldEntity)
                .HasForeignKey(d => d.GameEntityId);

            base.OnModelCreating(modelBuilder);


        }
    }
}
