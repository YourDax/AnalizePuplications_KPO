using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalyzePublicationsBusinessLogic.BusinessLogic;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры с Views
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
builder.Services.AddScoped<IAuthorStorage, AuthorStorage>(); // твоя реализация
builder.Services.AddScoped<IPublicationStorage, PublicationStorage>();
builder.Services.AddScoped<ISourceStorage, SourceStorage>();

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

app.Run();
