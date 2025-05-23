using BasicCRUD.ViewModels;
using FirstProject.Models.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Controllers
{
    public class BookController : Controller
    {
        private readonly DemoShopContext _dbContext;
        public BookController(DemoShopContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ActionResult> Search(string q = "")
        {
            var bcp = (from b in _dbContext.Books
                       from c in _dbContext.Categories
                       from p in _dbContext.Publishes
                       where (b.BookName.Contains(q))
                             && (b.CategoryId == c.CategoryId)
                             && (b.PublishId == p.PublishId)
                       select new BookCategoryPublisherViewModel
                       {
                           BookId = b.BookId,
                           BookName = b.BookName,
                           CategoryName = c.CategoryName,
                           PublishName = p.PublishName,
                           BookCost = b.BookCost,
                           BookPrice = b.BookPrice
                       }).ToListAsync();

            return View(await bcp);
        }

        public async Task<ActionResult> IndexViewModel()
        {
            var bcp = from b in _dbContext.Books
                      from c in _dbContext.Categories
                      from p in _dbContext.Publishes
                      where (b.CategoryId == c.CategoryId) && (b.PublishId == p.PublishId)
                      select new BookCategoryPublisherViewModel
                      {
                          BookId = b.BookId,
                          BookName = b.BookName,
                          CategoryName = c.CategoryName,
                          PublishName = p.PublishName,
                          BookCost = b.BookCost,
                          BookPrice = b.BookPrice
                      };
            return View(await bcp.ToListAsync());
        }
        // GET: BookController
        public async Task<ActionResult> Index()
        {
            var b = _dbContext.Books.Include(c => c.Category).Include(p => p.Publish);
            return View(await b.ToListAsync());
        }

        // GET: BookController/Details/5
        public async Task<ActionResult> Details(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books
                .Include(c => c.Category)
                .Include(p => p.Publish)
                .FirstOrDefaultAsync(b=>b.BookId==id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_dbContext.Publishes, "PublishId", "PublishName");
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("BookId,BookName,CategoryId,PublishId,Isbn,BookCost,BookPrice")] Book b,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if(!modelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName");
                ViewData["PublishId"] = new SelectList(_dbContext.Publishes, "PublishId", "PublishName");
                return View(b);
            }
            b.BookId = Guid.NewGuid().ToString();
            _dbContext.Books.Add(b);
            await _dbContext.SaveChangesAsync();    
            return RedirectToAction("IndexViewModel");
        }

        // GET: BookController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books.FindAsync(id);
            if(id == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_dbContext.Publishes, "PublishId", "PublishName");
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id,[Bind("BookId,BookName,CategoryId,PublishId,Isbn,BookCost,BookPrice")] Book book,
            Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }
            if (modelState.IsValid)
            {
                try
                {
                    _dbContext.Update(book);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName");
            ViewData["PublishId"] = new SelectList(_dbContext.Publishes, "PublishId", "PublishName");
            return View(book);
        }
        private bool BookExists(string id)
        {
            return _dbContext.Books.Any(b => b.BookId == id);
        }
        // GET: BookController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _dbContext.Books
                .Include(c => c.Category)
                .Include(p => p.Publish)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
