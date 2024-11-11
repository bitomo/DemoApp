using Ecom.DataAccess.Repository;
using Ecom.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Ecom.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
    {
        public IActionResult Index()
        {
            return View(unitOfWork.ProductRepository.GetAll(new List<string> { nameof(Product.Category) }));
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                CategoryList = unitOfWork.CategoryRepository.GetAll().Select(category =>
                    new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    })
            };

            if (id > 0)
            {
                productVM.Product = unitOfWork.ProductRepository.Get(product => product.Id == id);
            }
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    
                    // [TT]: The "/" character is working on macOS, check if it is working on Windows
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string existingPath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.Trim('\\'));
                        try
                        {
                            System.IO.File.Delete(existingPath);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error deleting file: " + ex.Message);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == null || productVM.Product.Id == 0)
                {
                    unitOfWork.ProductRepository.Add(productVM.Product);
                    unitOfWork.Save();

                    TempData["created"] = "Product created successfully.";
                }
                else
                {
                    unitOfWork.ProductRepository.Update(productVM.Product);
                    unitOfWork.Save();

                    TempData["updated"] = "Product updated successfully.";
                }


                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = unitOfWork.CategoryRepository.GetAll().Select(category =>
                new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
                return View(productVM);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Product productToDelete = unitOfWork.ProductRepository.Get(product => product.Id == id);
                if (productToDelete == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    unitOfWork.ProductRepository.Remove(productToDelete);
                    unitOfWork.Save();

                    TempData["deleted"] = "Product deleted successfully.";

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = unitOfWork.ProductRepository.GetAll(new List<string> { nameof(Product.Category) });
            return Json(new { data });
        }
    }
}
