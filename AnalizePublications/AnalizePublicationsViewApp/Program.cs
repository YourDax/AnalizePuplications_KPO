using AnalizePuplicationsContracts.BusinessLogicContracts;
using AnalizePuplicationsBusinessLogic.BusinessLogic;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsDatabaseImplement.Implements;

var builder = WebApplication.CreateBuilder(args);

// ��������� ����������� � Views
builder.Services.AddControllersWithViews();

// ----------------------
// ����������� ������-������
// ----------------------
builder.Services.AddScoped<IAuthorLogic, AuthorLogic>();
builder.Services.AddScoped<IPublicationLogic, PublicationLogic>();
builder.Services.AddScoped<ISourceLogic, SourceLogic>();

// ----------------------
// ����������� �������� (Storage)
// ----------------------
builder.Services.AddScoped<IAuthorStorage, AuthorStorage>(); // ���� ����������
builder.Services.AddScoped<IPublicationStorage, PublicationStorage>();
builder.Services.AddScoped<ISourceStorage, SourceStorage>();

// Регистрация сервисов для тестовых данных
builder.Services.AddScoped<AnalyzePublicationsWeb.Services.TestDataGenerator>();

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
