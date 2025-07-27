using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Interfaces;

namespace PresentationLayer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.FindAsync(c => c.markedAsDeleted == false);
            return View(categories);
        }

        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> AddCategory(string catName, int catOrder)
        {
            var cat = new Category
            {
                catName = catName,
                catOrder = catOrder,
                markedAsDeleted = false
            };

            await _unitOfWork.Categories.AddAsync(cat);
            await _unitOfWork.CompleteAsync();

            var categories = await _unitOfWork.Categories.FindAsync(c => c.markedAsDeleted == false);
            return View("Index", categories);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _unitOfWork.Categories.GetByIdAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category updatedCat)
        {
            var cat = await _unitOfWork.Categories.GetByIdAsync(updatedCat.id);
            if (cat == null) return NotFound();

            cat.catName = updatedCat.catName;
            cat.catOrder = updatedCat.catOrder;

            _unitOfWork.Categories.Update(cat);
            await _unitOfWork.CompleteAsync();

            TempData["SuccessMessage"] = "Category updated successfully!";
            var categories = await _unitOfWork.Categories.FindAsync(c => c.markedAsDeleted == false);
            return View("Index", categories);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _unitOfWork.Categories.GetByIdAsync(id);
            if (cat == null) return NotFound();

            cat.markedAsDeleted = true;
            _unitOfWork.Categories.Update(cat);
            await _unitOfWork.CompleteAsync();

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}