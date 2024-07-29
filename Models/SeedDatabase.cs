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

                    return;
                }

                Random ran = new Random();

                // if no movie exists
                Company[] companies = new Company[]
                {
                    // MAANG
                    new Company
                    {
                        Title = "Meta Corp",
                        Body = "Meta Corp is a very greate company",
                        Image = "https://images.cnbctv18.com/wp-content/uploads/2022/09/Meta.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    },
                    new Company
                    {
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
                        Title = "Google Corp",
                        Body = "Google Corp is a very greate company",
                        Image = "https://wallpapercave.com/wp/kmmXJbb.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                    }
                };

                Genre[] genres = new Genre[]
                {
                    new Genre { Title = "Action", Body = "Action is a very greate genre" },
                    new Genre { Title = "Sport", Body = "Sport is a very greate genre" },
                    new Genre { Title = "Horror", Body = "Horror is a very greate genre" },
                    new Genre { Title = "Humor", Body = "Humor is a very greate genre" },
                    new Genre { Title = "Romantic", Body = "Romantic is a very greate genre" },
                    new Genre { Title = "FPS", Body = "FPS is a very greate genre" },
                };

                Game[] games = new Game[]
                {
                    new Game
                    {
                        Title = "Game 0",
                        Body = "Game 0 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[0],
                        Genres = new List<Genre> { genres[0], genres[1] },
                    },
                    new Game
                    {
                        Title = "Game 1",
                        Body = "Game 1 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[1],
                        Genres = new List<Genre> { genres[1], genres[2] },
                    },
                    new Game
                    {
                        Title = "Game 2",
                        Body = "Game 2 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[2],
                        Genres = new List<Genre> { genres[2], genres[3] },
                    },
                    new Game
                    {
                        Title = "Game 3",
                        Body = "Game 3 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[3],
                        Genres = new List<Genre> { genres[3], genres[4] },
                    },
                    new Game
                    {
                        Title = "Game 4",
                        Body = "Game 4 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[4],
                        Genres = new List<Genre> { genres[4], genres[5] },
                    },
                    new Game
                    {
                        Title = "Game 5",
                        Body = "Game 5 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[0],
                        Genres = new List<Genre> { genres[0], genres[1], genres[2] },
                    },
                    new Game
                    {
                        Title = "Game 6",
                        Body = "Game 6 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[1],
                        Genres = new List<Genre> { genres[1], genres[2], genres[3] },
                    },
                    new Game
                    {
                        Title = "Game 7",
                        Body = "Game 7 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[2],
                        Genres = new List<Genre> { genres[2], genres[3], genres[4] },
                    },
                    new Game
                    {
                        Title = "Game 8",
                        Body = "Game 8 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[3],
                        Genres = new List<Genre> { genres[3], genres[4], genres[5] },
                    },
                    new Game
                    {
                        Title = "Game 9",
                        Body = "Game 9 is a very greate game",
                        // Image= "",
                        Rating = ran.Next(1, 5),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 30)}"
                        ),
                        Company = companies[4],
                        Genres = new List<Genre> { genres[4], genres[5], genres[0] },
                    },
                };

                context.Company.AddRange(companies);
                context.Genre.AddRange(genres);
                context.Game.AddRange(games);

                // save changes make to _context
                context.SaveChanges();
            }
        }
    }
}
