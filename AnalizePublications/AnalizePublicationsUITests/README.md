# Тесты для AnalizePublications

Проект содержит два типа тестов: модульные (Unit Tests) и UI тесты.

## Структура проекта

```
AnalizePublicationsUITests/
├── UnitTests/              # Модульные тесты
│   ├── AuthorLogicTests.cs
│   ├── PublicationLogicTests.cs
│   └── SourceLogicTests.cs
├── UITests/                # UI тесты
│   ├── AuthorControllerTests.cs
│   ├── PublicationControllerTests.cs
│   ├── SourceControllerTests.cs
│   ├── TestDataControllerTests.cs
│   ├── IntegrationTests.cs
│   ├── ContentTests.cs
│   ├── HomePageTests.cs
│   ├── AuthorTests.cs
│   ├── PublicationTests.cs
│   ├── FlaUIWebTests.cs
│   └── BaseUITest.cs
└── AnalizePublicationsUITests.csproj
```

## Модульные тесты (UnitTests/)

Модульные тесты проверяют бизнес-логику изолированно, используя моки (Moq) для зависимостей.

### Покрытие:
- **AuthorLogicTests**: Тестирование логики работы с авторами
  - Чтение списка (с фильтрами и без)
  - Чтение элемента
  - Создание (с валидацией)
  - Обновление
  - Удаление
  - Проверка дубликатов

- **PublicationLogicTests**: Тестирование логики работы с публикациями
  - Чтение списка
  - Чтение элемента
  - Создание (с валидацией авторов)
  - Обновление
  - Удаление
  - Получение публикаций по автору

- **SourceLogicTests**: Тестирование логики работы с источниками
  - Чтение списка
  - Чтение элемента
  - Создание (с валидацией URL)
  - Обновление
  - Удаление

## UI тесты (UITests/)

UI тесты проверяют работу веб-интерфейса через HTTP запросы и автоматизацию браузера.

### Типы UI тестов:

1. **HTTP интеграционные тесты** (WebApplicationFactory):
   - `AuthorControllerTests` - тесты контроллера авторов
   - `PublicationControllerTests` - тесты контроллера публикаций
   - `SourceControllerTests` - тесты контроллера источников
   - `TestDataControllerTests` - тесты контроллера тестовых данных
   - `IntegrationTests` - интеграционные тесты навигации
   - `ContentTests` - проверка содержимого страниц
   - `HomePageTests` - тесты главной страницы
   - `AuthorTests` - базовые тесты страниц авторов
   - `PublicationTests` - базовые тесты страниц публикаций

2. **Браузерные тесты** (FlaUI):
   - `FlaUIWebTests` - автоматизация браузера для проверки загрузки страниц

### Покрытие UI тестов:
- Доступность всех страниц (Index, Browse, Search, Create, Edit, Delete, Details)
- Сортировка по всем полям
- Поиск и фильтрация
- CRUD операции
- Экспорт отчетов (PDF, Word, Excel)
- Навигация между страницами
- Проверка содержимого страниц

## Запуск тестов

### В Visual Studio:
1. Откройте Test Explorer (Test → Test Explorer)
2. Выберите категорию тестов:
   - Модульные тесты: фильтр по `UnitTests`
   - UI тесты: фильтр по `UITests`
3. Запустите выбранные тесты

### Через командную строку:

**Все тесты:**
```bash
dotnet test
```

**Только модульные тесты:**
```bash
dotnet test --filter "FullyQualifiedName~UnitTests"
```

**Только UI тесты:**
```bash
dotnet test --filter "FullyQualifiedName~UITests"
```

**Только HTTP интеграционные тесты (без FlaUI):**
```bash
dotnet test --filter "FullyQualifiedName~UITests&FullyQualifiedName!~FlaUI"
```

## Зависимости

### Для модульных тестов:
- xUnit
- Moq (для создания моков)

### Для UI тестов:
- xUnit
- Microsoft.AspNetCore.Mvc.Testing (для HTTP тестов)
- FlaUI.Core, FlaUI.UIA3 (для браузерных тестов)

## Примечания

- Модульные тесты не требуют запущенного приложения
- HTTP интеграционные тесты используют WebApplicationFactory и создают тестовый сервер автоматически
- FlaUI тесты требуют запущенного приложения и браузера (Edge или Chrome)
- Для FlaUI тестов можно настроить URL через переменную окружения `TEST_BASE_URL`
