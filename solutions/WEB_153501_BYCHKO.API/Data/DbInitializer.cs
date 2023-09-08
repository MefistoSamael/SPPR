using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();

            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            var appUrl = app.Configuration["AppUrl"];

            //если категории в бд пусты, создаем их
            if (context.engineTypes.Count() == 0)
            {
                context.engineTypes.AddRange
                    (
                        new EngineTypeCategory { Name = "турбовинтовой", NormalizedName = "turboprop" },
                        new EngineTypeCategory { Name = "поршневой", NormalizedName = "reciprocating" },
                        new EngineTypeCategory { Name = "реактивный", NormalizedName = "propfan" },
                        new EngineTypeCategory { Name = "турбовентиляторный", NormalizedName = "turbofan" }
                    );
            }

            context.SaveChanges();

            //если продукты в бд пусты, создаем их
            if (context.airplanes.Count() == 0)
            {
                context.airplanes.AddRange
                    (
                        new Airplane
                        {
                            Name = "Boeing 737-800",
                            Description = "очень хороший самолет 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("turbofan")).First()!,
                            Price = 125,
                            PhotoPath = $"{appUrl}Images/boeing737800.jpeg",
                        },
                        new Airplane
                        {
                            Name = "F-16",
                            Description = "еще один очень хороший самолет 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("propfan")).First()!,
                            Price = 1700,
                            PhotoPath = $"{appUrl}Images/f16.jpeg",
                        },
                        new Airplane
                        {
                            Name = "ИЛ-76",
                            Description = "большй классный грузовичек, много повидал на своем веку 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("reciprocating")).First()!,
                            Price = 25,
                            PhotoPath = $"{appUrl}Images/il76.jpeg",
                        },
                        new Airplane
                        {
                            Name = "Airbus A320",
                            Description = "очень неплохой самолет 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("turbofan")).First()!,
                            Price = 156,
                            PhotoPath = $"{appUrl}Images/airbusa320.jpeg",
                        },
                         new Airplane
                         {
                             Name = "Boeing 747",
                             Description = "икона стиля среди дальнемагистральных самолетов 100/10",
                             Category = context.engineTypes.Where(c => c.NormalizedName.Equals("turbofan")).First()!,
                             Price = 350,
                             PhotoPath = $"{appUrl}Images/boeing747.jpeg",
                         },
                        new Airplane
                        {
                            Name = "Cessna 172",
                            Description = "простая рабочая лошадка, всем советую 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("reciprocating")).First()!,
                            Price = 75,
                            PhotoPath = $"{appUrl}Images/cessna172.jpeg",
                        },
                        new Airplane
                        {
                            Name = "Antonov An-225",
                            Description = "огромная бандура, понятия не имею как она летает 10/10",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("propfan")).First()!,
                            Price = 500,
                            PhotoPath = $"{appUrl}Images/antonov225.jpeg",
                        },
                        new Airplane
                        {
                            Name = "Beechcraft King Air",
                            Description = "идеально подходит для темных дел южноамереканских картелей",
                            Category = context.engineTypes.Where(c => c.NormalizedName.Equals("turboprop")).First()!,
                            Price = 250,
                            PhotoPath = $"{appUrl}Images/kingair.jpeg",
                        }
                    );
            }
            context.SaveChanges();
            

        }
    }
}
