// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.Rendering;
using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class GenreController : Controller
    {
        // logging
        private readonly ILogger<GameController> _logger;

        // db context
        private readonly GameManagementMvcContext _context;

        public GenreController(GameManagementMvcContext context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Genre
        public async Task<IActionResult> Index(string searchTitle, string sortBy)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'GameManagementMvc.Genre' is null.");
            }

            var genres = _context.Genre.AsQueryable();

            genres = GenreSortBy(genres, sortBy);

            // if search title is provided
            if (!String.IsNullOrEmpty(searchTitle))
            {
                // filter search title
                genres = genres.Where(genre =>
                    genre.Title!.ToUpper().Contains(searchTitle.ToUpper())
                );
            }

            // make all genre left a list
            var genreList = await genres.ToListAsync();

            foreach (var genre in genreList)
            {
                // populate Games of Genre
                genre.Games = await PopulateGamesInGenre(genre);
            }

            // create a view model to pass to genre index view
            var genreVM = new GenreViewModel
            {
                SearchTitle = searchTitle,
                SortBy = sortBy,
                Genres = genreList
            };

            return View(genreVM);
        }

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FirstOrDefaultAsync(m => m.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            genre.Games = await PopulateGamesInGenre(genre);

            return View(genre);
        }

        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        // GET: Genre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            genre.Games = await PopulateGamesInGenre(genre);

            return View(genre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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

            return View(genre);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FirstOrDefaultAsync(m => m.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            genre.Games = await PopulateGamesInGenre(genre);

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genre.FindAsync(id);

            if (genre != null && IsGenreDeletable(id))
            {
                _context.Genre.Remove(genre);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################

        // use to check if a genre exists
        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }

        // use to sort genres base on provided string
        private IQueryable<Genre> GenreSortBy(IQueryable<Genre> genres, string sortBy)
        {
            if (sortBy == "name")
                return genres.OrderBy(g => g.Title);
            if (sortBy == "-name")
                return genres.OrderByDescending(g => g.Title);
            // if (sortBy == "date")
            //     return genres.OrderBy(c => c.FoundingDate);
            // if (sortBy == "-date")
            //     return genres.OrderByDescending(c => c.FoundingDate);
            // if (sortBy == "rating")
            //     return genres.OrderBy(c => c.Rating);
            // if (sortBy == "-rating")
            //     return genres.OrderByDescending(c => c.Rating);
            return genres;
        }

        // use to populate Games in Genre model base on current Game context
        private async Task<List<Game>> PopulateGamesInGenre(Genre genre)
        {
            return await _context.Game.Where(g => g.GenreIds!.Contains(genre.Id)).ToListAsync();
        }

        // use to check if a company can be deleted
        private bool IsGenreDeletable(int id)
        {
            // all Games' GenreIds not match current id
            return _context.Game.All(g => !g.GenreIds!.Contains(id));
        }
    }
}
