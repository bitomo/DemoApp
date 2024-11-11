using Ecom.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext DbContext { get; set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public ICartItemRepository CartItemRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext dbContext) {
            DbContext = dbContext;
            CategoryRepository = new CategoryRepository(DbContext);
            ProductRepository = new ProductRepository(DbContext);
            CompanyRepository = new CompanyRepository(DbContext);
            CartItemRepository = new CartItemRepository(DbContext);
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}
