using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAppMain.Data;
using WebAppMain.Models;

namespace WebAppMain.Controllers
{
    public class ComputerController : Controller
    {
        private ApplicationDbContext db;
        public ComputerController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> ListComputer()
        {
            var computer = db.Computers.Include(p => p.Halls);
            return View(await computer.ToListAsync());
        }
      

        public IActionResult AddComputer()
        {
            selectOptions();
            return View();

        }
        public record SelectOptions
        {
            public int value { get; set; }
            public string text { get; set; }
        }

        public async Task<IActionResult> EditComputer(int? id)
        {
           

            if (id != null)
            {
                Computer computers = await db.Computers.FirstOrDefaultAsync(p => p.ComputerId == id);
                selectOptions();

                if (computers != null)
                    return View(computers);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddComputer(Computer computers)
        {
            if (ModelState.IsValid)
            {
                db.Computers.Add(computers);
                await db.SaveChangesAsync();
                return RedirectToAction("ListComputer");
            }
            else
            {
                selectOptions();
                return View(computers);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditComputer(Computer computers)
        {
            if (ModelState.IsValid)
            {
                db.Computers.Update(computers);
                await db.SaveChangesAsync();
                return RedirectToAction("ListComputer");
            }
            else
            {
                selectOptions();
                return View(computers);
            }
           
        }



        [HttpGet]
        [ActionName("DeleteComputer")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Computer computers = await db.Computers.FirstOrDefaultAsync(p => p.ComputerId == id);
                if (computers != null)
                    return View(computers);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Computer computers = new() { ComputerId = id.Value };
                db.Entry(computers).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ListComputer");
            }
            return NotFound();
        }

        public void selectOptions()
        {
            var halls = db.Halls
            .Select(s => new SelectOptions
            {
                value = s.HallsId,
                text = s.HallsName
            })
            .ToList();
            TempData["halls"] = halls;
        }
    }
}
