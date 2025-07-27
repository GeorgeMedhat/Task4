using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {

            var products = (await _unitOfWork.Products.FindAsync(p => !p.markedAsDeleted)).ToList();

            var categories = (await _unitOfWork.Categories.FindAsync(c => !c.markedAsDeleted)).ToList();

            foreach (var product in products)
            {
                product.category = categories.FirstOrDefault(c => c.id == product.categoryId);
            }

            ViewBag.Categories = categories;

            return View(products);
        }


        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Categories = await _unitOfWork.Categories.FindAsync(c => !c.markedAsDeleted);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _unitOfWork.Categories.FindAsync(c => !c.markedAsDeleted);
                return View(product);
            }

            product.markedAsDeleted = false;
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            var products = await _unitOfWork.Products.FindAsync(p => !p.markedAsDeleted);

            return RedirectToAction("Index" , products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();

            // Fetch active categories + the current one (even if deleted)
            var categories = await _unitOfWork.Categories
                .FindAsync(c => !c.markedAsDeleted || c.id == product.categoryId);

            ViewBag.Categories = new SelectList(categories, "id", "catName", product.categoryId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _unitOfWork.Categories.FindAsync(c => !c.markedAsDeleted);
                return View(updatedProduct);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(updatedProduct.id);
            if (product == null) return NotFound();

            product.title = updatedProduct.title;
            product.description = updatedProduct.description;
            product.author = updatedProduct.author;
            product.price = updatedProduct.price;
            product.categoryId = updatedProduct.categoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();

            TempData["SuccessMessage"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.markedAsDeleted = true;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();

            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
