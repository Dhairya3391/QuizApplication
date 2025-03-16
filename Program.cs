using QuizApplication;
using QuizApplication.DbConfigruation;

//using QuizApplication.DbConfigruation;
using QuizApplication.DbConfiguration;
using QuizApplication.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DbConfiguration>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddSingleton<QuestionCRUD>();
builder.Services.AddSingleton<QuestionLevelCRUD>();
builder.Services.AddSingleton<QuizCRUD>();
builder.Services.AddSingleton<QuizWiseQuestionCRUD>();
builder.Services.AddSingleton<UserCrud>();
//builder.Services.AddScoped<CommonVariables>();
builder.Services.AddScoped<UserCrud>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value!.ToLower();

    if (path.StartsWith("/css") || path.StartsWith("/assets") || path.StartsWith("/js"))
    {
        await next.Invoke();
        return;
    }

    var isAuthRoute = path.StartsWith("/auth/");
    var isLoggedIn = context.Session.GetInt32("UserId").HasValue;
    if (isAuthRoute || isLoggedIn)
        await next.Invoke();
    else if (context.Request.Method == "GET")
        context.Response.Redirect("/Auth/Login"); // Redirect to Login
    else
        await next.Invoke();
});
app.MapControllerRoute(
    "default",
    "{controller=Auth}/{action=Login}/{id?}"
);
app.Run();