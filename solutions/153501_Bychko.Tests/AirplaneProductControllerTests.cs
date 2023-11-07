using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_BYCHKO.Controllers;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;
using WEB_153501_BYCHKO.Services.ProductService;

namespace _153501_Bychko.Tests
{
    public class AirplaneProductControllerTests
    {
        Mock<IProductService> productService;
        Mock<ICategoryService> categoryService;
        AirplaneProductController controller;

        public AirplaneProductControllerTests()
        {
            productService = new();
            categoryService = new();
            controller = new AirplaneProductController(productService.Object, categoryService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void FailedGetCategoryListReturns404()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = false, ErrorMessage = "err" }));

            //act
            var result = controller.Index("a", 1).Result;

            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void FailedGetProductListReturns404()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<EngineTypeCategory>> { Success = false }));

            //act
            var result = controller.Index("a", 1).Result;

            //assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ViewDataGotCategotyList()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync("a", 1)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<EngineTypeCategory>> { Success = true, Data = new List<EngineTypeCategory>() }));

            //act
            controller.Index("a", 1);

            var result = controller.ViewBag.categories;

            Assert.IsType<List<EngineTypeCategory>>(result);
        }

        [Theory]
        [InlineData(null, 1)]
        public void ViewDataGotAllCategory(string? category, int pageno)
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(category, pageno)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<EngineTypeCategory>> { Success = true, Data = new List<EngineTypeCategory>() }));

            //act
            controller.Index(category, pageno);
            var result = controller.ViewData["currentCategory"];

            //assert
            Assert.Equal(result, "Все");
        }

        [Theory]
        [InlineData("a", 1)]
        public void ViewDataGotCurrentCategory(string? category, int pageno)
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(category, pageno)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = true }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<EngineTypeCategory>> { Success = true, Data = new List<EngineTypeCategory> { new EngineTypeCategory { Id = 1, Name = "a", NormalizedName = "a" } } }));

            //act
            controller.Index(category, pageno);
            var result = controller.ViewData["currentCategory"];

            //assert
            Assert.Equal(result, category);

        }

        [Fact]
        public void IndexReturnedObjectList()
        {
            //arrange
            productService.Setup(m => m.GetProductListAsync(null, 1)).Returns(Task.FromResult(new ResponseData<ListModel<Airplane>> { Success = true, Data = new ListModel<Airplane>() }));
            categoryService.Setup(m => m.GetCategoryListAsync()).Returns(Task.FromResult(new ResponseData<List<EngineTypeCategory>> { Success = true, Data = new List<EngineTypeCategory> { new EngineTypeCategory() } }));

            Mock<HttpRequest> request = new Mock<HttpRequest>();
            request.Setup(r => r.Headers["X-Requested-With"]).Returns("XMLHttpRequest");

            AirplaneProductController controller = new AirplaneProductController(productService.Object, categoryService.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };


            //act
            var result = controller.Index(null, 1).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ListModel<Airplane>>(viewResult.Model);

        }

    }
}
