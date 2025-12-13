using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;
using System;

namespace AnalyzePublicationsWeb.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorLogic _authorLogic;
        private readonly IPublicationLogic _publicationLogic;

        public AuthorController(IAuthorLogic authorLogic, IPublicationLogic publicationLogic)
        {
            _authorLogic = authorLogic;
            _publicationLogic = publicationLogic;
        }

        // Редактирование - список для управления
        public IActionResult Index()
        {
            var authors = _authorLogic.ReadList(null);
            return View(authors);
        }

        // Просмотр с сортировкой
        public IActionResult Browse(string sortBy = "FullName", string sortOrder = "asc")
        {
            var authors = _authorLogic.ReadList(null) ?? new List<AuthorViewModel>();

            // Применяем сортировку
            authors = SortAuthors(authors, sortBy, sortOrder);

            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

            return View(authors);
        }

        // Поиск с фильтрами
        public IActionResult Search(string? fullName, string? affiliation, string? email,
            string? sortBy = "FullName", string? sortOrder = "asc")
        {
            var searchModel = new AuthorSearchModel
            {
                FullName = fullName,
                Affiliation = affiliation,
                Email = email
            };

            var authors = _authorLogic.ReadList(searchModel) ?? new List<AuthorViewModel>();

            // Применяем сортировку
            authors = SortAuthors(authors, sortBy ?? "FullName", sortOrder ?? "asc");

            ViewBag.FullName = fullName;
            ViewBag.Affiliation = affiliation;
            ViewBag.Email = email;
            ViewBag.SortBy = sortBy ?? "FullName";
            ViewBag.SortOrder = sortOrder ?? "asc";

            return View(authors);
        }

        private List<AuthorViewModel> SortAuthors(List<AuthorViewModel> authors, string sortBy, string sortOrder)
        {
            var isAscending = sortOrder?.ToLower() == "asc";

            return sortBy?.ToLower() switch
            {
                "fullname" => isAscending
                    ? authors.OrderBy(a => a.FullName).ToList()
                    : authors.OrderByDescending(a => a.FullName).ToList(),
                "affiliation" => isAscending
                    ? authors.OrderBy(a => a.Affiliation).ToList()
                    : authors.OrderByDescending(a => a.Affiliation).ToList(),
                "email" => isAscending
                    ? authors.OrderBy(a => a.Email).ToList()
                    : authors.OrderByDescending(a => a.Email).ToList(),
                "publicationscount" => isAscending
                    ? authors.OrderBy(a => a.PublicationsCount).ToList()
                    : authors.OrderByDescending(a => a.PublicationsCount).ToList(),
                "coauthorscount" => isAscending
                    ? authors.OrderBy(a => a.CoauthorsCount).ToList()
                    : authors.OrderByDescending(a => a.CoauthorsCount).ToList(),
                "citationcount" => isAscending
                    ? authors.OrderBy(a => a.CitationCount).ToList()
                    : authors.OrderByDescending(a => a.CitationCount).ToList(),
                "hindex" => isAscending
                    ? authors.OrderBy(a => a.HIndex).ToList()
                    : authors.OrderByDescending(a => a.HIndex).ToList(),
                "averagepages" => isAscending
                    ? authors.OrderBy(a => a.AveragePages).ToList()
                    : authors.OrderByDescending(a => a.AveragePages).ToList(),
                _ => authors
            };
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

        // Детальная информация об авторе
        public IActionResult Details(int id)
        {
            var author = _authorLogic.ReadElement(new AuthorSearchModel { Id = id });
            if (author == null) return NotFound();

            // Получаем публикации автора
            var publications = _publicationLogic?.ReadListByAuthor(id) ?? new List<PublicationViewModel>();
            ViewBag.Publications = publications;

            return View(author);
        }
    }
}
