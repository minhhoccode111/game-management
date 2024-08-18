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
            // set 2 foreign keys GameId and GenreId to be primary key in GameGenre
            modelBuilder.Entity<GameGenre>().HasKey(gg => new { gg.GenreId, gg.GameId });

            // many-to-many: Game-to-Genre (demantory)
            modelBuilder
                .Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId)
                // auto remove all GameGenres if delete a Game
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // many-to-many: Genre-to-Game (optional)
            modelBuilder
                .Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId)
                // can't delete without removing all GameGenres first BUG:
                .OnDelete(DeleteBehavior.Restrict);

            // optional: make delete Genre cascade (auto delete all children)

            // many-to-many: Game-to-Company (demantory)
            modelBuilder
                .Entity<GameCompany>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.GameCompanies)
                .HasForeignKey(gc => gc.GameId)
                // auto remove all GameCompanies if delete a Game
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // many-to-many: Company-to-Game (optional)
            modelBuilder
                .Entity<GameCompany>()
                .HasOne(gc => gc.Company)
                .WithMany(c => c.GameCompanies)
                .HasForeignKey(gc => gc.CompanyId)
                // can't delete without removing all GameCompanies first BUG:
                .OnDelete(DeleteBehavior.Restrict);

            // optional: make delete Company cascade (auto delete all children)
        }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
        public DbSet<GameCompany> GameCompany { get; set; } = default!;
    }
}
