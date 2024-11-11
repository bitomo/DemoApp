using Ecom.DataAccess.Data;
using Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private ApplicationDbContext DbContext;

        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public void Update(CartItem cartItem)
        {
            DbContext.CartItems.Update(cartItem);
        }
    }
}
