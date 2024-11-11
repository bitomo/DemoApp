using Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public interface ICartItemRepository: IRepository<CartItem>
    {
        void Update(CartItem cartItem);
    }
}
