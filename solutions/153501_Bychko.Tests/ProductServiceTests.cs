using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;
using WEB_153501_BYCHKO.Controllers;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Services.ProductService;
using System.Xml.Serialization;
using Humanizer;
using Microsoft.AspNetCore.Http;
using WEB_153501_BYCHKO.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WEB_153501_BYCHKO.API.Data;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;
using WEB_153501_BYCHKO.API.Services.ProductService;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using Microsoft.Extensions.Options;
using WEB_153501_BYCHKO.API.Models;

namespace _153501_Bychko.Tests
{
    public class ProductServiceTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly Mock<IWebHostEnvironment> env;
        private readonly Mock<IHttpContextAccessor> accessor;

        private int maxPageSize = 20;

        public ProductServiceTests()
        {
            env = new();
            accessor = new();

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureCreated();

            context.engineTypes.AddRange(
                new EngineTypeCategory
                {
                    Id = 1,
                    Name = "category1",
                    NormalizedName = "cat1"
                },
                new EngineTypeCategory
                {
                    Id = 2,
                    Name = "category2",
                    NormalizedName = "cat2"
                });
            context.airplanes.AddRange(
                new Airplane
                {
                    Id = 1,
                    Name = "plane1",
                    Description = "d1",
                    PhotoPath = "path1",
                    Price = 1,
                    MIMEType = "type",
                    Category = context.engineTypes.Find(1)
                },
                new Airplane
                {
                    Id = 2,
                    Name = "plane2",
                    Description = "d2",
                    PhotoPath = "path2",
                    Price = 2,
                    MIMEType = "type",
                    Category = context.engineTypes.Find(2)
                },
                new Airplane
                {
                    Id = 3,
                    Name = "plane3",
                    Description = "d3",
                    PhotoPath = "path3",
                    Price = 3,
                    MIMEType = "type",
                    Category = context.engineTypes.Find(2)
                },
                new Airplane
                {
                    Id = 4,
                    Name = "plane4",
                    Description = "d4",
                    PhotoPath = "path4",
                    Price = 4,
                    MIMEType = "type",
                    Category = context.engineTypes.Find(1)
                }
                );
            context.SaveChanges();

        }

        public void Dispose() => _connection.Dispose();

        AppDbContext GetContext() => new AppDbContext(_contextOptions);

        [Fact]
        public void GetProductListAsync_Default_ThreeObject_CorrectPageCountCalculation()
        {
            //Mock<IOptions<ConfigData>> opt = new();
            //opt.Setup(o => o.Value).Returns(new ConfigData { MaxPageSize=20});
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object);

            var result = productService.GetProductListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Airplane>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.airplanes.First(), result.Data.Items[0]);
        }

        [Theory]
        [InlineData(2)]
        public void GetProductListAsync_ReturnsRightPage(int pageno)
        {
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object);

            var result = productService.GetProductListAsync(null, pageno).Result;
            Assert.IsType<ResponseData<ListModel<Airplane>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
        }

        [Theory]
        [InlineData("cat1")]
        public void GetProductListAsync_ReturnsRightElements(string category)
        {
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object);

            List<Airplane> requiredResult = new() { context.airplanes.Find(1), context.airplanes.Find(4) };

            var result = productService.GetProductListAsync(category).Result;
            Assert.IsType<ResponseData<ListModel<Airplane>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.Equal(1, result.Data.TotalPages);
            Assert.Equal(requiredResult, result.Data.Items);
        }

        [Fact]
        public void GetProductListAsync_CorrectMaxPageSize()
        {
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object);


            var result = productService.GetProductListAsync(null, 1, pageSize: 21).Result;
            Assert.IsType<ResponseData<ListModel<Airplane>>>(result);
            Assert.True(result.Success);

            Assert.True(result.Data.Items.Count <= maxPageSize);
        }

        [Fact]
        public void GetProductListAsync_Pageno_GT_PageCount_ReturnsFalse()
        {
            var context = GetContext();
            var productService = new ProductService(context, env.Object, accessor.Object);

            var result = productService.GetProductListAsync(null, 3).Result;
            Assert.IsType<ResponseData<ListModel<Airplane>>>(result);
            Assert.True(!result.Success);
        }

    }
}
