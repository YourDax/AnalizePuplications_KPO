using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnalyzePublicationsWeb.Services
{
    public class TestDataGenerator
    {
        private readonly IAuthorLogic _authorLogic;
        private readonly IPublicationLogic _publicationLogic;
        private readonly ISourceLogic _sourceLogic;

        private readonly string[] _firstNames = { "Иван", "Петр", "Сергей", "Александр", "Дмитрий", "Андрей", "Михаил", "Алексей", "Николай", "Владимир" };
        private readonly string[] _lastNames = { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов", "Соколов", "Лебедев", "Козлов", "Новиков" };
        private readonly string[] _middleNames = { "Иванович", "Петрович", "Сергеевич", "Александрович", "Дмитриевич", "Андреевич", "Михайлович", "Алексеевич", "Николаевич", "Владимирович" };
        private readonly string[] _affiliations = { "МГУ", "СПбГУ", "МФТИ", "ВШЭ", "МГТУ", "ИТМО", "МИФИ", "МГИМО", "РГУ", "УрФУ" };
        private readonly string[] _domains = { "mail.ru", "gmail.com", "yandex.ru", "university.edu", "research.org" };

        private readonly string[] _publicationTitles = {
            "Алгоритмы машинного обучения", "Квантовые вычисления", "Биоинформатика и геномика",
            "Кибербезопасность в облачных системах", "Нейронные сети глубокого обучения",
            "Распределенные системы", "Компьютерное зрение", "Обработка естественного языка",
            "Блокчейн технологии", "Интернет вещей", "Большие данные", "Квантовая криптография",
            "Робототехника", "Виртуальная реальность", "Искусственный интеллект в медицине",
            "Зеленые технологии", "Нанотехнологии", "Космические исследования", "Климатология",
            "Энергетика будущего"
        };

        private readonly string[] _sourceNames = {
            "Nature", "Science", "IEEE Transactions", "ACM Computing Surveys", "Journal of Machine Learning",
            "Physical Review", "Cell", "The Lancet", "Nature Biotechnology", "Science Advances",
            "Proceedings of the National Academy of Sciences", "Nature Communications", "PLOS ONE",
            "Scientific Reports", "Nature Methods", "Cell Reports", "Nature Genetics", "Science Robotics",
            "Nature Machine Intelligence", "IEEE Computer"
        };

        public TestDataGenerator(
            IAuthorLogic authorLogic,
            IPublicationLogic publicationLogic,
            ISourceLogic sourceLogic)
        {
            _authorLogic = authorLogic;
            _publicationLogic = publicationLogic;
            _sourceLogic = sourceLogic;
        }

        public bool HasExistingData()
        {
            var existingAuthors = _authorLogic.ReadList(null);
            return existingAuthors != null && existingAuthors.Count >= 20;
        }

        public bool InitializeTestData(bool clearExisting = false)
        {
            try
            {
                Debug.WriteLine("=== Начало создания тестовых данных ===");
                
                // Если нужно очистить существующие данные
                if (clearExisting)
                {
                    Debug.WriteLine("Очистка существующих данных...");
                    ClearAllData();
                    Debug.WriteLine("Существующие данные очищены");
                }
                else
                {
                    // Проверяем, есть ли уже данные
                    var existingAuthors = _authorLogic.ReadList(null);
                    if (existingAuthors != null && existingAuthors.Count >= 20)
                    {
                        Debug.WriteLine($"Данные уже существуют: {existingAuthors.Count} авторов");
                        return false; // Данные уже созданы
                    }
                }

                var random = new Random();
                Debug.WriteLine("Начало создания источников...");

                // Создаем источники
                var sources = new List<int>();
                int createdSources = 0;
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        var sourceType = (SourceType)(i % 4);
                        var sourceModel = new SourceBindingModel
                        {
                            Name = _sourceNames[i % _sourceNames.Length] + $" {i + 1}",
                            SourceType = sourceType,
                            Url = $"https://example.com/source{i + 1}"
                        };

                        var result = _sourceLogic.Create(sourceModel);
                        if (result)
                        {
                            var createdSource = _sourceLogic.ReadElement(new AnalyzePublicationsContracts.SearchModels.SourceSearchModel 
                            { 
                                Name = sourceModel.Name 
                            });
                            if (createdSource != null)
                            {
                                sources.Add(createdSource.Id);
                                createdSources++;
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"Не удалось создать источник {i + 1}: {sourceModel.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Ошибка при создании источника {i + 1}: {ex.Message}");
                    }
                }
                Debug.WriteLine($"Создано источников: {createdSources} из 20");

                // Создаем авторов
                var authors = new List<int>();
                int createdAuthors = 0;
                Debug.WriteLine("Начало создания авторов...");
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        // Делаем имена уникальными, добавляя номер
                        var firstName = _firstNames[i % _firstNames.Length];
                        var lastName = _lastNames[i % _lastNames.Length];
                        var middleName = _middleNames[i % _middleNames.Length];
                        var fullName = $"{lastName} {firstName} {middleName} {i + 1}"; // Добавляем номер для уникальности

                        var authorModel = new AuthorBindingModel
                        {
                            FullName = fullName,
                            Affiliation = _affiliations[i % _affiliations.Length],
                            Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@{_domains[i % _domains.Length]}" // Добавляем номер в email для уникальности
                        };

                        var result = _authorLogic.Create(authorModel);
                        if (result)
                        {
                            var createdAuthor = _authorLogic.ReadElement(new AnalyzePublicationsContracts.SearchModels.AuthorSearchModel 
                            { 
                                FullName = fullName 
                            });
                            if (createdAuthor != null)
                            {
                                authors.Add(createdAuthor.Id);
                                createdAuthors++;
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"Не удалось создать автора {i + 1}: {fullName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Ошибка при создании автора {i + 1}: {ex.Message}");
                    }
                }
                Debug.WriteLine($"Создано авторов: {createdAuthors} из 20");

                // Проверяем, что источники и авторы созданы
                if (sources.Count == 0)
                {
                    Debug.WriteLine("Ошибка: не удалось создать источники");
                    return false;
                }

                if (authors.Count == 0)
                {
                    Debug.WriteLine("Ошибка: не удалось создать авторов");
                    return false;
                }

                Debug.WriteLine($"Итого: источников: {sources.Count}, авторов: {authors.Count}");

                // Создаем публикации
                Debug.WriteLine("Начало создания публикаций...");
                int createdPublications = 0;
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        var publicationType = (PublicationType)(i % 6);
                        var publicationDate = DateTime.Now.AddYears(-random.Next(0, 10)).AddMonths(-random.Next(0, 12));

                        // Выбираем случайный источник и автора
                        var selectedSourceId = sources[random.Next(sources.Count)];
                        var selectedAuthorId = authors[random.Next(authors.Count)];

                        if (selectedSourceId <= 0)
                        {
                            Debug.WriteLine($"Ошибка: некорректный SourceId {selectedSourceId} для публикации {i + 1}");
                            continue;
                        }

                        if (selectedAuthorId <= 0)
                        {
                            Debug.WriteLine($"Ошибка: некорректный AuthorId {selectedAuthorId} для публикации {i + 1}");
                            continue;
                        }

                        var publicationModel = new PublicationBindingModel
                        {
                            Title = _publicationTitles[i % _publicationTitles.Length] + $" (Часть {i + 1})",
                            PublicationDate = publicationDate,
                            PublicationType = publicationType,
                            SourceId = selectedSourceId,
                            Doi = $"10.1000/example.{i + 1}",
                            Pages = random.Next(5, 50),
                            Abstract = $"Аннотация для публикации {i + 1}. Это тестовая публикация, созданная для демонстрации функциональности системы.",
                            Keywords = $"тест, публикация {i + 1}, исследование",
                            CitationCount = random.Next(0, 100),
                            Authors = new List<PublicationAuthorBindingModel>
                            {
                                new PublicationAuthorBindingModel
                                {
                                    AuthorId = selectedAuthorId,
                                    AuthorContribution = random.Next(2) == 0 ? "Основной автор" : null
                                }
                            }
                        };

                        // Добавляем еще 1-2 случайных автора
                        if (random.Next(2) == 0 && authors.Count > 1)
                        {
                            var additionalAuthor = authors[random.Next(authors.Count)];
                            if (publicationModel.Authors[0].AuthorId != additionalAuthor)
                            {
                                publicationModel.Authors.Add(new PublicationAuthorBindingModel
                                {
                                    AuthorId = additionalAuthor,
                                    AuthorContribution = "Соавтор"
                                });
                            }
                        }

                        try
                        {
                            var pubResult = _publicationLogic.Create(publicationModel);
                            if (pubResult)
                            {
                                createdPublications++;
                                Debug.WriteLine($"✓ Успешно создана публикация {i + 1}: {publicationModel.Title}");
                            }
                            else
                            {
                                Debug.WriteLine($"✗ Не удалось создать публикацию {i + 1}: {publicationModel.Title}");
                                Debug.WriteLine($"  SourceId: {publicationModel.SourceId}, Authors: {publicationModel.Authors.Count}");
                            }
                        }
                        catch (Exception createEx)
                        {
                            Debug.WriteLine($"✗ Исключение при создании публикации {i + 1}: {createEx.Message}");
                            Debug.WriteLine($"  Тип: {createEx.GetType().Name}");
                            if (createEx.InnerException != null)
                            {
                                Debug.WriteLine($"  Внутреннее исключение: {createEx.InnerException.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"✗ Критическая ошибка при создании публикации {i + 1}: {ex.Message}");
                        Debug.WriteLine($"  Тип: {ex.GetType().Name}");
                        Debug.WriteLine($"  StackTrace: {ex.StackTrace}");
                    }
                }

                Debug.WriteLine($"Создано публикаций: {createdPublications} из 20");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при создании тестовых данных: {ex.Message}");
                return false;
            }
        }

        public void ClearAllData()
        {
            try
            {
                // Удаляем все публикации
                var publications = _publicationLogic.ReadList(null);
                if (publications != null)
                {
                    foreach (var pub in publications)
                    {
                        _publicationLogic.Delete(new AnalyzePublicationsContracts.BindingModels.PublicationBindingModel { Id = pub.Id });
                    }
                }

                // Удаляем всех авторов
                var authors = _authorLogic.ReadList(null);
                if (authors != null)
                {
                    foreach (var author in authors)
                    {
                        _authorLogic.Delete(new AnalyzePublicationsContracts.BindingModels.AuthorBindingModel { Id = author.Id });
                    }
                }

                // Удаляем все источники
                var sources = _sourceLogic.ReadList(null);
                if (sources != null)
                {
                    foreach (var source in sources)
                    {
                        _sourceLogic.Delete(new AnalyzePublicationsContracts.BindingModels.SourceBindingModel { Id = source.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при удалении данных: {ex.Message}");
                throw;
            }
        }
    }
}

