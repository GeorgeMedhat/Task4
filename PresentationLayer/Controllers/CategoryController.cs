using BusinessLogic.Models;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDBContext dbContext;

        public CategoryController(AppDBContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            var categories = dbContext.Categories.Where(c=>c.markedAsDeleted==false);

            return View(categories);
        }


        public IActionResult GetAllCategories()
        {
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }

        public IActionResult AddCategory()
        {

            Category cat = new Category();
            cat.catName = "Category1";
            cat.catOrder = 1;
            cat.markedAsDeleted = false;

            dbContext.Categories.Add(cat);

            dbContext.SaveChanges();

            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(string catName,int catOrder)
        {
            Category cat = new Category();
            cat.catName = catName;
            cat.catOrder = catOrder;
            cat.markedAsDeleted = false;
            dbContext.Categories.Add(cat);

            dbContext.SaveChanges();

            return View("Index",dbContext.Categories.Where(c=>c.markedAsDeleted==false));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cat = dbContext.Categories.ToList().FirstOrDefault(c => c.id == id);

            if (cat == null)
                return NotFound();
            else
                return View(cat);
        }

        [HttpPost]
        public IActionResult Edit(Category updatedCat)
        {
            var myCat = dbContext.Categories.ToList().FirstOrDefault(c => c.id == updatedCat.id);

            if (myCat == null)
                return NotFound();

            myCat.catName = updatedCat.catName;
            myCat.catOrder = updatedCat.catOrder;

            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Category updated successfully!";

            return View("Index", dbContext.Categories.Where(c => c.markedAsDeleted == false));
        }


        public IActionResult Delete(int id)
        {
            var cat = dbContext.Categories.FirstOrDefault(c => c.id == id); //used queriable
            if (cat == null)
                return NotFound();

            cat.markedAsDeleted = true;

            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
