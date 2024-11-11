using Ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public interface ICompanyRepository: IRepository<Company>
    {
        void Update(Company company);
    }
}
