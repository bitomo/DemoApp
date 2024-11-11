using Ecom.DataAccess.Repository;
using Ecom.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecom.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController(IUnitOfWork unitOfWork) : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id > 0)
            {
                company = unitOfWork.CompanyRepository.Get(company => company.Id == id);
            }
            return View(company);
        }

        [HttpPost]
        public ActionResult Upsert(Company companyInput)
        {
            if (ModelState.IsValid)
            {
                if (companyInput.Id == 0)
                {
                    unitOfWork.CompanyRepository.Add(companyInput);
                    unitOfWork.Save();

                    TempData["created"] = "Company added successfully.";
                }
                else
                {
                    unitOfWork.CompanyRepository.Update(companyInput);
                    unitOfWork.Save();

                    TempData["updated"] = "Company updated successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(companyInput);
            }
        }

        public ActionResult Delete(int id)
        {
            Company companyToDelete = unitOfWork.CompanyRepository.Get(company => company.Id == id);
            if (companyToDelete == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                unitOfWork.CompanyRepository.Remove(companyToDelete);
                unitOfWork.Save();

                TempData["deleted"] = "Company deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult GetAll()
        {
            var data = unitOfWork.CompanyRepository.GetAll();
            return Json(new { data });
        }
    }
}
