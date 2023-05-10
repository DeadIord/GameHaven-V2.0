using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAppMain.Data;
using WebAppMain.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace WebAppMain.Controllers
{
    public class VisitorController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VisitorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;

        }

  
        
        public IActionResult ListVisitor(string searchString, string constantFilter)
        {
            var visitors = db.Visitors.Include(v => v.Visiting).AsQueryable();


            if (!string.IsNullOrEmpty(searchString))
            {
                visitors = visitors.Where(v => v.Name.Contains(searchString));
            }


            if (!string.IsNullOrEmpty(constantFilter))
            {
                visitors = visitors.Where(v => v.Constant == constantFilter);
            }

            return View(visitors.ToList());
        }



        public IActionResult AddVisitor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVisitor(Visitors visitor)
        {
            db.Visitors.Add(visitor);
            await db.SaveChangesAsync();
            return RedirectToAction("ListVisitor");
        }


        public async Task<IActionResult> EditVisitor(int? id)
        {
            if (id != null)
            {
                Visitors visitor = await db.Visitors.FirstOrDefaultAsync(p => p.VisitorsId == id);
                if (visitor != null)
                    return View(visitor);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditVisitor(Visitors visitor)
        {
            db.Visitors.Update(visitor);
            await db.SaveChangesAsync();
            return RedirectToAction("ListVisitor");
        }

       

        [HttpGet]
        [ActionName("DeleteVisitor")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Visitors visitor = await db.Visitors.FirstOrDefaultAsync(p => p.VisitorsId == id);
                if (visitor != null)
                    return View(visitor);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Visitors services = new Visitors { VisitorsId = id.Value };
                db.Entry(services).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ListVisitor");
            }
            return NotFound();
        }
    }
}
