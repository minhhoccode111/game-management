using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config explicitly
            // GameGenre pick GameId and GenreId to be primary key
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.Games)
                .UsingEntity<GameGenre>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
    }
}
