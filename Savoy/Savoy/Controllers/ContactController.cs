using Microsoft.AspNetCore.Mvc;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IContactService _contactService;

        public ContactController(AppDbContext context,
                                 IContactService contactService)
        {
            _context = context;
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Contact> contacts = await _contactService.GetAllAsync();

            ContactVM model = new()
            {
                Contacts = contacts
            };

            return View(model);
        }
    }
}
