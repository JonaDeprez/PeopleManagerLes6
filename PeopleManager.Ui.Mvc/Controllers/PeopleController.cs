using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PeopleManagerDbContext _dbContext;

        public PeopleController(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var people = _dbContext.People
                .Include(p => p.Organization)
                .ToList();

            return View(people);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person person)
        {
            _dbContext.People.Add(person);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit([FromRoute]int id)
        {
            var person = _dbContext.People
                .FirstOrDefault(p => p.Id == id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }
            
            return View(person);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, [FromForm]Person person)
        {
            var dbPerson = _dbContext.People
                .FirstOrDefault(p => p.Id == id);

            if (dbPerson is null)
            {
                return RedirectToAction("Index");
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Email = person.Email;

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            var person = _dbContext.People
                .FirstOrDefault(p => p.Id == id);

            return View(person);
        }

        [HttpPost("/[controller]/Delete/{id:int}"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            var person = _dbContext.People
                .FirstOrDefault(p => p.Id == id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            _dbContext.People.Remove(person);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
