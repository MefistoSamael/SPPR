using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Admin
{
    public class CreateModel : PageModel
    {
        private readonly API.Services.ProductService.IProductService _context;

        public CreateModel(API.Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Airplane Airplane { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Airplane == null)
            {
                return Page();
            }

            await _context.CreateProductAsync(Airplane);

            return RedirectToPage("./Index");
        }
    }
}
