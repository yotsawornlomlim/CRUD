using FirstProject.Models.db;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DemoShopContext _dbContext;
        public CategoryController(DemoShopContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            //IEnumerable<Category> allCate = _dbContext.Categories;

            var allCate = from cate in _dbContext.Categories
                          where cate.CategoryId > 1
                          select cate;

            if (allCate == null)
            {
                return NotFound();
            }

            return View(allCate);
        }
        public IActionResult Create(Category category)
        {//Validate
            if (ModelState.IsValid)//correct
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            //incorrect
            return View();
        }
        public IActionResult Update()
        {
            IEnumerable<Category> allCate = _dbContext.Categories;
            return View(allCate);
        }
        public IActionResult ShowUpd(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            var obj = _dbContext.Categories.Find(id);
            return View(obj);
        }
        public IActionResult UpdDB(Category cat) 
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(cat);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult DelDB(int? id) 
        {
            if (id == 0 || id is null)
            {
                return NotFound();
            }
            var obj = _dbContext.Categories.Find(id);
            if(obj is null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
