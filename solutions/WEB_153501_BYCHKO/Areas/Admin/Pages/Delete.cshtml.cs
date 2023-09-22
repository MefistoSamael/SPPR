using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;
        public DeleteModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        [BindProperty]
      public Airplane Airplane { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _context.GetProductByIdAsync(id ?? default(int));

            if (airplane == null)
            {
                return NotFound();
            }
            else 
            {
                Airplane = airplane.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _context.DeleteProductAsync(id ?? default(int));

            return RedirectToPage("./Index");
        }
    }
}
