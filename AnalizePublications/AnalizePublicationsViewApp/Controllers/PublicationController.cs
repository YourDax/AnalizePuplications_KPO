using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
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

        public IActionResult Index()
        {
            var list = _publicationLogic.ReadList(null);
            return View(list);
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
                Year = publication.Year,
                PublicationType = publication.PublicationType,
                AuthorIds = publication.AuthorIds,
                SourceId = publication.SourceId,
                Doi = publication.Doi,
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
