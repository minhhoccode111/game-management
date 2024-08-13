using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        private readonly GameManagementMvcContext _context;

        public GameController(ILogger<GameController> logger, GameManagementMvcContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Game
        public async Task<IActionResult> Index(
            string searchRating,
            string searchTitle,
            string searchGenre,
            string searchCompany,
            string sortBy
        )
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'GameManagementMvc.Game' is null.");
            }

            var games = _context.Game.Include(g => g.Company).Include(g => g.Genres).AsQueryable();

            games = GameSortBy(games, sortBy);

            if (int.TryParse(searchRating, out int searchRatingInt))
            {
                games = games.Where(game => game.Rating == searchRatingInt);
            }

            if (!String.IsNullOrEmpty(searchTitle))
            {
                games = games.Where(game => game.Title!.ToUpper().Contains(searchTitle.ToUpper()));
            }

            if (int.TryParse(searchCompany, out int searchCompanyInt))
            {
                games = games.Where(game => game.Company.Id == searchCompanyInt);
            }

            if (int.TryParse(searchGenre, out int searchGenreInt))
            {
                // TODO: filter genre
            }

            var gameList = await games.ToListAsync();

            foreach (var game in gameList)
            {
                //
            }

            // create a view mode to pass to game index view
            var gameVM = new GameViewModel
            {
                SearchCompany = searchCompany,
                SearchRating = searchRating,
                SearchGenre = searchGenre,
                SearchTitle = searchTitle, // default value of search input
                Companies = await GetContextCompaniesSelectList(),
                Genres = await GetContextGenresSelectList(),
                Games = gameList
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

            var game = await _context.Game.FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // game.Genres = await PopulateGenreIdsInGame(game);
            // game.Company = await PopulateCompanyIdInGame(game);

            return View(game);
        }

        // GET: Game/Create
        public async Task<IActionResult> Create()
        {
            var genres = await GetContextGenresMultiSelectList();
            ViewData["Genres"] = genres;
            var companies = await GetContextCompaniesSelectList();
            ViewData["Companies"] = companies;
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you
        // want to bind to. For more details, see
        // http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // only execute the method if anti-forgery-token pass
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Body,Image,Rating,ReleaseDate,CompanyId,GenreIds")] Game game
        )
        {
            // in case race condition
            if (!IsValidCompanyId(game.CompanyId) || !IsValidGenreIds(game.GenreIds))
            {
                var genres = await GetContextGenresMultiSelectList();
                ViewData["Genres"] = genres;
                var companies = await GetContextCompaniesSelectList();
                ViewData["Companies"] = companies;
                // the form pre-populated with user's previous input
                return View("Create", game);
            }

            if (ModelState.IsValid)
            {
                // Company company = await _context.Company.FirstAsync(c =>
                //     c.Id == game.CompanyId
                // );
                // List<Genre> genres = await _context
                //     .Genre.Where(g => game.GenreIds.Any(id => g.Id == id))
                //     .ToListAsync();
                // Game game = new Game
                // {
                //     Company = company,
                //     Genres = genres,
                //     Title = game.Title,
                //     Body = game.Body,
                //     Image = game.Image,
                //     Rating = game.Rating,
                //     ReleaseDate = game.ReleaseDate,
                // };

                _context.Game.Add(game);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            var genres = await GetContextGenresMultiSelectList(game.Genres);
            var companies = await GetContextCompaniesSelectList(game.Company.Id);

            ViewData["Genres"] = genres;
            ViewData["Companies"] = companies;

            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you
        // want to bind to. For more details, see
        // http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,Image,Rating,ReleaseDate,CompanyId,GenreIds")] Game game
        )
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (!IsValidCompanyId(game.CompanyId) || !IsValidGenreIds(game.GenreIds))
            {
                return Redirect($"/Game/Edit/{game.Id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Company company = await _context.Company.FirstAsync(c =>
                    //     c.Id == game.CompanyId
                    // );
                    // List<Genre> genres = await _context
                    //     .Genre.Where(g => game.GenreIds.Any(id => g.Id == id))
                    //     .ToListAsync();
                    // Game game = new Game
                    // {
                    //     Id = game.Id, // keep id to update
                    //     Company = company,
                    //     Genres = genres,
                    //     Title = game.Title,
                    //     Body = game.Body,
                    //     Image = game.Image,
                    //     Rating = game.Rating,
                    //     ReleaseDate = game.ReleaseDate,
                    // };

                    _context.Game.Update(game);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsGameExists(game.Id))
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

            return View(game);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // game.Genres = await PopulateGenreIdsInGame(game);
            // game.Company = await PopulateCompanyIdInGame(game);

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
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################

        // use to sort games base on provided string
        private IQueryable<Game> GameSortBy(IQueryable<Game> games, string sortBy)
        {
            if (sortBy == "name")
                return games.OrderBy(g => g.Title);
            if (sortBy == "-name")
                return games.OrderByDescending(g => g.Title);
            if (sortBy == "date")
                return games.OrderBy(g => g.ReleaseDate);
            if (sortBy == "-date")
                return games.OrderByDescending(g => g.ReleaseDate);
            if (sortBy == "rating")
                return games.OrderBy(g => g.Rating);
            if (sortBy == "-rating")
                return games.OrderByDescending(g => g.Rating);
            return games;
        }

        // use to check if a game exists in current context
        private bool IsGameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        // use to check if a company's id is valid in create and edit action
        private bool IsValidCompanyId(int id)
        {
            return _context.Company.Any(c => c.Id == id);
        }

        // use to check a list of GenreIds are valid genres exist in current
        // context to use when create and edit games
        private bool IsValidGenreIds(List<int>? genreIds = null)
        {
            if (genreIds == null)
            {
                return false;
            }

            var genres = _context.Genre.Select(g => g.Id);

            return genreIds.All(id => genres.Contains(id));
        }

        // use to populate Company field in game model base on CompanyId field
        private async Task<Company?> PopulateCompanyIdInGame(Game game)
        {
            return await _context.Company.FirstOrDefaultAsync(c => c.Id == game.Company.Id);
        }

        // use to populate Genres field in game model base on GenreIds field
        private async Task<List<Genre>> PopulateGenreIdsInGame(Game game)
        {
            return await _context.Genre.ToListAsync();
        }

        // use to generate select dropdown when filter games' company
        private async Task<SelectList> GetContextGenresSelectList(int? selected = null)
        {
            var genres = await _context.Genre.OrderBy(g => g.Title).ToListAsync();

            return new SelectList(genres, "Id", "Title", selected?.ToString());
        }

        /*
           new SelectList(items, dataValueField, dataTextField, selectedValue)
           items: the list of items you want to display in SelectList
           dataValueField: the field will be used for <option>'s value
           dataTextField: the field will be used for <option>'s text display
           selectedValue: the default selected value, have to match one of the
           dataValueField
        */

        // use to generate select dropdown when create, edit and filter games' company
        private async Task<SelectList> GetContextCompaniesSelectList(int? selected = null)
        {
            var companies = await _context.Company.OrderBy(c => c.Title).ToListAsync();

            return new SelectList(companies, "Id", "Title", selected?.ToString());
        }

        // use to generate checkboxes when create and edit games' genres
        private async Task<MultiSelectList> GetContextGenresMultiSelectList(
            List<Genre>? selected = null
        )
        {
            var genres = await _context.Genre.OrderBy(g => g.Title).ToListAsync();

            return new MultiSelectList(genres, "Id", "Title", selected);
        }
    }
}
