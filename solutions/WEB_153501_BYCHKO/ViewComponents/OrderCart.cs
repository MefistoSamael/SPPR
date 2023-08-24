﻿using Microsoft.AspNetCore.Mvc;

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
            return new Tuple<int, int>(127, 7);
        }
    }
}
