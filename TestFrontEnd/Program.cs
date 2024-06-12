using Frontend.Web.Models.CommonModels;
using Frontend.WebUI.Extensions.HttpConfig;
using Frontend.WebUI.Helper.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<TimingHandler>();
builder.Services.AddHttpClient<TaskClient>().AddHttpMessageHandler<TimingHandler>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(ApplicationSettings.SectionKey));
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<ApplicationSettings>>().Value);
int ClaimExpires = builder.Configuration.GetValue<int>("Application:ClaimExpires");
builder.Services.AddSession(options => {
  options.IdleTimeout = TimeSpan.FromDays(ClaimExpires);//You can set Time   
});
builder.Services.AddAuthentication(options =>
{
  options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{

  //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  //options.Cookie.SameSite = SameSiteMode.Lax;
  options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
  options.LoginPath = "/Account/Login";
  options.AccessDeniedPath = "/Account/AccessDenied";
  options.Cookie.IsEssential = true;
  options.SlidingExpiration = true; // here 1
  options.ExpireTimeSpan = TimeSpan.FromDays(ClaimExpires);// here 2
});
builder.Services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
