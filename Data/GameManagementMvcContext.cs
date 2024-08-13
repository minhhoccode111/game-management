using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        // configured explicitly

        // // one company to many games relationship
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder
        //         .Entity<Company>()
        //         .HasMany(e => e.Games)
        //         .WithOne(e => e.Company)
        //         .HasForeignKey(e => e.CompanyId)
        //         .IsRequired();
        // }

        // // many games to many genres relationship
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder
        //         .Entity<Game>()
        //         .HasMany(e => e.Genres)
        //         .WithMany(e => e.Games)
        //         .UsingEntity<GameGenre>();
        // }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
    }
}
