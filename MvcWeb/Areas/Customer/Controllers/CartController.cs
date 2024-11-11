using Ecom.DataAccess.Repository;
using Ecom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }

        public CartController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            UserManager = userManager;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);

            CartVM cartVM = new CartVM
            {
                CartItems = UnitOfWork.CartItemRepository
                                .GetAll(new List<string> { nameof(CartItem.Product) })
                                .Where(item => item.ApplicationUserId == user.Id)
            };

            foreach (var item in cartVM.CartItems)
            {
                item.Price = GetPriceBasedOnQuantity(item);
                cartVM.Total += (item.Price * item.Count);
            }

            return View(cartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult RemoveCartItem(int id)
        {
            try
            {
                CartItem cartItemDBO = UnitOfWork.CartItemRepository.Get(cdbo => cdbo.Id == id);

                UnitOfWork.CartItemRepository.Remove(cartItemDBO);
                UnitOfWork.Save();

                TempData["updated"] = "Product removed from cart.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult IncrementCartItem(int id)
        {
            try
            {
                CartItem cartItemDBO = UnitOfWork.CartItemRepository.Get(cdbo => cdbo.Id == id);
                cartItemDBO.Count++;
                UnitOfWork.Save();
                TempData["updated"] = "Cart updated.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult DecrementCartItem(int id)
        {
            try
            {
                CartItem cartItemDBO = UnitOfWork.CartItemRepository.Get(cdbo => cdbo.Id == id);
                cartItemDBO.Count--;
                if (cartItemDBO.Count == 0)
                {
                    UnitOfWork.CartItemRepository.Remove(cartItemDBO);
                    TempData["updated"] = "Product removed from cart.";
                }
                else
                {
                    TempData["updated"] = "Cart updated.";
                }
                UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private double GetPriceBasedOnQuantity(CartItem cartItem)
        {
            if (cartItem.Count <= 50)
            {
                return cartItem.Product.Price;
            }
            else
            {
                if (cartItem.Count <= 100)
                {
                    return cartItem.Product.Price50;
                }
                else
                {
                    return cartItem.Product.Price100;
                }
            }
        }

    }
}
