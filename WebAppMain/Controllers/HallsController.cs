using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAppMain.Data;
using WebAppMain.Models;


namespace WebAppMain.Controllers
{
    public class HallsController : Controller
    {
        private ApplicationDbContext db;
        public HallsController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<ActionResult> ListHallsAsync()
        {
            var halls = await db.Halls
             .Include(c => c.Computers)
             .ToListAsync();
            return View(halls);
            //var computers = db.Computers
            //    .Where(c => c.ComputerName =="Компьютер")

            //    .ToList();
            //var halls = db.Halls.ToList();
            //var model = new ComputersAndHallsViewModel
            //{
            //    Computers = computers,
            //    Halls = halls
            //};
            //return View(model);
        }

        public IActionResult AddHalls()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddHalls(Halls halls)
        {
            if (ModelState.IsValid)
            {
                db.Halls.Add(halls);
                await db.SaveChangesAsync();
                return RedirectToAction("ListHalls");
            }
            else
            {
                return View(halls);
            }
        }


        public async Task<IActionResult> EditHalls(int? id)
        {
            if (id != null)
            {
                Halls halls = await db.Halls.FirstOrDefaultAsync(p => p.HallsId == id);
                if (halls != null)
                    return View(halls);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditHalls(Halls halls)
        {
            if (ModelState.IsValid)
            {
                db.Halls.Update(halls);
                await db.SaveChangesAsync();
                return RedirectToAction("ListHalls");
            }
            else
            {
                return View(halls);
            }
            
        } 
    

        

        [HttpGet]
        [ActionName("DeleteHalls")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Halls halls = await db.Halls.FirstOrDefaultAsync(p => p.HallsId == id);
                if (halls != null)
                    return View(halls);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Halls computers = new Halls { HallsId = id.Value };
                db.Entry(computers).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ListHalls");
            }
            return NotFound();
        }


    }
}
