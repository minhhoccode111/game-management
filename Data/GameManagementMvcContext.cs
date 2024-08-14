using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        // configured explicitly
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set 2 foreign keys to be primary key
            modelBuilder.Entity<GameGenre>().HasKey(gg => new { gg.GenreId, gg.GameId });

            // Game must have at least one Genre
            modelBuilder
                .Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId)
                .IsRequired();

            // Genre exist independent
            modelBuilder
                .Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId);

            // optional: make delete Genre cascade (auto delete all child)

            // Game must have at least one Company
            modelBuilder
                .Entity<GameCompany>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.GameCompanies)
                .HasForeignKey(gc => gc.GameId)
                .IsRequired();

            // Company exist independent
            modelBuilder
                .Entity<GameCompany>()
                .HasOne(gc => gc.Company)
                .WithMany(c => c.GameCompanies)
                .HasForeignKey(gc => gc.CompanyId);

            // optional: make delete Company cascade (auto delete all child)
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
        public DbSet<GameCompany> GameCompany { get; set; } = default!;
    }
}
