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

namespace WEB_153501_BYCHKO.Admin
{
    public class EditModel : PageModel
    {
        private readonly API.Services.ProductService.IProductService _context;

        public EditModel(API.Services.ProductService.IProductService context)
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

            var airplane =  await _context.GetProductByIdAsync(id ?? default(int));
            if (airplane.Success == false)
            {
                return NotFound();
            }

            Airplane = airplane.Data;
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateProductAsync(Airplane.Id, Airplane);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AirplaneExists(Airplane.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

        //private bool AirplaneExists(int id)
        //{
        //  return (_context.airplanes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
