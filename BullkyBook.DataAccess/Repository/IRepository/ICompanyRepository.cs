using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository: IRepository<CompanyModel>
    {

        void Update(CompanyModel obj);
        
    }
}
