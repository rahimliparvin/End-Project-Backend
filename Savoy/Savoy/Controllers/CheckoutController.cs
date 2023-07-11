using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;


        public CheckoutController(AppDbContext context,
                                  UserManager<AppUser> userManager,
                                  IProductService productService)
        {
            _context = context;
            _userManager = userManager;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
    
            AppUser user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user is null) return NotFound();
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.AppUser).Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList();
                //Product product = await _productService.GetFullDataByIdAsync(basketItems.FirstOrDefault().ProductId);

                //CheckoutVM model = new()
                //{
                //    BasketItems = basketItems,
                    
                //};

                return View(basketItems);

            }
            else
            {


                //CheckoutVM model = new()
                //{
                //    BasketItems = null,

                //};
                List<BasketItem> basketItems = new();
                return View(basketItems);

            }


        }
    }
    
}
