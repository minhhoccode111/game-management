using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<GameController> _logger;

        private readonly GameManagementMvcContext _context;

        public CompanyController(GameManagementMvcContext context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Company
        public async Task<IActionResult> Index(string searchTitle, string sortBy)
        {
            if (_context.Company == null)
            {
                return Problem("Entity set 'GameManagementMvc.Company' is null.");
            }

            var companies = _context.Company.AsQueryable();

            companies = CompanySortBy(companies, sortBy);

            if (!String.IsNullOrEmpty(searchTitle))
            {
                companies = companies.Where(company =>
                    company.Title!.ToUpper().Contains(searchTitle.ToUpper())
                );
            }

            var companyList = await companies.ToListAsync();

            // foreach (var company in companyList)
            // {
            //     company.Games = await PopulateGamesInCompany(company);
            // }

            var companyVM = new CompanyViewModel
            {
                SearchTitle = searchTitle,
                SortBy = sortBy,
                Companies = companyList
            };

            return View(companyVM);
        }

        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            // company.Games = await PopulateGamesInCompany(company);

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Body,Image,FoundingDate")] Company company
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            // company.Games = await PopulateGamesInCompany(company);

            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,Image,FoundingDate")] Company company
        )
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsCompanyExists(company.Id))
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

            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            // company.Games = await PopulateGamesInCompany(company);

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);

            if (company != null && IsCompanyDeletable(id))
            {
                _context.Company.Remove(company);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ############################## HELPERS ##############################

        // use to sort companies base on provided string
        private IQueryable<Company> CompanySortBy(IQueryable<Company> companies, string sortBy)
        {
            if (sortBy == "name")
                return companies.OrderBy(c => c.Title);
            if (sortBy == "-name")
                return companies.OrderByDescending(c => c.Title);
            if (sortBy == "date")
                return companies.OrderBy(c => c.FoundingDate);
            if (sortBy == "-date")
                return companies.OrderByDescending(c => c.FoundingDate);
            // if (sortBy == "rating")
            //     return companies.OrderBy(c => c.Rating);
            // if (sortBy == "-rating")
            //     return companies.OrderByDescending(c => c.Rating);
            return companies;
        }

        // use to check if a company can be deleted
        private bool IsCompanyDeletable(int id)
        {
            return _context.Game.All(g => g.CompanyId != id);
        }

        // use to check if a company exists
        private bool IsCompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }

        // use to populate Games in Company model base on current Game context
        private async Task<List<Game>> PopulateGamesInCompany(Company company)
        {
            return await _context.Game.Where(g => g.CompanyId == company.Id).ToListAsync();
        }
    }
}
