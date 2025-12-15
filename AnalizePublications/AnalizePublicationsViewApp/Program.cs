using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalizePuplicationsBusinessLogic.BusinessLogic;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);

// Регистрация контроллеров и Views
builder.Services.AddControllersWithViews();

// ----------------------
// Регистрация бизнес-логики
// ----------------------
builder.Services.AddScoped<IAuthorLogic, AuthorLogic>();
builder.Services.AddScoped<IPublicationLogic, PublicationLogic>();
builder.Services.AddScoped<ISourceLogic, SourceLogic>();

// ----------------------
// Регистрация хранилищ (Storage)
// ----------------------
builder.Services.AddScoped<IAuthorStorage, AuthorStorage>();
builder.Services.AddScoped<IPublicationStorage, PublicationStorage>();
builder.Services.AddScoped<ISourceStorage, SourceStorage>();

// Регистрация сервисов для тестовых данных
builder.Services.AddScoped<AnalyzePublicationsWeb.Services.TestDataGenerator>();

// Регистрация сервиса экспорта отчетов
builder.Services.AddScoped<AnalyzePublicationsWeb.Services.AuthorReportExportService>();

// ----------------------
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Запускаем приложение (WebApplicationFactory сам управляет запуском в тестах)
app.Run();

// Делаем Program доступным для тестов через WebApplicationFactory
// Это должно быть в самом конце файла, после всех инструкций верхнего уровня
public partial class Program { }
