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
        public GameManagementMvcContext(DbContextOptions<GameManagementMvcContext> options)
            : base(options) { }

        public DbSet<GameManagementMvc.Models.Game> Game { get; set; } = default!;
        public DbSet<GameManagementMvc.Models.Company> Company { get; set; } = default!;
        public DbSet<GameManagementMvc.Models.Genre> Genre { get; set; } = default!;
    }
}
