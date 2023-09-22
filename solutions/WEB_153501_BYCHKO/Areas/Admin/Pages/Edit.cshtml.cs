using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;
        private readonly Services.EngineTypeCategoryService.ICategoryService _service;


        [BindProperty]
        public IFormFile? Image { get; set; }

        public EditModel(Services.ProductService.IProductService context,
                         Services.EngineTypeCategoryService.ICategoryService service)
        {
            _context = context;
            _service = service;
        }

        [BindProperty]
        public Airplane Airplane { get; set; } = default!;

        [BindProperty]
        public int CategoryId { get; set; }

        public SelectList selectList { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane =  await _context.GetProductByIdAsync(id ?? default(int));
            if (airplane.Success == false)
            {
                return NotFound();
            }

            Airplane = airplane.Data;
            selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                            nameof(EngineTypeCategory.Id), nameof(EngineTypeCategory.Name), Airplane.Category!.Name);


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Airplane.Category = (await _service.GetCategoryListAsync()).Data.Where(c => c.Id == CategoryId).FirstOrDefault();

            ModelState.ClearValidationState(nameof(Airplane));
            if (!TryValidateModel(Airplane, nameof(Airplane)))
            {
                selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                                        nameof(EngineTypeCategory.Id), nameof(EngineTypeCategory.Name));
                return Page();
            }

            await _context.UpdateProductAsync(Airplane.Id, Airplane, Image);

            return RedirectToPage("./Index");
        }
    }
}
