using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Pages.Categories
{
    /**
     * [TT]: Use this annotation to bind all class properties to the page.
     */
    //[BindProperties]
    public class CreateModel(ApplicationDbContext _db) : PageModel
    {
        [BindProperty]
        public Category Category { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (Category?.Name == Category?.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name and Display order values can not be the same.");
            }


            if (ModelState.IsValid)
            {
                _db.Categories.Add(Category);
                _db.SaveChanges();

                TempData["created"] = "Category created successfully.";

                return RedirectToPage("Index");
            }

            return Page();
        }

    }
}
