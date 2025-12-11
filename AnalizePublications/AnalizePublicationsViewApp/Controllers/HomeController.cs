using Microsoft.AspNetCore.Mvc;
using AnalizePuplicationsContracts.BusinessLogicContracts;

namespace AnalyzePublicationsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorLogic _authorLogic;
        private readonly IPublicationLogic _publicationLogic;
        private readonly ISourceLogic _sourceLogic;

        public HomeController(
            IAuthorLogic authorLogic,
            IPublicationLogic publicationLogic,
            ISourceLogic sourceLogic)
        {
            _authorLogic = authorLogic;
            _publicationLogic = publicationLogic;
            _sourceLogic = sourceLogic;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
