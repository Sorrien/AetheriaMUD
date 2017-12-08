using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AetheriaWebService.Models
{
    public class AetheriaContext : DbContext
    {
        public AetheriaContext(DbContextOptionsBuilder optionsBuilder)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=AetheriaContext;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<World> Worlds { get; set; }
        public virtual DbSet<Cell> Cells { get; set; }

        public virtual DbSet<Entity> Entities { get; set; }   
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<NonPlayerCharacter> NonPlayerCharacters { get; set; }
        public virtual DbSet<CharacterStats> CharacterStats { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<ChatUser> ChatUsers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<Effect> Effects { get; set; }
    }
}

