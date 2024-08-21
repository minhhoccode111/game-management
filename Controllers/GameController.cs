using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GameManagementMvc.Controllers
{
    public class GameController : Controller
    {
        private readonly GameManagementMvcContext _context;

        public GameController(GameManagementMvcContext context)
        {
            _context = context;
        }

        // GET: Game
        public async Task<IActionResult> Index(
            int? rating,
            int? genreId,
            int? companyId,
            string sort,
            string title
        )
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'GameManagementMvc.Game' is null.");
            }

            IQueryable<Game> games = GetAll();

            games = GameSortBy(games, sort);

            if (!String.IsNullOrEmpty(title))
            {
                games = games.Where(game => game.Title!.ToUpper().Contains(title.ToUpper()));
            }

            if (rating.HasValue)
            {
                games = games.Where(g => g.Rating == rating);
            }

            if (companyId.HasValue)
            {
                games = games.Where(game =>
                    game.GameCompanies.Any(gc => gc.CompanyId == companyId)
                );
            }

            if (genreId.HasValue)
            {
                games = games.Where(game => game.GameGenres.Any(gc => gc.GenreId == genreId));
            }

            // TODO: Distinct Game.GameCompany.CompanyId (not display a company more than 1 time)

            var gameList = await games.ToListAsync();

            var gameVM = new GameViewModel
            {
                Companies = await GetAllCompaniesSelectList(),
                Genres = await GetAllGenresSelectList(),
                Games = gameList,
                CompanyId = companyId,
                GenreId = genreId,
                Rating = rating,
                Title = title,
                Sort = sort,
            };

            return View(gameVM);
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game? game = await GetById(id);

            if (game == null || !game.IsActive)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Game/Create
        public async Task<IActionResult> Create()
        {
            var genres = await GetAllGenresMultiSelect();
            ViewData["Genres"] = genres;

            var companies = await GetAllCompaniesSelectList();
            ViewData["Companies"] = companies;
            ViewData["CompaniesJson"] = JsonConvert.SerializeObject(companies);

            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Body,Rating,ReleaseDate,Image,GenreIds,GameCompanies")]
                GameFormViewModel gameVM
        )
        {
            // manually validate game form view model
            if (!gameVM.GameCompanies.Any())
            {
                // <span asp-validation-for="GameCompanies" class="text-danger"></span>
                // will be used to display error message we specify
                ModelState.AddModelError("GameCompanies", "At least one game company is required.");
            }
            if (!gameVM.GenreIds.Any())
            {
                ModelState.AddModelError("GenreIds", "At least one game genre is required.");
            }
            // in case race condition
            if (!GenreExist(gameVM.GenreIds))
            {
                ModelState.AddModelError(
                    "GenreIds",
                    "Some game genres not exist in current database."
                );
            }
            foreach (var company in gameVM.GameCompanies)
            {
                if (string.IsNullOrWhiteSpace(company.Title))
                {
                    ModelState.AddModelError("GameCompanies", "Company Title is required.");
                }
                if (string.IsNullOrWhiteSpace(company.Body))
                {
                    ModelState.AddModelError("GameCompanies", "Company Body is required.");
                }
                // in case race condition
                if (!CompanyExist(company.CompanyId))
                {
                    ModelState.AddModelError(
                        "GameCompanies",
                        "Some game companies not existed in current database."
                    );
                }
            }

            if (ModelState.IsValid)
            {
                // extract game data
                Game game = new Game
                {
                    Title = gameVM.Title,
                    Body = gameVM.Body,
                    Rating = gameVM.Rating,
                    Image = gameVM.Image,
                    ReleaseDate = gameVM.ReleaseDate,
                };
                _context.Game.Add(game);
                // WARN: save change to generate Game.Id
                await _context.SaveChangesAsync();

                // create game genres
                foreach (var genreId in gameVM.GenreIds)
                {
                    var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
                    _context.GameGenre.Add(gameGenre);
                }

                // create game companies
                foreach (var companyVM in gameVM.GameCompanies)
                {
                    var gameCompany = new GameCompany
                    {
                        GameId = game.Id,
                        CompanyId = companyVM.CompanyId,
                        Title = companyVM.Title,
                        Body = companyVM.Body,
                        StartDate = companyVM.StartDate,
                        EndDate = companyVM.EndDate,
                    };
                    _context.GameCompany.Add(gameCompany);
                }

                // NOTE: because this is CREATE, no need to check for concurrency?

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // else prepare data again
            var genres = await GetAllGenresMultiSelect();
            ViewData["Genres"] = genres;

            var companies = await GetAllCompaniesSelectList();
            ViewData["Companies"] = companies;
            ViewData["CompaniesJson"] = JsonConvert.SerializeObject(companies);

            return View(gameVM);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game? game = await GetById(id);

            if (game == null || !game.IsActive)
            {
                return NotFound();
            }

            // all game genres already exists
            List<int> genreIds = game.GameGenres.Select(gg => gg.GenreId).ToList();

            // all genres to create new genres, checked exist ones
            MultiSelectList genres = await GetAllGenresMultiSelect(genreIds);
            ViewData["Genres"] = genres;

            // all companies to create new game companies
            var companies = await GetAllCompaniesSelectList();
            ViewData["Companies"] = companies;
            ViewData["CompaniesJson"] = JsonConvert.SerializeObject(companies);

            // game companies already exists
            List<GameCompanyViewModel> gameCompanies = new List<GameCompanyViewModel>();
            foreach (var item in game.GameCompanies.ToList())
            {
                var gameCompany = new GameCompanyViewModel
                {
                    Id = item.Id,
                    GameId = item.GameId,
                    CompanyId = item.CompanyId,
                    Title = item.Title,
                    Body = item.Body,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                };
                gameCompanies.Add(gameCompany);
            }
            ViewData["GameCompaniesJson"] = JsonConvert.SerializeObject(gameCompanies);

            var gameFormVM = new GameFormViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Body = game.Body,
                Rating = game.Rating,
                ReleaseDate = game.ReleaseDate,
                Image = game.Image,
                GenreIds = genreIds,
                GameCompanies = gameCompanies,
            };

            return View(gameFormVM);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,Rating,ReleaseDate,Image,GenreIds,GameCompanies")]
                GameFormViewModel gameVM
        )
        {
            if (id != gameVM.Id)
            {
                return NotFound();
            }

            Game? game = await GetById(id);

            if (game == null || !game.IsActive)
            {
                return NotFound();
            }

            // NOTE: this is duplicate validation with above POST /Game/Create
            // because current don't know how to interact with ModelState object
            // using abstract methods
            // manually validate game form view model
            if (!gameVM.GameCompanies.Any())
            {
                // <span asp-validation-for="GameCompanies" class="text-danger"></span>
                // will be used to display error message we specify
                ModelState.AddModelError("GameCompanies", "At least one game company is required.");
            }
            if (!gameVM.GenreIds.Any())
            {
                ModelState.AddModelError("GenreIds", "At least one game genre is required.");
            }
            // in case race condition
            if (!GenreExist(gameVM.GenreIds))
            {
                ModelState.AddModelError(
                    "GenreIds",
                    "Some game genres not exist in current database."
                );
            }
            foreach (var company in gameVM.GameCompanies)
            {
                if (string.IsNullOrWhiteSpace(company.Title))
                {
                    ModelState.AddModelError("GameCompanies", "Company Title is required.");
                }
                if (string.IsNullOrWhiteSpace(company.Body))
                {
                    ModelState.AddModelError("GameCompanies", "Company Body is required.");
                }
                // in case race condition
                if (!CompanyExist(company.CompanyId))
                {
                    ModelState.AddModelError(
                        "GameCompanies",
                        "Some game companies not existed in current database."
                    );
                }
            }

            // NOTE: this is how to debug ModelState validation
            // foreach (var state in ModelState)
            // {
            //     var key = state.Key;
            //     var errors = state.Value.Errors;
            //     foreach (var error in errors)
            //     {
            //         Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
            //     }
            // }

            if (ModelState.IsValid)
            {
                try
                {
                    // extract game data
                    Game newGame = new Game
                    {
                        Id = (int)gameVM.Id,
                        Title = gameVM.Title,
                        Body = gameVM.Body,
                        Rating = gameVM.Rating,
                        Image = gameVM.Image,
                        ReleaseDate = gameVM.ReleaseDate,
                    };

                    // update game with new data (and old Game.Id)
                    _context.Game.Update(newGame);

                    // first remove old ones that we not use anymore in database
                    IQueryable<GameGenre> removeGameGenres = _context.GameGenre.Where(gg =>
                        gg.GameId == id && gameVM.GenreIds.All(gi => gi != gg.GenreId)
                    );

                    _context.GameGenre.RemoveRange(removeGameGenres);
                    // then create new ones that not in current database
                    foreach (var genreId in gameVM.GenreIds)
                    {
                        bool gameGenreNotInDatabase = _context.GameGenre.All(gg =>
                            gg.GenreId != genreId || gg.GameId != id
                        );

                        if (gameGenreNotInDatabase)
                        {
                            var gameGenre = new GameGenre
                            {
                                GameId = newGame.Id,
                                GenreId = genreId,
                            };
                            _context.GameGenre.Add(gameGenre);
                        }
                    }

                    List<int> gameCompanyKept = gameVM.GameCompanies.Select(gc => gc.Id).ToList();

                    // how to update game companies
                    // first remove old ones that we not use anymore in database
                    IQueryable<GameCompany> removeGameCompanies = _context.GameCompany.Where(gc =>
                        gc.GameId == id && gameCompanyKept.All(gcid => gcid != gc.Id)
                    );

                    _context.GameCompany.RemoveRange(removeGameCompanies);

                    // then create ones that not in current database
                    foreach (var gameCompanyVM in gameVM.GameCompanies)
                    {
                        bool gameCompanyVMNotInDB = _context.GameCompany.All(gc =>
                            gameCompanyVM.Id != gc.Id
                            || gameCompanyVM.GameId != gc.GameId
                            || gameCompanyVM.CompanyId != gc.CompanyId
                        );

                        if (gameCompanyVMNotInDB)
                        {
                            var newGameCompany = new GameCompany
                            {
                                GameId = newGame.Id,
                                CompanyId = gameCompanyVM.CompanyId,
                                Title = gameCompanyVM.Title,
                                Body = gameCompanyVM.Body,
                                StartDate = gameCompanyVM.StartDate,
                                EndDate = gameCompanyVM.EndDate,
                            };
                            _context.GameCompany.Add(newGameCompany);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                // unlike create new Game, we have to check for race condition when edit
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExist((int)gameVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // NOTE: what happens if we redirect to GET /Game/Edit/{Id}
            // so that we don't have to manually prepare data like below
            // are the changes we made to ModelState above presist?
            // and error messages will be display on the client?

            // return Redirect($"/Game/Edit/{game.Id}");

            // all game genres already exists
            List<int> genreIds = game.GameGenres.Select(gg => gg.GenreId).ToList();

            // all genres to create new genres, checked exist ones
            MultiSelectList genres = await GetAllGenresMultiSelect(genreIds);
            ViewData["Genres"] = genres;

            // all companies to create new game companies
            var companies = await GetAllCompaniesSelectList();
            ViewData["Companies"] = companies;
            ViewData["CompaniesJson"] = JsonConvert.SerializeObject(companies);

            // game companies already exists
            List<GameCompanyViewModel> gameCompanies = new List<GameCompanyViewModel>();
            foreach (var item in game.GameCompanies.ToList())
            {
                var gameCompany = new GameCompanyViewModel
                {
                    Id = item.Id,
                    GameId = item.GameId,
                    CompanyId = item.CompanyId,
                    Title = item.Title,
                    Body = item.Body,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                };
                gameCompanies.Add(gameCompany);
            }
            ViewData["GameCompaniesJson"] = JsonConvert.SerializeObject(gameCompanies);

            return View(gameVM);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Game? game = await GetById(id);

            if (game == null || !game.IsActive)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);

            if (game != null)
            {
                game.IsActive = false;
                // _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################

        private async Task<Game?> GetById(int? id)
        {
            return await _context
                .Game.Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == id);
        }

        private IQueryable<Game> GetAll()
        {
            return _context
                .Game.Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .AsNoTracking() // improve perf, for read-only query
                .AsQueryable(); // hold and not execute sql yet
        }

        // use to sort games base on provided string
        private IQueryable<Game> GameSortBy(IQueryable<Game> games, string sort)
        {
            if (sort == "name")
                return games.OrderBy(g => g.Title);
            if (sort == "-name")
                return games.OrderByDescending(g => g.Title);
            if (sort == "date")
                return games.OrderBy(g => g.ReleaseDate);
            if (sort == "-date")
                return games.OrderByDescending(g => g.ReleaseDate);
            if (sort == "rating")
                return games.OrderBy(g => g.Rating);
            if (sort == "-rating")
                return games.OrderByDescending(g => g.Rating);
            return games;
        }

        // CHECK EXISTENCE
        private bool GameExist(int id)
        {
            return _context.Game.Any(g => g.Id == id || g.IsActive);
        }

        private bool CompanyExist(int id)
        {
            return _context.Company.Any(c => c.Id == id || c.IsActive);
        }

        private bool GenreExist(List<int>? genreIds = null)
        {
            if (genreIds == null || !genreIds.Any())
            {
                return false;
            }
            return genreIds.All(id => _context.Genre.Any(g => g.Id == id && g.IsActive));
        }

        // drop down select
        private async Task<SelectList> GetAllGenresSelectList(int? selected = null)
        {
            List<Genre> genres = await _context
                .Genre.Where(g => g.IsActive)
                .OrderBy(g => g.Title)
                .ToListAsync();
            return new SelectList(genres, "Id", "Title", selected?.ToString());
        }

        private async Task<SelectList> GetAllCompaniesSelectList(int? selected = null)
        {
            List<Company> companies = await _context
                .Company.Where(c => c.IsActive)
                .OrderBy(c => c.Title)
                .ToListAsync();
            return new SelectList(companies, "Id", "Title", selected?.ToString());
        }

        // checkboxes
        private async Task<MultiSelectList> GetAllGenresMultiSelect(List<int>? selected = null)
        {
            List<Genre> genres = await _context
                .Genre.Where(g => g.IsActive)
                .OrderBy(g => g.Title)
                .ToListAsync();
            return new MultiSelectList(genres, "Id", "Title", selected);
        }
    }
}
