using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaBackEndProject.Areas.Admin.ViewModels;
using Savoy.Areas.Admin.ViewModels.SliderVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service.Interfaces;
using System.Data;

namespace Savoy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SliderController : Controller
	{
		private readonly AppDbContext _context;
		private readonly ISliderService _sliderService;
		private readonly IWebHostEnvironment _env;

		public SliderController(AppDbContext context,
								ISliderService sliderService,
								IWebHostEnvironment env)
		{
			_env = env;
			_context = context;
			_sliderService = sliderService;

		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Slider> sliders = await _sliderService.GetAllAsync();

			List<SliderIndexVM> mappedDatas = new();

			foreach (var slider in sliders)
			{
				SliderIndexVM sliderVM = new()
				{
					Id = slider.Id,
					Title = slider.Title,
					CategoryName = slider.CategoryName,
					Image = slider.Image
				};

				mappedDatas.Add(sliderVM);

			}

			return View(mappedDatas);
		}


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!slider.Photo.CheckFileType("image/"))
                {

                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();

                }


                if (slider.Photo.CheckFileSize(200))
                {

                    ModelState.AddModelError("Photo", "Photo size must be max 200Kb");
                    return View();

                }


                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                await FileHelper.SaveFileAsync(path, slider.Photo);

                
                Slider newSlider = new()
                {
                    Image = fileName,
                    Title = slider.Title,
					CategoryName = slider.CategoryName
                 
                };

                await _context.Sliders.AddAsync(newSlider);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return BadRequest();

            Slider dbSlider = await _sliderService.GetFullDataByIdAsync((int)id);

            if (dbSlider == null) return NotFound();


            SliderEditVM sliderEdit = new SliderEditVM()
            {

                Title = dbSlider.Title,
                CategoryName = dbSlider.CategoryName,
                Image = dbSlider.Image
            };



            return View(sliderEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderEditVM newSlider)
        {
            if (id == null) return BadRequest();

            Slider slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (slider == null) return NotFound();

            if (!ModelState.IsValid)
            {

                return View(newSlider);
            }

            if (newSlider.Photo != null)
            {
                if (!newSlider.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View(newSlider);
                }

                if (newSlider.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Photo size must be max 200Kb");
                    return View(newSlider);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + newSlider.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);


                using (FileStream stream = new(path, FileMode.Create))
                {
                    await newSlider.Photo.CopyToAsync(stream);
                }


                string expath = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", slider.Image);


                FileHelper.DeleteFile(expath);

                slider.Image = fileName;
            }
            else
            {
                slider.Photo = slider.Photo;
            }



            slider.Title = newSlider.Title;
            slider.CategoryName = newSlider.CategoryName;


            _context.Sliders.Update(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await _sliderService.GetFullDataByIdAsync(id);

            _context.Sliders.Remove(slider);

            string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", slider.Image);

            FileHelper.DeleteFile(path);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
		public async Task<IActionResult> Detail(int id)
		{
			if (id == null) return BadRequest();

			Slider slider = await _sliderService.GetFullDataByIdAsync(id);

			if (slider == null) return NotFound();

			SliderDetailVM model = new()
			{
				Title = slider.Title,
				CategoryName = slider.CategoryName,
				Image = slider.Image,

			};

			return View(model);
		}
	}
}
