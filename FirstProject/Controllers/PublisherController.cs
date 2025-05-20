using System.Security.Permissions;
using FirstProject.Models.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Controllers
{
    public class PublisherController : Controller
    {
        private readonly DemoShopContext _dbContext;
        public PublisherController(DemoShopContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: PublisherController

        public async Task<ActionResult> index()
        {
            var pub = from p in _dbContext.Publishes select p;
            return View(await pub.ToListAsync());
        }

        // GET: PublisherController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var pub = await _dbContext.Publishes.FirstOrDefaultAsync(c => c.PublishId == id);
            if(pub == null)
            {
                return NotFound();
            }
            return View(pub);
        }

        // GET: PublisherController/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Publish pub)
        {
            if (ModelState.IsValid)
            {
                try {
                    _dbContext.Publishes.Add(pub);
                    await _dbContext.SaveChangesAsync();    
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(pub);
        }
        // POST: PublisherController/Create
        

        // GET: PublisherController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var pub = await _dbContext.Publishes.FindAsync(id);
            if (pub == null)
            {
                return NotFound();
            }
            return View(pub);
        }

        // POST: PublisherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,Publish pub)
        {
            if(id!= pub.PublishId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Publishes.Update(pub);
                    await _dbContext.SaveChangesAsync();    
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!PublisherExits(pub.PublishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pub);
        }
        private bool PublisherExits(int id)
        {
            return _dbContext.Publishes.Any(p => p.PublishId == id);
        }

        // GET: PublisherController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id==0)
            {
                return NotFound();
            }
            var pub = await _dbContext.Publishes.FindAsync(id);
            if (pub == null)
            {
                return NotFound();
            }
            return View(pub);
        }

        // POST: PublisherController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var pub = await (_dbContext.Publishes.FindAsync(id));
            _dbContext.Publishes.Remove(pub);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
