using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppMain.Data;
using WebAppMain.Models;


namespace WebAppMain.Controllers
{
    public class VisitingController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public VisitingController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            db = applicationDbContext;
        }
        public IActionResult ListVisiting(string searchString, int? page, string currentFilter, string constantFilter)
        {
            var visitors = db.Visitors.ToList();
            var visiting = db.Visiting.ToList();
            var halss = db.Halls.ToList();
            var comps = db.Computers.ToList();
            var users = _userManager.Users.ToList();
            var services = db.Services.ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                visiting = visiting.Where(u => !string.IsNullOrEmpty(u.DateAndTimeOfTheVisit.Date.ToString("yyyy-MM-dd")) &&
                    u.DateAndTimeOfTheVisit.Date.ToString("yyyy-MM-dd").Contains(searchString)).ToList();

            }

            if (!string.IsNullOrEmpty(constantFilter))
            {

                visiting = visiting.Where(u => u.Status != null && u.Status != null && u.Status.Contains(constantFilter)).ToList();
            }

            visiting = visiting.OrderByDescending(u => u.DateAndTimeOfTheVisitEnd).ToList();
            //устанавливаем размер страницы и получаем количество страниц
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            int totalUsers = visiting.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalUsers / pageSize);

            var model = new VisitingListVM
            {
                Visiting = visiting.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                constantFilter = searchString,
                Halls = halss,
                ApplicationUser = users,
                Computer = comps,
                Services = services,
                Visitors = visitors
            };

            return View(model);
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
        public async Task<IActionResult> AddVisitingAsync()
        {

            await selectOptions();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVisiting(Visiting visiting)
        {

            try
            {
                visiting.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                visiting.DateAndTimeOfTheVisitEnd = visiting.DateAndTimeOfTheVisit.AddHours(visiting.NumberOfHours);
                visiting.Status = "Подтвержден";
                
                var isDuplicate = await db.Visiting.AnyAsync(v => v.HallsId == visiting.HallsId
                        && v.ComputerId == visiting.ComputerId
                        && v.Status == "Подтвержден"
                        && v.DateAndTimeOfTheVisit <= visiting.DateAndTimeOfTheVisitEnd
                        && v.DateAndTimeOfTheVisitEnd >= visiting.DateAndTimeOfTheVisit);

                if (isDuplicate)
                {
                    ModelState.AddModelError("", "Этот компьютер уже занят, выберите другое время.");
                    await selectOptions();
                    

                    return View(visiting);
                }
                else
                {
                    db.Visiting.Add(visiting);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListVisiting");
                }
            }
            catch (Exception)
            {
                await selectOptions();

                ModelState.AddModelError("", "Этот компьютер уже занят, выберите другое время");

                return View(visiting);
            }
        }
        public async Task<IActionResult> EditVisiting(int? id)
        {
            await selectOptions();

            if (id != null)
            {
                Visiting visiting = await db.Visiting.FirstOrDefaultAsync(p => p.VisitingId == id);
                if (visiting != null)
                    return View(visiting);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditVisiting(Visiting visiting)
        {
            try
            {
                visiting.Status = "Подтвержден";
                visiting.DateAndTimeOfTheVisitEnd = visiting.DateAndTimeOfTheVisit.AddHours(visiting.NumberOfHours);
                var isDuplicate = await db.Visiting.AnyAsync(v => v.HallsId == visiting.HallsId
                      && v.ComputerId == visiting.ComputerId
                      && v.Status == "Подтвержден"
                      && v.DateAndTimeOfTheVisit <= visiting.DateAndTimeOfTheVisitEnd
                      && v.DateAndTimeOfTheVisitEnd >= visiting.DateAndTimeOfTheVisit);

                if (isDuplicate)
                {
                    ModelState.AddModelError("", "Вы не можете сохранить изменения, время записи пересекается с другой, выберите другое время.");
                    await selectOptions();
                    return View(visiting);
                }
                else
                {
                    db.Visiting.Update(visiting);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListVisiting");
                }
               

            }
            catch (Exception)
            {
                await selectOptions();

                ModelState.AddModelError("", "Этот компьютер уже занят, выберите другое время");

                return View(visiting);
            }



        }
        [HttpGet]
        public IActionResult GetExpiredVisits()
        {
            var currentTime = DateTime.Now;
            var expiredVisits = db.Visiting.Where(v => v.DateAndTimeOfTheVisitEnd < currentTime && v.DateAndTimeOfTheVisitEnd > currentTime.AddMinutes(-2)).ToList();
            return Json(expiredVisits);
        }
        [HttpGet]

        public async Task<JsonResult> GetComputerStatus(int hallId, DateTime selectedDateTime)
        {
            var computers = await db.Computers
                .Where(c => c.HallsId == hallId)
                .Select(c => new
                {
                    computerName = c.ComputerName,
                    isBusy = db.Visiting.Any(v => v.ComputerId == c.ComputerId
                        && v.Status == "Подтвержден"
                        && v.DateAndTimeOfTheVisit <= selectedDateTime
                        && v.DateAndTimeOfTheVisitEnd >= selectedDateTime)
                })
                .ToListAsync();

            return Json(computers);
        }
        [HttpGet]
        public JsonResult GetComputersByHallId(int hallId)
        {
            var computers = db.Computers
                .Where(c => c.HallsId == hallId)
                .Select(c => new SelectOptions
                {
                    value = c.ComputerId,
                    text = c.ComputerName,
                })
                .ToList();
            return Json(computers);
        }

        [HttpGet]
        [ActionName("DeleteVisiting")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Visiting visiting = await db.Visiting.FirstOrDefaultAsync(p => p.VisitingId == id);
                if (visiting != null)
                    return View(visiting);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Visiting visiting = new Visiting { VisitingId = id.Value };
                db.Entry(visiting).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ListVisiting");
            }
            return NotFound();
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

            var roles = await _userManager.GetUsersInRoleAsync("Сотрудник");
            var employees = roles
               .Select(s => new SelectOption
                {
                    value = s.Id,
                    text = s.UserName,
                })
                .ToList();
            ViewData["Employees"] = employees;
           

        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int orderId, string newStatus)
        {
            var order = await db.Visiting.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
           

            // если новый статус не "Подтвержден", то проверка дублей не требуется
            if (newStatus == "Подтвержден")
            {
                var isDuplicate = await db.Visiting.AnyAsync(v => v.HallsId == order.HallsId
                             && v.ComputerId == order.ComputerId
                             && v.Status == "Подтвержден"
                             && v.DateAndTimeOfTheVisit <= order.DateAndTimeOfTheVisitEnd
                             && v.DateAndTimeOfTheVisitEnd >= order.DateAndTimeOfTheVisit);
                if (isDuplicate)
                {
                    TempData["ErrorMessage"] = "Вы не можете обновить статус, время записи пересекается с другой, выберите другое время.";
                    await selectOptions();
                    return RedirectToAction("ListVisiting");
                }
            }

            // сохраняем новый статус в заказе и в базе данных (если проверка дублей не обнаружила пересечений)
            order.Status = newStatus;
            await db.SaveChangesAsync();

            return RedirectToAction("ListVisiting");

        }
    }
}
