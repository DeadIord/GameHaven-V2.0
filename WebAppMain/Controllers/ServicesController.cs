using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using System.Linq;
using System.Threading.Tasks;
using WebAppMain.Models;
using WebAppMain.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAppMain.Controllers
{
    public class ServicesController : Controller
    {

       
            private ApplicationDbContext db;
            public ServicesController(ApplicationDbContext context)
            {
                db = context;
            }
        public async Task<IActionResult> ListServices()
        {
            return View(await db.Services.ToListAsync());
        }

        public IActionResult AddServices()
        {
            return View();
        }
      


        public async Task<IActionResult> EditServices(int? id)
        {
            if (id != null)
            {
                Services services = await db.Services.FirstOrDefaultAsync(p => p.ServicesId == id);
                if (services != null)
                    return View(services);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddServices(Services services)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(services);
            await db.SaveChangesAsync();
            return RedirectToAction("ListServices");
            }
            else
            {
                return View(services);
            }
        }
    
        [HttpPost]
        public async Task<IActionResult> EditServices(Services services)
        {
            if (ModelState.IsValid)
            {
                db.Services.Update(services);
                await db.SaveChangesAsync();
                return RedirectToAction("ListServices");
            }
            else
            {
                return View(services);
            }
              
        }



        [HttpGet]
        [ActionName("DeleteServices")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Services services = await db.Services.FirstOrDefaultAsync(p => p.ServicesId == id);
                if (services != null)
                    return View(services);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Services services = await db.Services
                    .Include(h => h.Visitings)
                    .FirstOrDefaultAsync(p => p.ServicesId == id);

                if (services != null)
                {
                    if (services.Visitings.Count == 0)
                    {
                        db.Entry(services).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
                        return RedirectToAction("ListServices");
                    }
                }
            }

            return RedirectToAction("ListServices");
        }

       
    }
    }
    

