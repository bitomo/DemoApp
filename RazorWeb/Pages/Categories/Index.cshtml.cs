using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Pages.Categories
{
    public class IndexModel(ApplicationDbContext _db) : PageModel
    {
        public List<Category> CategoryList { get; set; }

        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }

        public IActionResult OnPostDelete(int? id)
        {
            Category? categoryToRemove = _db.Categories.Find(id);

            if (categoryToRemove is not null)
            {
                _db.Remove(categoryToRemove);
                _db.SaveChanges();
            }

            TempData["deleted"] = "Category deleted successfully.";

            return RedirectToPage("Index");
        }
    }
}
