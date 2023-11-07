using Serilog;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Middleware;
using WEB_153501_BYCHKO.Models;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;
using WEB_153501_BYCHKO.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

// Add services to the container.
builder.Services.AddControllersWithViews();

var uriData = builder.Configuration["UriData:ApiUri"];

builder.Services
.AddHttpClient<IProductService, ApiProductService>(opt =>
opt.BaseAddress = new Uri(uriData!));

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt => 
opt.BaseAddress = new Uri(uriData!));

builder.Services.AddRazorPages();

builder.Services.AddScoped(typeof(Cart), sp => SessionCart.GetCart(sp));

builder.Services.AddAuthentication(opt =>
{
opt.DefaultScheme = "cookie";
opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
 {
    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
    // Получить Claims пользователя
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";
    options.SaveTokens = true;
 });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages().RequireAuthorization();

app.Run();
