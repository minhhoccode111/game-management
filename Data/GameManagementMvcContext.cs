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
            // entity type
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.Games)
                .UsingEntity<GameGenre>();

            // // WARN: don't understand those
            // // company to game: 1 mandatory to many optional
            // modelBuilder.Entity<Game>()
            //     .HasOne(g=>g.Company)
            //     .WithMany(c=>c.Games)
            //     .HasForeignKey(g=>g.CompanyId)
            //     .IsRequired();
            // // game to genre: many mandatory to many optional
            // modelBuilder
            //     .Entity<GameGenre>()
            //     .HasKey(gg => new { gg.GameId, gg.GenreId });
            // modelBuilder
            //     .Entity<GameGenre>()
            //     .HasOne(gg => gg.Game)
            //     .WithMany(g => g.GameGenres)
            //     .HasForeignKey(gg => gg.GameId);
            // modelBuilder
            //     .Entity<GameGenre>()
            //     .HasOne(gg => gg.Genre)
            //     .WithMany(g => g.GameGenres)
            //     .HasForeignKey(gg => gg.GenreId);
            // // ensure that each game has at least one genre
            // modelBuilder
            //     .Entity<Game>()
            //     .HasMany(g => g.GameGenres)
            //     .WithOne(gg => gg.Game)
            //     .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
    }
}
