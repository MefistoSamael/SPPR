using Microsoft.AspNetCore.Mvc;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Extensions;

namespace WEB_153501_BYCHKO.ViewComponents
{
    public class OrderCart : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(GetItems());
        }

        private Tuple<int, int> GetItems()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new();

            int price = cart.Price;
            int count = cart.Count;
            return new Tuple<int, int>(price, count);
        }
    }
}
