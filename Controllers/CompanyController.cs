using GameManagementMvc.Data;
using GameManagementMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameManagementMvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly GameManagementMvcContext _context;

        public CompanyController(GameManagementMvcContext context)
        {
            _context = context;
        }

        // GET: Company
        public async Task<IActionResult> Index(string sort, string title)
        {
            if (_context.Company == null)
            {
                return Problem("Entity set 'GameManagementMvc.Company' is null.");
            }

            var companies = _context
                .Company.Include(c => c.GameCompanies)
                .ThenInclude(gc => gc.Game)
                .AsNoTracking()
                .AsQueryable();

            companies = CompanySortBy(companies, sort);

            if (!String.IsNullOrEmpty(title))
            {
                companies = companies.Where(c => c.Title.ToUpper().Contains(title.ToUpper()));
            }

            var companyList = await companies.ToListAsync();

            var companyVM = new CompanyViewModel { Companies = companyList, Title = title, };

            return View(companyVM);
        }

        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context
                .Company.Include(c => c.GameCompanies)
                .ThenInclude(gc => gc.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

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
            [Bind("Title,Body,FoundingDate,Image")] Company company
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

            var company = await _context
                .Company.Include(c => c.GameCompanies)
                .ThenInclude(gc => gc.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Body,FoundingDate,Image")] Company company
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
                    if (!CompanyExist(company.Id))
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

            var company = await _context
                .Company.Include(c => c.GameCompanies)
                .ThenInclude(gc => gc.Game)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);

            // BUG: Currently allow delete Company even when we set
            // DeleteBehavior.Restrict which cause we to  manually check
            if (!IsCompanyDeletable(id))
            {
                return RedirectToAction(nameof(Delete));
            }

            // company do exist and can be deleted
            if (company != null)
            {
                // remove from current context
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

        private bool IsCompanyDeletable(int id)
        {
            return _context.GameCompany.All(g => g.CompanyId != id);
        }

        private bool CompanyExist(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
