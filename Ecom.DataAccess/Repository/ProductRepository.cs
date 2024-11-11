using Ecom.DataAccess.Data;
using Ecom.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public void Update(Product product)
        {
            Product dbProduct = base.Get(p => p.Id == product.Id);
            foreach (var propUpdate in product.GetType().GetProperties())
            {
                if (propUpdate.GetValue(product) != null) dbProduct.GetType().GetProperty(propUpdate.Name)!.SetValue(dbProduct, propUpdate.GetValue(product));
            }

            /**
             * [TT]: Manually calling update here is unecessary.
             */
            //DbContext.Products.Update(dbProduct);
        }
    }
}
