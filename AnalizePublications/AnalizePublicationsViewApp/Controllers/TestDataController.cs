using AnalyzePublicationsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AnalyzePublicationsWeb.Controllers
{
    public class TestDataController : Controller
    {
        private readonly TestDataGenerator _testDataGenerator;

        public TestDataController(TestDataGenerator testDataGenerator)
        {
            _testDataGenerator = testDataGenerator;
        }

        [HttpGet]
        public IActionResult ShowDialog()
        {
            try
            {
                // Проверяем, есть ли уже данные
                var hasData = _testDataGenerator.HasExistingData();
                return Json(new { hasData = hasData });
            }
            catch
            {
                return Json(new { hasData = false });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Initialize(string action)
        {
            try
            {
                bool result = false;
                string message = "";

                switch (action?.ToLower())
                {
                    case "clear":
                        // Удалить все данные из базы данных
                        _testDataGenerator.ClearAllData();
                        message = "Все данные успешно удалены из базы данных";
                        result = true;
                        break;

                    case "create":
                    default:
                        // Просто создать, если нет данных
                        result = _testDataGenerator.InitializeTestData(clearExisting: false);
                        if (result)
                        {
                            message = "Тестовые данные успешно созданы (20 авторов, 20 публикаций, 20 источников)";
                        }
                        else
                        {
                            message = "Тестовые данные уже существуют в системе. Используйте 'Удалить все данные' для очистки базы данных.";
                        }
                        break;
                }

                if (result)
                {
                    TempData["SuccessMessage"] = message;
                }
                else
                {
                    TempData["WarningMessage"] = message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка при инициализации данных: {ex.Message}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

