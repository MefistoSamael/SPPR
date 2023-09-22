using WEB_153501_BYCHKO.Models;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;
using WEB_153501_BYCHKO.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped(typeof(ICategoryService),typeof(MemoryCategoryService));
builder.Services.AddScoped(typeof(IProductService), typeof(MemoryProductService));

var uriData = builder.Configuration["UriData:ApiUri"];

builder.Services
.AddHttpClient<IProductService, ApiProductService>(opt =>
opt.BaseAddress = new Uri(uriData!));

builder.Services.AddRazorPages();


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

app.MapRazorPages();

app.Run();
