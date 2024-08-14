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
                if (
                    context.Game.Any()
                    || context.Company.Any()
                    || context.Genre.Any()
                    || context.GameCompany.Any()
                    || context.GameCompany.Any()
                )
                {
                    // uncomment to clear database

                    // context.Game.RemoveRange(context.Game);
                    // context.Company.RemoveRange(context.Company);
                    // context.Genre.RemoveRange(context.Genre);
                    // context.GameGenre.RemoveRange(context.GameGenre);
                    // context.GameCompany.RemoveRange(context.GameCompany);
                    // context.SaveChanges();
                    // Console.WriteLine("Database clear!");

                    return;
                }

                var ran = new Random();

                // seed companies
                var companyList = new Company[]
                {
                    // MAANG
                    new Company
                    {
                        Title = "Meta",
                        Body = "Some Meta description",
                        Image = "https://images.cnbctv18.com/wp-content/uploads/2022/09/Meta.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    },
                    new Company
                    {
                        Title = "Apple",
                        Body = "Some Apple description",
                        Image =
                            "https://dm0qx8t0i9gc9.cloudfront.net/thumbnails/video/UD7CEz6/editorial-apple-inc-logo-on-glass-building_smk22zqcg_thumbnail-1080_01.png",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    },
                    new Company
                    {
                        Title = "Amazon",
                        Body = "Some Amazon description",
                        Image =
                            "https://www.wealthandfinance-news.com/wp-content/uploads/2020/01/amazon.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    },
                    new Company
                    {
                        Title = "Netflix",
                        Body = "Some Netflix description",
                        Image =
                            "https://s.aolcdn.com/hss/storage/midas/dae3c205f61d252afbea973ef0409803/206200911/Netflix+Media_0193+2.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    },
                    new Company
                    {
                        Title = "Google",
                        Body = "Some Google description",
                        Image = "https://wallpapercave.com/wp/kmmXJbb.jpg",
                        FoundingDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    }
                };

                context.Company.AddRange(companyList);
                context.SaveChanges();
                List<Company> companies = context.Company.ToList();

                // seed genres
                var genreList = new Genre[]
                {
                    new Genre { Title = "Action", Body = "Some Action description" },
                    new Genre { Title = "Sport", Body = "Some Sport description" },
                    new Genre { Title = "Horror", Body = "Some Horror description" },
                    new Genre { Title = "Humor", Body = "Some Humor description" },
                    new Genre { Title = "Romantic", Body = "Some Romantic description" },
                    new Genre { Title = "FPS", Body = "Some FPS description" },
                };

                context.Genre.AddRange(genreList);
                context.SaveChanges();
                List<Genre> genres = context.Genre.ToList();

                // seed games
                int numGames = 40;
                Game[] gameList = new Game[numGames];
                for (int i = 0; i < numGames; i++)
                {
                    Company company = companies[ran.Next(0, companies.Count)];
                    Game currGame = new Game
                    {
                        Title = $"Game {i}",
                        Body = $"Some Game {i} description",
                        Rating = ran.Next(1, 6),
                        ReleaseDate = DateTime.Parse(
                            $"{ran.Next(2000, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        ),
                    };
                    gameList[i] = currGame;
                }

                context.Game.AddRange(gameList);
                context.SaveChanges();
                List<Game> games = context.Game.ToList();

                // seed GameGenre
                int numGameGenres = games.Count;
                List<GameGenre> gameGenreList = new List<GameGenre>();
                for (int i = 0; i < numGameGenres; i++)
                {
                    Dictionary<int, bool> trackUsedGenreIds = new Dictionary<int, bool>();
                    for (int j = 0; j < 3; j++)
                    {
                        Game game = games[i];

                        Genre genre = genres[ran.Next(0, genres.Count)];

                        // keep generate new genre id until it's not in the dict
                        do
                        {
                            genre = genres[ran.Next(0, genres.Count)];
                        } while (trackUsedGenreIds.ContainsKey(genre.Id));

                        GameGenre currGameGenre = new GameGenre { Game = game, Genre = genre };
                        trackUsedGenreIds.Add(genre.Id, true);
                        gameGenreList.Add(currGameGenre);
                    }
                }
                context.GameGenre.AddRange(gameGenreList);
                context.SaveChanges();

                // seed GameCompany
                int numGameCompanies = games.Count;
                List<GameCompany> gameCompanyList = new List<GameCompany>();
                for (int i = 0; i < numGameCompanies; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        DateTime startDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        );
                        DateTime endDate = DateTime.Parse(
                            $"{ran.Next(1990, 2024)}-{ran.Next(1, 12)}-{ran.Next(1, 28)}"
                        );
                        GameCompany currGameCompany = new GameCompany
                        {
                            Game = games[i],
                            Company = companies[ran.Next(0, companies.Count)],
                            Title = $"Develop phase {j}",
                            Body = $"Develop phase {j} description",
                            StartDate = startDate,
                            EndDate = endDate,
                        };
                        gameCompanyList.Add(currGameCompany);
                    }
                }
                context.GameCompany.AddRange(gameCompanyList);
                context.SaveChanges();
            }
        }
    }
}
