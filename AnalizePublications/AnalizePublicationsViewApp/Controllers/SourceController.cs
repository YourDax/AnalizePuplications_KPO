using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;

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

            bool result = _sourceLogic.Create(model);
            if (!result)
            {
                ModelState.AddModelError("", "Не удалось создать источник");
                return View(model);
            }

            return RedirectToAction("Index");
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

            bool result = _sourceLogic.Update(model);
            if (!result)
            {
                ModelState.AddModelError("", "Не удалось обновить источник");
                return View(model);
            }

            return RedirectToAction("Index");
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
            _sourceLogic.Delete(new SourceBindingModel { Id = id });
            return RedirectToAction("Index");
        }
    }
}
