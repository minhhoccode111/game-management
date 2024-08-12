using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        // constructor
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config explicitly

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
    }
}
