using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.ViewModels;
using System;

namespace AnalyzePublicationsWeb.Controllers
{
    public class SourceController : Controller
    {
        private readonly ISourceLogic _sourceLogic;

        public SourceController(ISourceLogic sourceLogic)
        {
            _sourceLogic = sourceLogic;
        }

        // Редактирование - список для управления
        public IActionResult Index()
        {
            var list = _sourceLogic.ReadList(null);
            return View(list);
        }

        // Просмотр с сортировкой
        public IActionResult Browse(string sortBy = "Name", string sortOrder = "asc")
        {
            var sources = _sourceLogic.ReadList(null) ?? new List<SourceViewModel>();
            
            // Применяем сортировку
            sources = SortSources(sources, sortBy, sortOrder);
            
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            
            return View(sources);
        }

        // Поиск с фильтрами
        public IActionResult Search(string? name, int? sourceType, string? sortBy = "Name", string? sortOrder = "asc")
        {
            var searchModel = new SourceSearchModel
            {
                Name = name,
                SourceType = sourceType
            };
            
            var sources = _sourceLogic.ReadList(searchModel) ?? new List<SourceViewModel>();
            
            // Применяем сортировку
            sources = SortSources(sources, sortBy ?? "Name", sortOrder ?? "asc");
            
            ViewBag.Name = name;
            ViewBag.SourceType = sourceType;
            ViewBag.SortBy = sortBy ?? "Name";
            ViewBag.SortOrder = sortOrder ?? "asc";
            
            return View(sources);
        }

        private List<SourceViewModel> SortSources(List<SourceViewModel> sources, string sortBy, string sortOrder)
        {
            var isAscending = sortOrder?.ToLower() == "asc";
            
            return sortBy?.ToLower() switch
            {
                "name" => isAscending
                    ? sources.OrderBy(s => s.Name).ToList()
                    : sources.OrderByDescending(s => s.Name).ToList(),
                "sourcetype" => isAscending
                    ? sources.OrderBy(s => s.SourceType).ToList()
                    : sources.OrderByDescending(s => s.SourceType).ToList(),
                "url" => isAscending
                    ? sources.OrderBy(s => s.Url ?? "").ToList()
                    : sources.OrderByDescending(s => s.Url ?? "").ToList(),
                _ => sources
            };
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SourceBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool result = _sourceLogic.Create(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось создать источник");
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
            var source = _sourceLogic.ReadElement(new SourceSearchModel { Id = id });
            if (source == null) return NotFound();

            return View(new SourceBindingModel
            {
                Id = source.Id,
                Name = source.Name,
                Url = source.Url,
                SourceType = source.SourceType
            });
        }

        [HttpPost]
        public IActionResult Edit(SourceBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bool result = _sourceLogic.Update(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Не удалось обновить источник");
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
            var source = _sourceLogic.ReadElement(new SourceSearchModel { Id = id });
            if (source == null) return NotFound();

            return View(source);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool result = _sourceLogic.Delete(new SourceBindingModel { Id = id });
                if (!result)
                {
                    TempData["ErrorMessage"] = "Не удалось удалить источник";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Детальная информация об источнике
        public IActionResult Details(int id)
        {
            var source = _sourceLogic.ReadElement(new SourceSearchModel { Id = id });
            if (source == null) return NotFound();

            return View(source);
        }
    }
}
