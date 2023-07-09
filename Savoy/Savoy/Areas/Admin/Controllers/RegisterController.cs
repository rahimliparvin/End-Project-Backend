using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Areas.Admin.ViewModels.LoginVM;
using Savoy.Areas.Admin.ViewModels.RegisterVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly IRegisterService _registerService;

        public RegisterController(AppDbContext context,
                                IRegisterService registerService,
                                IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
            _registerService = registerService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Register register = await _registerService.GetAsync();

            RegisterIndexVM model = new()
            {
                Image = register.BackgroundImage,
                Id = register.Id
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return BadRequest();

            Register dbRegister = await _registerService.GetFullDataByIdAsync((int)id);

            if (dbRegister == null) return NotFound();


            RegisterEditVM registerEdit = new RegisterEditVM()
            {
                Image = dbRegister.BackgroundImage
            };



            return View(registerEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegisterEditVM newRegister)
        {
            if (id == null) return BadRequest();

            Register register = await _context.Registers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (register == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(newRegister);
            }

            if (!newRegister.Photo.CheckFileType("image"))
            {
                if (!newRegister.Photo.CheckFileType("webp"))
                {
                    if (!newRegister.Photo.CheckFileType("jpg"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(newRegister);
                    }
                }


            }

            if (newRegister.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Photo size must be max 1000Kb");
                return View(newRegister);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + newRegister.Photo.FileName;


            string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await newRegister.Photo.CopyToAsync(stream);
            }


            string expath = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", register.BackgroundImage);


            FileHelper.DeleteFile(expath);

            register.BackgroundImage = fileName;


            _context.Registers.Update(register);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

