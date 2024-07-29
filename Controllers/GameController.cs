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
        private readonly GameManagementMvcContext _context;

        public GameController(GameManagementMvcContext context)
        {
            _context = context;
        }

        // GET: Game
        public async Task<IActionResult> Index(
            string searchTitle,
            string searchCompany,
            string searchGenre
        )
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'GameManagementMvc.Game' is null.");
            }

            IQueryable<string> genreQuery =
                from genre in _context.Genre
                orderby genre.Title
                select genre.Title;
            IQueryable<string> companyQuery =
                from company in _context.Company
                orderby company.Title
                select company.Title;

            var games = _context.Game.Include(game => game.Company).AsQueryable();

            if (!String.IsNullOrEmpty(searchTitle))
            {
                games = games.Where(game => game.Title!.ToUpper().Contains(searchTitle.ToUpper()));
            }

            if (!String.IsNullOrEmpty(searchCompany))
            {
                games = games.Where(game => game.Company!.Title == searchCompany);
            }

            if (!String.IsNullOrEmpty(searchGenre))
            {
                games = games.Where(game =>
                    game.GenreIds!.Contains(
                        _context
                            .Genre.Where(genre => genre.Title == searchGenre)
                            .Select(g => g.Id)
                            .FirstOrDefault()
                    )
                );
            }

            var gameList = await games.ToListAsync();

            var genreList = await _context.Genre.ToListAsync();

            foreach (var game in gameList)
            {
                game.Genres = genreList.Where(genre => game.GenreIds!.Contains(genre.Id)).ToList();
            }

            var gameVM = new GameViewModel
            {
                SearchTitle = searchTitle,
                SearchCompany = searchCompany,
                SearchGenre = searchGenre,
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Companies = new SelectList(await companyQuery.Distinct().ToListAsync()),
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

            return View(game);
        }

        // GET: Game/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
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
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FirstOrDefaultAsync(m => m.Id == id);
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
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}
