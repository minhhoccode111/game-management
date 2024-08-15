using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

            IQueryable<Game> games = _context
                .Game.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .AsNoTracking() // improve perf, for read-only query
                .AsQueryable(); // hold and not execute sql yet

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
                Sort = sort
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

            var game = await _context
                .Game.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Game/Create
        public async Task<IActionResult> Create()
        {
            // TODO:
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Body,Rating,ReleaseDate,Image")] Game game
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
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

            var game = await _context
                .Game.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            List<int> genreIds = game.GameGenres.Select(gg => gg.GenreId).ToList();
            MultiSelectList genres = await GetAllGenresMultiSelect(genreIds);
            ViewData["Genres"] = genres;

            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,Rating,ReleaseDate,Image")] Game game
        )
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
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

            var game = await _context
                .Game.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .Include(g => g.GameCompanies)
                .ThenInclude(gc => gc.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
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
            // TODO: work on this
            // double check if related GameCompanies and GameGenres are also deleted
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

        private bool IsGameExists(int id)
        {
            return false;
        }

        private bool IsValidCompanyId(int id)
        {
            return false;
        }

        private bool IsValidGenreIds(List<int>? genreIds = null)
        {
            return false;
        }

        private async Task<SelectList> GetAllGenresSelectList(int? selected = null)
        {
            List<Genre> genres = await _context.Genre.OrderBy(g => g.Title).ToListAsync();
            return new SelectList(genres, "Id", "Title", selected?.ToString());
        }

        private async Task<SelectList> GetAllCompaniesSelectList(int? selected = null)
        {
            List<Company> companies = await _context.Company.OrderBy(c => c.Title).ToListAsync();
            return new SelectList(companies, "Id", "Title", selected?.ToString());
        }

        private async Task<MultiSelectList> GetAllGenresMultiSelect(List<int>? selected = null)
        {
            List<Genre> genres = await _context.Genre.OrderBy(g => g.Title).ToListAsync();
            return new MultiSelectList(genres, "Id", "Title", selected);
        }

        private async Task<MultiSelectList> GetAllCompaniesMultiSelect(List<int>? selected = null)
        {
            List<Company> companies = await _context.Company.OrderBy(g => g.Title).ToListAsync();
            return new MultiSelectList(companies, "Id", "Title", selected);
        }
    }
}
