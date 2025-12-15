using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace AnalyzePublicationsWeb.Controllers
{
    public class PublicationController : Controller
    {
        private readonly IPublicationLogic _publicationLogic;
        private readonly IAuthorLogic _authorLogic;
        private readonly ISourceLogic _sourceLogic;

        public PublicationController(IPublicationLogic publicationLogic,
                                     IAuthorLogic authorLogic,
                                     ISourceLogic sourceLogic)
        {
            _publicationLogic = publicationLogic;
            _authorLogic = authorLogic;
            _sourceLogic = sourceLogic;
        }

        // Редактирование - список для управления
        public IActionResult Index()
        {
            var list = _publicationLogic.ReadList(null);
            return View(list);
        }

        // Просмотр с сортировкой
        public IActionResult Browse(string sortBy = "Title", string sortOrder = "asc")
        {
            var publications = _publicationLogic.ReadList(null) ?? new List<PublicationViewModel>();
            
            // Применяем сортировку
            publications = SortPublications(publications, sortBy, sortOrder);
            
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            
            return View(publications);
        }

        // Поиск с фильтрами
        public IActionResult Search(string? title, DateTime? dateFrom, DateTime? dateTo, int? sourceId,
            string? sortBy = "Title", string? sortOrder = "asc")
        {
            var searchModel = new PublicationSearchModel
            {
                Title = title,
                PublicationDateFrom = dateFrom,
                PublicationDateTo = dateTo,
                SourceId = sourceId
            };
            
            var publications = _publicationLogic.ReadList(searchModel) ?? new List<PublicationViewModel>();
            
            // Применяем сортировку
            publications = SortPublications(publications, sortBy ?? "Title", sortOrder ?? "asc");
            
            ViewBag.SearchTitle = title; // Используем SearchTitle вместо Title, чтобы избежать конфликта с ViewData["Title"]
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            ViewBag.SourceId = sourceId;
            ViewBag.SortBy = sortBy ?? "Title";
            ViewBag.SortOrder = sortOrder ?? "asc";
            ViewBag.Sources = _sourceLogic.ReadList(null)?
                .Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem 
                { 
                    Value = s.Id.ToString(), 
                    Text = s.Name 
                })
                .ToList() ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            
            return View(publications);
        }

        private List<PublicationViewModel> SortPublications(List<PublicationViewModel> publications, string sortBy, string sortOrder)
        {
            var isAscending = sortOrder?.ToLower() == "asc";
            
            return sortBy?.ToLower() switch
            {
                "title" => isAscending
                    ? publications.OrderBy(p => p.Title).ToList()
                    : publications.OrderByDescending(p => p.Title).ToList(),
                "publicationdate" => isAscending
                    ? publications.OrderBy(p => p.PublicationDate ?? DateTime.MaxValue).ToList()
                    : publications.OrderByDescending(p => p.PublicationDate ?? DateTime.MinValue).ToList(),
                "publicationtype" => isAscending
                    ? publications.OrderBy(p => p.PublicationType).ToList()
                    : publications.OrderByDescending(p => p.PublicationType).ToList(),
                "pages" => isAscending
                    ? publications.OrderBy(p => p.Pages ?? int.MaxValue).ToList()
                    : publications.OrderByDescending(p => p.Pages ?? int.MinValue).ToList(),
                "citationcount" => isAscending
                    ? publications.OrderBy(p => p.CitationCount).ToList()
                    : publications.OrderByDescending(p => p.CitationCount).ToList(),
                _ => publications
            };
        }

        public IActionResult Create()
        {
            PopulateAuthorsAndSources();
            return View();
        }

        [HttpPost]
        public IActionResult Create(PublicationBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateAuthorsAndSources();
                return View(model);
            }

            try
            {
                bool result = _publicationLogic.Create(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось создать публикацию");
                    PopulateAuthorsAndSources();
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                PopulateAuthorsAndSources();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var publication = _publicationLogic.ReadElement(new PublicationSearchModel { Id = id });
            if (publication == null) return NotFound();

            PopulateAuthorsAndSources();

            return View(new PublicationBindingModel
            {
                Id = publication.Id,
                Title = publication.Title,
                PublicationDate = publication.PublicationDate,
                PublicationType = publication.PublicationType,
                Authors = publication.Authors?.Select(a => new PublicationAuthorBindingModel
                {
                    AuthorId = a.AuthorId,
                    AuthorContribution = a.AuthorContribution
                }).ToList() ?? new List<PublicationAuthorBindingModel>(),
                SourceId = publication.SourceId,
                Doi = publication.Doi,
                Pages = publication.Pages,
                Abstract = publication.Abstract,
                Keywords = publication.Keywords,
                CitationCount = publication.CitationCount
            });
        }

        [HttpPost]
        public IActionResult Edit(PublicationBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateAuthorsAndSources();
                return View(model);
            }

            try
            {
                bool result = _publicationLogic.Update(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось обновить публикацию");
                    PopulateAuthorsAndSources();
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                PopulateAuthorsAndSources();
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            var pub = _publicationLogic.ReadElement(new PublicationSearchModel { Id = id });
            if (pub == null) return NotFound();

            return View(pub);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool result = _publicationLogic.Delete(new PublicationBindingModel { Id = id });
                if (!result)
                {
                    TempData["ErrorMessage"] = "Не удалось удалить публикацию";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Детальная информация о публикации
        public IActionResult Details(int id)
        {
            var publication = _publicationLogic.ReadElement(new PublicationSearchModel { Id = id });
            if (publication == null) return NotFound();

            return View(publication);
        }

        private void PopulateAuthorsAndSources()
        {
            var authors = _authorLogic.ReadList(null);
            var sources = _sourceLogic.ReadList(null);

            ViewBag.Authors = authors?
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FullName })
                .ToList() ?? new List<SelectListItem>();

            ViewBag.Sources = sources?
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Url })
                .ToList() ?? new List<SelectListItem>();
        }
    }
}
