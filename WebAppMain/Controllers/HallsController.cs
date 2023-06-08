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
            
                Halls halls = await db.Halls
                    .Include(h => h.Visitings)
                    .FirstOrDefaultAsync(p => p.HallsId == id);

                if (halls != null)
                {
                    if (halls.Visitings.Count == 0)
                    {
                        db.Entry(halls).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
                        return RedirectToAction("ListHalls");
                    }
                }
            

            return RedirectToAction("ListHalls");
        }

    }
}
