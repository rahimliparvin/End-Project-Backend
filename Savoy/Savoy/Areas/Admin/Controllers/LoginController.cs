using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Areas.Admin.ViewModels.LoginVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly ILoginService _loginService;

        public LoginController(AppDbContext context,
                                ILoginService loginService,
                                IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
            _loginService = loginService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Login login = await _loginService.GetAsync();

            LoginIndexVM model = new()
            {
                Image = login.BackgroundImage,
                Id = login.Id
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return BadRequest();

            Login dbLogin = await _loginService.GetFullDataByIdAsync((int)id);

            if (dbLogin == null) return NotFound();


            LoginEditVM loginEdit = new LoginEditVM()
            {
                Image = dbLogin.BackgroundImage
            };



            return View(loginEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoginEditVM newLogin)
        {
            if (id == null) return BadRequest();

            Login login = await _context.Logins.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (login == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(newLogin);
            }

            if (!newLogin.Photo.CheckFileType("image"))
            {
                if (!newLogin.Photo.CheckFileType("webp"))
                {
                    if (!newLogin.Photo.CheckFileType("jpg"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(newLogin);
                    }
                }

               
            }

            if (newLogin.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Photo size must be max 1000Kb");
                return View(newLogin);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newLogin.Photo.FileName;


            string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await newLogin.Photo.CopyToAsync(stream);
            }


            string expath = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", login.BackgroundImage);


            FileHelper.DeleteFile(expath);

            login.BackgroundImage = fileName;


            _context.Logins.Update(login);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

