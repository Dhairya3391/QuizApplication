using QuizApplication.DbConfigruation;
using QuizApplication.QuestionCRUD;
using QuizApplication.QuestionLevelCRUD;
using QuizApplication.QuizCRUD;
using QuizApplication.QuizWiseQuestionCRUDCRUD;
using QuizApplication.UserCRUD;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DbConfiguration>();
builder.Services.AddSingleton<QuestionCRUD>();
builder.Services.AddSingleton<QuestionLevelCRUD>();
builder.Services.AddSingleton<QuizCRUD>();
builder.Services.AddSingleton<QuizWiseQuestionCRUD>();
builder.Services.AddSingleton<UserCRUD>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
