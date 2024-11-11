using Microsoft.AspNetCore.Mvc;
using Ecom.Models;
using Ecom.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Ecom.Utility;

namespace Ecom.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CategoryController(IUnitOfWork unitOfWork) : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = unitOfWork.CategoryRepository.GetAll();
            return View(categoryList);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (model.Name == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name and Display order values can not be the same.");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Add(model);
                unitOfWork.Save();

                TempData["created"] = "Category created successfully.";

                //return Redirect("Index");
                return RedirectToAction("Index", "Category");
            }
            return View(model);
        }


        //[HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            Category? categoryToRemove = unitOfWork.CategoryRepository.Get(category => category.Id == id);

            if (categoryToRemove is not null)
            {
                unitOfWork.CategoryRepository.Remove(categoryToRemove);
                unitOfWork.Save();
            }

            TempData["deleted"] = "Category deleted successfully.";

            return RedirectToAction("Index", "Category");
        }


        public IActionResult Edit(int? id)
        {
            Category? category = unitOfWork.CategoryRepository.Get(category => category.Id == id);
            if (category == null || id == null || id == 0)
            {
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }


        [HttpPost]
        public IActionResult Edit(Category model)
        {

            if (model.Name == model.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name and Display order values can not be the same.");
            }

            // Because the id of the category is named "Id", it is automatically included in the post request from the form.
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Update(model);
                unitOfWork.Save();

                TempData["updated"] = "Category updated successfully.";

                return RedirectToAction("Index", "Category");
            }
            return View();
        }


    }
}
