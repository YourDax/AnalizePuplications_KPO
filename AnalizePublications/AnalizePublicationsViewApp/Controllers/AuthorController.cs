using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using System;

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

            try
            {
                bool result = _authorLogic.Create(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось создать автора");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
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

            try
            {
                bool result = _authorLogic.Update(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось обновить автора");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
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
            try
            {
                bool result = _authorLogic.Delete(new AuthorBindingModel { Id = id });
                if (!result)
                {
                    TempData["ErrorMessage"] = "Не удалось удалить автора";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
