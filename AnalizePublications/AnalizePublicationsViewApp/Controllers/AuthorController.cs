using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;

namespace AnalyzePublicationsWeb.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorLogic _authorLogic;

        public AuthorController(IAuthorLogic authorLogic)
        {
            _authorLogic = authorLogic;
        }

        public IActionResult Index()
        {
            var authors = _authorLogic.ReadList(null);
            return View(authors);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(AuthorBindingModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _authorLogic.Create(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var author = _authorLogic.ReadElement(new AuthorSearchModel { Id = id });
            if (author == null) return NotFound();

            return View(new AuthorBindingModel
            {
                Id = author.Id,
                FullName = author.FullName
            });
        }

        [HttpPost]
        public IActionResult Edit(AuthorBindingModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _authorLogic.Update(model);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var author = _authorLogic.ReadElement(new AuthorSearchModel { Id = id });
            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _authorLogic.Delete(new AuthorBindingModel { Id = id });
            return RedirectToAction("Index");
        }
    }
}
