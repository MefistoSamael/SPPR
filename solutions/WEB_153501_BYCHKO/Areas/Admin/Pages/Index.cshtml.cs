using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;

        public IndexModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public ListModel<Airplane> Airplane { get;set; } = default!;

        public async Task OnGetAsync(int pageno)
        {
            var answer = (await _context.GetProductListAsync(null, pageno));
            if (answer.Success)
            {
                Airplane = answer.Data;
            }
        }
    }
}
