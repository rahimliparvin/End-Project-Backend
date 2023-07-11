using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaBackEndProject.Areas.Admin.ViewModels;
using Savoy.Areas.Admin.ViewModels.SliderVM;
using Savoy.Areas.Admin.ViewModels.TeamVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service;
using Savoy.Service.Interfaces;
using System.Data;

namespace Savoy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITeamService _teamService;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context,
                                ITeamService teamService,
                                IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
            _teamService = teamService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Team> teams = await _teamService.GetAllAsync();

            List<TeamIndexVM> mappedDatas = new();

            foreach (var team in teams)
            {
                TeamIndexVM teamVM = new()
                {
                    Id = team.Id,
                    Name = team.Name,
                    Position = team.Position,
                    Image = team.Image
                };

                mappedDatas.Add(teamVM);

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
        public async Task<IActionResult> Create(TeamCreateVM team)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!team.Photo.CheckFileType("image/"))
                {

                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();

                }


                if (team.Photo.CheckFileSize(200))
                {

                    ModelState.AddModelError("Photo", "Photo size must be max 200Kb");
                    return View();

                }


                string fileName = Guid.NewGuid().ToString() + "_" + team.Photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                await FileHelper.SaveFileAsync(path, team.Photo);


                Team newTeam = new()
                {
                    Image = fileName,
                    Name = team.Name,
                    Position = team.Position

                };

                await _context.Teams.AddAsync(newTeam);
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

            Team dbTeam = await _teamService.GetFullDataByIdAsync((int)id);

            if (dbTeam == null) return NotFound();


            TeamEditVM teamEdit = new TeamEditVM()
            {

                Name = dbTeam.Name,
                Position = dbTeam.Position,
                Image = dbTeam.Image
            };



            return View(teamEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamEditVM newTeam)
        {
            if (id == null) return BadRequest();

            Team team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (team == null) return NotFound();

            if (!ModelState.IsValid)
            {

                return View(newTeam);
            }

            if (newTeam.Photo != null)
            {
                if (!newTeam.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View(newTeam);
                }

                if (newTeam.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Photo size must be max 200Kb");
                    return View(newTeam);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + newTeam.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);


                using (FileStream stream = new(path, FileMode.Create))
                {
                    await newTeam.Photo.CopyToAsync(stream);
                }


                string expath = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", team.Image);


                FileHelper.DeleteFile(expath);

                team.Image = fileName;
            }
            else
            {
                team.Photo = team.Photo;
            }



            team.Name = newTeam.Name;
            team.Position = newTeam.Position;


            _context.Teams.Update(team);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Team team = await _teamService.GetFullDataByIdAsync(id);

            _context.Teams.Remove(team);

            string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", team.Image);

            FileHelper.DeleteFile(path);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            Team team = await _teamService.GetFullDataByIdAsync(id);

            if (team == null) return NotFound();

            TeamDetailVM model = new()
            {
                Name = team.Name,
                Position = team.Position,
                Image = team.Image,

            };

            return View(model);
        }

    }
}
