using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.Domain.Models
{
    public class CartItem
    {
        public int Count { get; set; }

        public Airplane Airplane { get; set; }
    }
}
