using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppMain.Data;
using WebAppMain.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace WebAppMain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            db = context;
        }

        public async Task<IActionResult> Index(string userId, string code)
        {

            var lastVisiting = await db.Visiting
              .Where(v => v.ApplicationUserId == userId)
              .OrderByDescending(v => v.DateAndTimeOfTheVisit)
              .FirstOrDefaultAsync();

            if (lastVisiting != null)
            {
                ViewBag.TotalCost = lastVisiting.TotalCost;
            }

            if (userId == null || code == null)
            {
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View();
        
        }

       
        public async Task<IActionResult> AddVisitingGuestAsync()
        {

            await selectOptions();

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> AddVisitingGuestAsync(Visiting visiting)
        {

            try
            {
                visiting.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                visiting.DateAndTimeOfTheVisitEnd = visiting.DateAndTimeOfTheVisit.AddHours(visiting.NumberOfHours);
                visiting.Status = "Не подтвержден";

                var isDuplicate = await db.Visiting.AnyAsync(v => v.HallsId == visiting.HallsId
                && v.ComputerId == visiting.ComputerId
                && v.Status == "Подтвержден"
                && v.DateAndTimeOfTheVisit <= visiting.DateAndTimeOfTheVisitEnd
                && v.DateAndTimeOfTheVisitEnd >= visiting.DateAndTimeOfTheVisit);

                if (!isDuplicate)
                {
                    ModelState.AddModelError("", "На это время уже есть запись");
                    await selectOptions();
                    return View(visiting);
                }
                else
                {
                    db.Visiting.Add(visiting);
                    await db.SaveChangesAsync();

                    var service = await db.Services.FindAsync(visiting.ServicecId);
                    double totalCost = service.PricePerService * visiting.NumberOfHours;
                    TempData["TotalCost"] = totalCost.ToString();

                    TempData["SuccessMessage"] = "Бронирование успешно создано";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                await selectOptions();

                ModelState.AddModelError("", "Этот компьютер уже занят, выберите другое время");

                return View(visiting);
            }
        }

        public record SelectOptions
        {
            public int value { get; set; }
            public string text { get; set; }


        }
        public record SelectOption
        {
            public string value { get; set; }
            public string text { get; set; }


        }
        public IActionResult Privacy()
        {
            return View();
        }
        private async Task selectOptions()
        {
            var services = await db.Services
            .Select(s => new SelectOptions
            {
                value = s.ServicesId,
                text = s.NameOfTheService,
            })
            .ToListAsync();
            ViewData["Services"] = services;
            var halls = await db.Halls
            .Select(s => new SelectOptions
            {
                value = s.HallsId,
                text = s.HallsName,
            })
            .ToListAsync();
            ViewData["Halls"] = halls;

            var employees = await db.Users
                .Select(s => new SelectOption
                {
                    value = s.Id,
                    text = s.UserName,
                })
                .ToListAsync();
            ViewData["Employees"] = employees;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
