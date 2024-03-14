using Microsoft.AspNetCore.Mvc;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly PeopleManagerDbContext _dbContext;

        public OrganizationsController(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var organizations = _dbContext.Organizations.ToList();
            return View(organizations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Organization organization)
        {
            _dbContext.Organizations.Add(organization);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            var organization = _dbContext.Organizations
                .FirstOrDefault(p => p.Id == id);

            if (organization is null)
            {
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [FromForm] Organization organization)
        {
            var dbOrganization = _dbContext.Organizations
                .FirstOrDefault(p => p.Id == id);

            if (dbOrganization is null)
            {
                return RedirectToAction("Index");
            }

            dbOrganization.Name = organization.Name;
            dbOrganization.Description = organization.Description;

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
