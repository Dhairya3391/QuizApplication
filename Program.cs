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

// Middleware to restrict access
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value.ToLower();
    System.Diagnostics.Debug.WriteLine($"Request path: {path}");
    
    if (path.StartsWith("/css") || path.StartsWith("/assets") || path.StartsWith("/js"))
    {
        System.Diagnostics.Debug.WriteLine("Static file request, skipping middleware");
        await next.Invoke();
        return;
    }

    bool isLogin = path == "/auth/login" || path.StartsWith("/auth/login/");
    bool isRegister = path == "/auth/register" || path.StartsWith("/auth/register/");
    bool isLoggedIn = context.Session.GetInt32("UserId").HasValue;

    System.Diagnostics.Debug.WriteLine($"IsLogin: {isLogin}, IsRegister: {isRegister}, IsLoggedIn: {isLoggedIn}");

    if (isLogin || isRegister || isLoggedIn)
    {
        System.Diagnostics.Debug.WriteLine("Allowing request");
        await next.Invoke();
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("Redirecting to /Auth/Login");
        context.Response.Redirect("/Auth/Login");
    }
});

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Register}/{id?}"
);

app.Run();
