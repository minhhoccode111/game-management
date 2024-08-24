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
            if (!gameVM.GameCompanies.Any())
            {
                ModelState.AddModelError("GameCompanies", "At least one game company is required.");
            }

            if (!gameVM.GenreIds.Any())
            {
                ModelState.AddModelError("GenreIds", "At least one game genre is required.");
            }

            // race condition
            if (!GenreExist(gameVM.GenreIds))
            {
                ModelState.AddModelError(
                    "GenreIds",
                    "Some game genres not exist in current database."
                );
            }

            foreach (var c in gameVM.GameCompanies)
            {
                if (string.IsNullOrWhiteSpace(c.Title))
                {
                    ModelState.AddModelError("GameCompanies", "Company Title is required.");
                }

                if (string.IsNullOrWhiteSpace(c.Body))
                {
                    ModelState.AddModelError("GameCompanies", "Company Body is required.");
                }

                // race condition
                if (!CompanyExist(c.CompanyId))
                {
                    ModelState.AddModelError(
                        "GameCompanies",
                        "Some game companies not existed in current database."
                    );
                }
            }

            if (ModelState.IsValid)
            {
                Game game = new Game
                {
                    Title = gameVM.Title,
                    Body = gameVM.Body,
                    Rating = gameVM.Rating,
                    Image = gameVM.Image,
                    ReleaseDate = gameVM.ReleaseDate,
                };

                _context.Game.Add(game);
                await _context.SaveChangesAsync(); // generate game.Id

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

            if (!gameVM.GameCompanies.Any())
            {
                ModelState.AddModelError("GameCompanies", "At least one game company is required.");
            }

            if (!gameVM.GenreIds.Any())
            {
                ModelState.AddModelError("GenreIds", "At least one game genre is required.");
            }

            // race condition
            if (!GenreExist(gameVM.GenreIds))
            {
                ModelState.AddModelError(
                    "GenreIds",
                    "Some game genres not exist in current database."
                );
            }

            foreach (var c in gameVM.GameCompanies)
            {
                if (string.IsNullOrWhiteSpace(c.Title))
                {
                    ModelState.AddModelError("GameCompanies", "Company Title is required.");
                }

                if (string.IsNullOrWhiteSpace(c.Body))
                {
                    ModelState.AddModelError("GameCompanies", "Company Body is required.");
                }

                // race condition
                if (!CompanyExist(c.CompanyId))
                {
                    ModelState.AddModelError(
                        "GameCompanies",
                        "Some game companies not existed in current database."
                    );
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    game.Id = (int)gameVM.Id;
                    game.Title = gameVM.Title;
                    game.Body = gameVM.Body;
                    game.Rating = gameVM.Rating;
                    game.Image = gameVM.Image;
                    game.ReleaseDate = gameVM.ReleaseDate;

                    // update game with new data (and old Game.Id)
                    _context.Game.Update(game);

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
                            var gameGenre = new GameGenre { GameId = game.Id, GenreId = genreId };
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
                                GameId = game.Id,
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

            List<int> genreIds = game.GameGenres.Select(gg => gg.GenreId).ToList();

            MultiSelectList genres = await GetAllGenresMultiSelect(genreIds);
            ViewData["Genres"] = genres;

            var companies = await GetAllCompaniesSelectList();
            ViewData["Companies"] = companies;
            ViewData["CompaniesJson"] = JsonConvert.SerializeObject(companies);

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
                // not delete
                game.IsActive = false;
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
