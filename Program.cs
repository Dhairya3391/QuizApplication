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
    var path = context.Request.Path.Value;
    System.Diagnostics.Debug.WriteLine($"Raw request path: {path} (Method: {context.Request.Method})");
    var lowerPath = path.ToLower();
    System.Diagnostics.Debug.WriteLine($"Lowered request path: {lowerPath}");

    // Skip middleware for static files
    if (lowerPath.StartsWith("/css") || lowerPath.StartsWith("/assets") || lowerPath.StartsWith("/js"))
    {
        System.Diagnostics.Debug.WriteLine("Static file request, skipping middleware");
        await next.Invoke();
        return;
    }

    bool isLogin = lowerPath == "/auth/login" || lowerPath.StartsWith("/auth/login/");
    bool isRegister = lowerPath == "/auth/register" || lowerPath.StartsWith("/auth/register/");
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
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Auth", action = "Login" }
);

app.Run();
