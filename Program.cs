using QuizApplication.DbConfigruation;
using QuizApplication.QuestionCRUD;
using QuizApplication.QuestionLevelCRUD;
using QuizApplication.QuizCRUD;
using QuizApplication.QuizWiseQuestionCRUDCRUD;
using QuizApplication.UserCrud;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DbConfiguration>();
builder.Services.AddSingleton<QuestionCRUD>();
builder.Services.AddSingleton<QuestionLevelCRUD>();
builder.Services.AddSingleton<QuizCRUD>();
builder.Services.AddSingleton<QuizWiseQuestionCRUD>();
builder.Services.AddSingleton<UserCrud>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserCrud>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseStaticFiles();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value!.ToLower();

    if (path.StartsWith("/css") || path.StartsWith("/assets") || path.StartsWith("/js"))
    {
        await next.Invoke();
        return;
    }

    bool isAuthRoute = path.StartsWith("/auth/");
    bool isLoggedIn = context.Session.GetInt32("UserId").HasValue;

    if (isAuthRoute || isLoggedIn)
    {
        await next.Invoke();
    }
    else
    {
        context.Response.Redirect("/Auth/Register");
        // context.Response.Redirect("/Auth/Login");
    }
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Register}/{id?}"
    // pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();
