// using System;
// using System.Linq;
// using Microsoft.Extensions.DependencyInjection;
using GameManagementMvc.Data;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Models
{
    public static class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (
                var context = new GameManagementMvcContext(
                    serviceProvider.GetRequiredService<DbContextOptions<GameManagementMvcContext>>()
                )
            )
            {
                // if any movie exists
                if (context.Game.Any() || context.Company.Any() || context.Genre.Any())
                {
                    // turn on to clear database

                    // context.Game.RemoveRange(context.Game);
                    // context.Company.RemoveRange(context.Company);
                    // context.Genre.RemoveRange(context.Genre);
                    // context.SaveChanges();
                    // Console.WriteLine("Database clear!");

                    return;
                }

                var ran = new Random();

                // if no movie exists
                var companies = new Company[]
                {
                    // MAANG
                    new Company
                    {
                        // Id = 0,
                        Title = "Meta Corp",
                        Body = "Meta Corp is a very greate company",
                        Image = "https://images.cnbctv18.com/wp-content/uploads/2022/09/Meta.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    },
                    new Company
                    {
                        // Id = 1,
                        Title = "Apple Corp",
                        Body = "Apple Corp is a very great company",
                        Image =
                            "https://dm0qx8t0i9gc9.cloudfront.net/thumbnails/video/UD7CEz6/editorial-apple-inc-logo-on-glass-building_smk22zqcg_thumbnail-1080_01.png",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    },
                    new Company
                    {
                        // Id = 2,
                        Title = "Amazon Corp",
                        Body = "Amazon Corp is a very greate company",
                        Image =
                            "https://www.wealthandfinance-news.com/wp-content/uploads/2020/01/amazon.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    },
                    new Company
                    {
                        // Id = 3,
                        Title = "Netflix Corp",
                        Body = "Netflix Corp is a very greate company",
                        Image =
                            "https://s.aolcdn.com/hss/storage/midas/dae3c205f61d252afbea973ef0409803/206200911/Netflix+Media_0193+2.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    },
                    new Company
                    {
                        // Id = 4,
                        Title = "Google Corp",
                        Body = "Google Corp is a very greate company",
                        Image = "https://wallpapercave.com/wp/kmmXJbb.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    }
                };

                context.Company.AddRange(companies);

                var genres = new Genre[]
                {
                    new Genre
                    {
                        Id = 6,
                        Title = "Action",
                        Body = "Action is a very greate genre"
                    },
                    new Genre
                    {
                        Id = 7,
                        Title = "Sport",
                        Body = "Sport is a very greate genre"
                    },
                    new Genre
                    {
                        Id = 8,
                        Title = "Horror",
                        Body = "Horror is a very greate genre"
                    },
                    new Genre
                    {
                        Id = 9,
                        Title = "Humor",
                        Body = "Humor is a very greate genre"
                    },
                    new Genre
                    {
                        Id = 10,
                        Title = "Romantic",
                        Body = "Romantic is a very greate genre"
                    },
                    new Genre
                    {
                        Id = 11,
                        Title = "FPS",
                        Body = "FPS is a very greate genre"
                    },
                };

                // BUG: must call this before init game models so that the genres automatically initialize its Ids
                // so that we can use to store it in GenreIds field in game models
                // and don't have to hard code it
                // Myth? fuck gpt-4
                context.Genre.AddRange(genres);

                var numGames = 20;
                var games = new Game[numGames];
                for (int i = 0; i < numGames; i++)
                {
                    var genreIds = new List<int>();
                    var genre0 = ran.Next(0, genres.Length - 1);
                    var genre1 = ran.Next(0, genres.Length - 1);
                    var genre2 = ran.Next(0, genres.Length - 1);
                    genreIds.Add(genres[genre0].Id);
                    genreIds.Add(genres[genre1].Id);
                    genreIds.Add(genres[genre2].Id);
                    Game currGame = new Game
                    {
                        Title = $"Game {i}",
                        Body = $"Game {i} is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[ran.Next(0, companies.Length - 1)],
                        GenreIds = genreIds,
                    };
                    games[i] = currGame;
                }

                context.Game.AddRange(games);

                // save changes make to _context
                context.SaveChanges();
            }
        }
    }
}
