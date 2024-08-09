// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using GameManagementMvc.Models;
using GameManagementMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Data
{
    public class GameManagementMvcContext : DbContext
    {
        // constructor
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Company> Company { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
    }
}
