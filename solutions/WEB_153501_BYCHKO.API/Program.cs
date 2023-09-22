using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.API.Services;
using WEB_153501_BYCHKO.API.Services.CategoryService;
using WEB_153501_BYCHKO.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// нужен для получения appUrl из launchsettings.json в
// productService
builder.Services.AddSingleton(typeof(ConfigurationService));

// нужен для получения макисмального количества страниц в
// productService
builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
