using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.API.Models;
using WEB_153501_BYCHKO.API.Services;
using WEB_153501_BYCHKO.API.Services.CategoryService;
using WEB_153501_BYCHKO.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthorization();
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration.GetSection("isUri").Value;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����� ��� ��������� appUrl �� launchsettings.json �
// productService
builder.Services.AddSingleton(typeof(ConfigurationService));

// ����� ��� ��������� ������������� ���������� ������� �
// productService
builder.Services.AddSingleton(builder.Configuration);

builder.Services.Configure<ConfigData>(builder.Configuration.GetSection("ConfigData"));

builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7004")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

