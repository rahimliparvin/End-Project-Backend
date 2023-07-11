using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;

namespace Savoy.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public CartController(AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        public async Task<IActionResult> Index()
        {
            AppUser user = new();

            if (User.Identity.IsAuthenticated)
            {
                user= await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is null) return NotFound();
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.AppUser).Include(x=>x.Product).ThenInclude(x=>x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();
                return View(basketItems);

            }
            else
            {
                List<BasketItem> basketItems = new();
                return View(basketItems);

            }

        }

    }
}
