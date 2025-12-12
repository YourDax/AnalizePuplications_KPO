using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
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

        public IActionResult Index()
        {
            var list = _sourceLogic.ReadList(null);
            return View(list);
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
    }
}
