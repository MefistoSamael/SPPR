using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;
        private readonly Services.EngineTypeCategoryService.ICategoryService _service;


        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public  SelectList selectList { get; set; }

        [BindProperty]
        public Airplane Airplane { get; set; } = default!;
        
        public CreateModel(Services.ProductService.IProductService context,
                            Services.EngineTypeCategoryService.ICategoryService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<IActionResult> OnGet()
        {
            selectList = new SelectList((await _service.GetCategoryListAsync()).Data, 
                                        nameof(EngineTypeCategory.Id), nameof(EngineTypeCategory.Name));
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Airplane.Category = (await _service.GetCategoryListAsync()).Data.Where(c => c.Id == CategoryId).FirstOrDefault();

            ModelState.ClearValidationState(nameof(Airplane));
            if (!TryValidateModel(Airplane, nameof(Airplane)) || Airplane == null || Image == null)
            {
                selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                                        nameof(EngineTypeCategory.Id), nameof(EngineTypeCategory.Name));
                return Page();
            }

            var response = await _context.CreateProductAsync(Airplane, Image);
            if (!response.Success)
                throw new Exception(response.ErrorMessage);

            return RedirectToPage("./Index");
        }
    }
}
