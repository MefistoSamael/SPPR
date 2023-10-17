using Microsoft.AspNetCore.Mvc;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Extensions;
using WEB_153501_BYCHKO.Services.EngineTypeCategoryService;
using WEB_153501_BYCHKO.Services.ProductService;

namespace WEB_153501_BYCHKO.Controllers
{
    public class AirplaneProductController : Controller
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;


        public AirplaneProductController(IProductService productService, ICategoryService categoryService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageno)
        {
            // получаем список самолетов
            var productResponse = 
           await _productService.GetProductListAsync(category, pageno);

            // обработка неуспешного обращения
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            // получаем все категории
            
            var categories = _categoryService.GetCategoryListAsync().Result.Data;

            // currentCategory вынесено в отдельную переменную, чтобы проверить на null и, в случае null, передать 
            // в представление строку "Все"
            var currentCategory = categories.Find(c => c.NormalizedName == category);

            // чтобы отображать категорию "все", на странице с типами
            ViewData["currentCategory"] = currentCategory == null ? "Все" : currentCategory.Name;
            ViewBag.categories = categories;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FurnituriesPartial", productResponse.Data);
            }

            return View(productResponse.Data);
        }
    }
}
