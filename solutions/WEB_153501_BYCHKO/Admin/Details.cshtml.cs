using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly API.Services.ProductService.IProductService _context;

        public DetailsModel(API.Services.ProductService.IProductService context)
        {
            _context = context;
        }

      public Airplane Airplane { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _context.GetProductByIdAsync(id ?? default(int));
            if (airplane.Success == false)
            {
                return NotFound();
            }
            else 
            {
                Airplane = airplane.Data;
            }
            return Page();
        }
    }
}
