using Microsoft.AspNetCore.Mvc;
using Ecom.Models;
using System.Diagnostics;
using Ecom.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Ecom.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }

        public HomeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            UnitOfWork = unitOfWork;
            UserManager = userManager;
        }

        public IActionResult Index()
        {
            return View(UnitOfWork.ProductRepository.GetAll(new List<string> { nameof(Category) }));
        }

        public IActionResult Details(int productId)
        {
            CartItem cartItem = new CartItem()
            {
                Product = UnitOfWork.ProductRepository.Get(
                            cartItem => cartItem.Id == productId,
                            new List<string> { nameof(Category) }
                          ),
                Count = 1,
                ProductId = productId
            };

            return View(cartItem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(CartItem cartItemVM)
        {
            // [TT]: Example 1 of getting user id.
            //var userIdentity = User.Identity;
            //var claimsIdentity = (ClaimsIdentity)userIdentity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // [TT]: Example 2 of getting user id.
            //string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier).Value;

            // [TT]: Example 3 of getting user id.
            var user = await UserManager.GetUserAsync(HttpContext.User);

            cartItemVM.ApplicationUserId = user.Id;

            CartItem cartItemDBO = UnitOfWork.CartItemRepository.Get(cdbo => cdbo.ProductId == cartItemVM.ProductId && cdbo.ApplicationUserId == cartItemVM.ApplicationUserId);

            if (cartItemDBO == null)
            {
                UnitOfWork.CartItemRepository.Add(cartItemVM);
            }
            else
            {
                cartItemDBO.Count = cartItemVM.Count;
                UnitOfWork.CartItemRepository.Update(cartItemDBO);
            }

            UnitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
