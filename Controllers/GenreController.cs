using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class GenreController : Controller
    {
        private readonly GameManagementMvcContext _context;

        public GenreController(GameManagementMvcContext context)
        {
            _context = context;
        }

        // GET: Genre
        public async Task<IActionResult> Index(string title, string sort)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'GameManagementMvc.Genre' is null.");
            }

            IQueryable<Genre> genres = GetAll();

            genres = GenreSortBy(genres, sort);

            if (!String.IsNullOrEmpty(title))
            {
                genres = genres.Where(genre => genre.Title.ToUpper().Contains(title.ToUpper()));
            }

            var genreList = await genres.ToListAsync();

            var genreVM = new GenreViewModel
            {
                Genres = genreList,
                Title = title,
                Sort = sort,
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

            Genre? genre = await GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("Title,Body")] Genre genre)
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

            Genre? genre = await GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

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
                    if (!GenreExist(genre.Id))
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

            Genre? genre = await GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genre.FindAsync(id);

            if (!IsGenreDeletable(id))
            {
                return RedirectToAction(nameof(Delete));
            }

            if (genre != null)
            {
                _context.Genre.Remove(genre);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################

        private async Task<Genre?> GetById(int? id)
        {
            return await _context
                .Genre.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == id);
        }

        private IQueryable<Genre> GetAll()
        {
            return _context
                .Genre.Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Game)
                .AsNoTracking()
                .AsQueryable();
        }

        private bool GenreExist(int id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }

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

        private bool IsGenreDeletable(int id)
        {
            return _context.GameGenre.All(gg => gg.GenreId != id);
        }
    }
}
