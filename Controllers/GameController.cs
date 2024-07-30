// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.Rendering;
using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class GameController : Controller
    {
        // logging
        private readonly ILogger<GameController> _logger;

        // db context
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
            string searchCompany
        )
        {
            // if Game model not exists in current context
            if (_context.Game == null)
            {
                // produce a problem detail response
                return Problem("Entity set 'GameManagementMvc.Game' is null.");
            }

            // use `var` to infer the type in case we change in the future
            // so that we don't have to change everywhere

            // select all game in current context, include its company
            // and make as queryable
            var games = _context.Game.Include(game => game.Company).AsQueryable();
            // var games = from game in _context.Game select game;

            // if search rating is provided then we try convert it to int
            // if success then it will return true and a variable SearchRating
            // type int to use
            if (int.TryParse(searchRating, out int SearchRating))
            {
                // filter search rating
                games = games.Where(game => game.Rating == SearchRating);
            }

            // if search title is provided
            if (!String.IsNullOrEmpty(searchTitle))
            {
                // filter search title
                games = games.Where(game => game.Title!.ToUpper().Contains(searchTitle.ToUpper()));
            }

            // if search company is provided
            if (!String.IsNullOrEmpty(searchCompany))
            {
                // filter search company
                games = games.Where(game => game.Company!.Title == searchCompany);
            }

            // if search genre is provided
            if (!String.IsNullOrEmpty(searchGenre))
            {
                // find the genre has title match searchGenre, then extract its id
                int searchGenreId = _context
                    .Genre.FirstOrDefault(genre => genre.Title == searchGenre)!
                    .Id;
                // filter base on search genre
                games = games.Where(game => game.GenreIds!.Contains(searchGenreId));
            }

            // make all games left a list
            var gameList = await games.ToListAsync();

            // populate all ids in GenreIds with real Genre models
            foreach (var game in gameList)
            {
                game.Genres = await PopulateGenreIdsInGame(game);
            }

            // create a view mode to pass to game index view
            var gameVM = new GameViewModel
            {
                SearchCompany = searchCompany,
                SearchRating = SearchRating,
                SearchGenre = searchGenre,
                SearchTitle = searchTitle, // default value of search input
                Companies = await GetContextCompaniesSelectList(null),
                Genres = await GetContextGenresSelectList(null),
                Games = gameList
            };

            return View(gameVM);
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // if id not provided
            if (id == null)
            {
                return NotFound();
            }

            // find the game match the id in current context
            var game = await _context
                .Game.Include(g => g.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            // if game not found
            if (game == null)
            {
                return NotFound();
            }

            // populate GenreIds field with real genres
            game.Genres = await PopulateGenreIdsInGame(game);

            return View(game);
        }

        // GET: Game/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Genres"] = await GetContextGenresSelectList(null);
            ViewData["Companies"] = await GetContextCompaniesSelectList(null);
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // only execute the method if anti-forgery-token pass
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Body,Image,Rating,ReleaseDate")] Game game
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
            // if id not provided
            if (id == null)
            {
                return NotFound();
            }

            // find game with id in current context
            var game = await _context.Game.FindAsync(id);

            // if game not found
            if (game == null)
            {
                return NotFound();
            }

            // populate game Genres field, to default check the checkboxes
            game.Genres = await PopulateGenreIdsInGame(game);

            // var genres = await GetContextGenresSelectList(null); // TODO: MultiSelectList
            var companies = await GetContextCompaniesSelectList(game.Company!.Title);

            // pass to genres and companies to update
            ViewData["Genres"] = genres; // TODO: fix this
            ViewData["Companies"] = companies;

            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,Image,Rating,ReleaseDate")] Game game
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
                    if (!GameExists(game.Id))
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
            // if id not provided
            if (id == null)
            {
                return NotFound();
            }

            // find the game match the id in current context
            var game = await _context
                .Game.Include(g => g.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            // if game not found
            if (game == null)
            {
                return NotFound();
            }

            // populate GenreIds field with real genres
            game.Genres = await PopulateGenreIdsInGame(game);

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // find the game match the id in current context
            var game = await _context.Game.FindAsync(id);

            // if game found
            if (game != null)
            {
                // remove game from current context
                _context.Game.Remove(game);
            }

            // save change to current context
            await _context.SaveChangesAsync();

            // Redirects to the specified action using the actionName .
            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################
        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        private async Task<List<Genre>> PopulateGenreIdsInGame(Game game)
        {
            // get all genres in current context (include id)
            var genres = from g in _context.Genre select g;

            // make it a list
            var genreList = await genres.ToListAsync();

            // return a list a genres
            return genreList.Where(genre => game.GenreIds!.Contains(genre.Id)).ToList();
        }

        private async Task<SelectList> GetContextGenresSelectList(string? selected)
        {
            // select every genres' title in current context (to make a select list filter)
            // this make the variable queryable
            var genreQuery = from genre in _context.Genre orderby genre.Title select genre.Title;

            IEnumerable<string> genres = await genreQuery.Distinct().ToListAsync();

            if (String.IsNullOrEmpty(selected))
            {
                return new SelectList(genres);
            }

            return new SelectList(genres, selected);
        }

        private async Task<SelectList> GetContextCompaniesSelectList(string? selected)
        {
            // select every company' title in current context (to make a select list filter)
            // this make the variable queryable
            var companyQuery =
                from company in _context.Company
                orderby company.Title
                select company.Title;

            IEnumerable<string> companies = await companyQuery.Distinct().ToListAsync();

            if (String.IsNullOrEmpty(selected))
            {
                return new SelectList(companies);
            }

            return new SelectList(companies, selected);
        }

        // TODO: GetContextGenresMultiSelectList
    }
}
