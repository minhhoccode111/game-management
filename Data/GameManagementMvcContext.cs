// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        /*
           In the seed database, we must initialize the Id field in Genre models
           so that when we create Game models, we have the Ids to pass to the
           field GenreIds in Game models
           this method allow the Id field to automatically initialize when we
           call `new` instead of manually assign an Id field to it
           Not working as expect?
        */
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Genre>().Property(g => g.Id).ValueGeneratedOnAdd();
        //     base.OnModelCreating(modelBuilder);
        // }

        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        public DbSet<GameManagementMvc.Models.Game> Game { get; set; } = default!;
        public DbSet<GameManagementMvc.Models.Company> Company { get; set; } = default!;
        public DbSet<GameManagementMvc.Models.Genre> Genre { get; set; } = default!;
    }
}
