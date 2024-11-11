using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Pages.Categories
{
    public class EditModel(ApplicationDbContext _db) : PageModel
    {
        [BindProperty]
        public Category? Category { get; set; }
        public IActionResult OnGet(int? id)
        {
            Category = _db.Categories.FirstOrDefault(category => category.Id == id);

            if (Category == null || id == null || id == 0)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Category?.Name == Category?.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name and Display order values can not be the same.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();

                TempData["created"] = "Category created successfully.";

                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
