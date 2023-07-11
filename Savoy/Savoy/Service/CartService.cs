using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCartProductBasket(string userId)
        {
            IEnumerable<BasketItem> basketItems = await _context.BasketItems.Where(m => m.AppUserId == userId).ToListAsync();

            return basketItems.Count();
        }

    }
}
