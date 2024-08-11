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
            // The GameId and GenreId are automatically picked up as the foreign
            // keys and are configured as the composite primary key for the join
            // entity type.
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.Games)
                .UsingEntity<GameGenre>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
    }
}
