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
    public class IndexModel : PageModel
    {
        private readonly API.Services.ProductService.IProductService _context;

        public IndexModel(API.Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public IList<Airplane> Airplane { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var answer = (await _context.GetProductListAsync(null));
            if (answer.Success)
            {
                Airplane = answer.Data.Items;
            }
        }
    }
}
