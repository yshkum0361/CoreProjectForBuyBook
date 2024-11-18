using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<CompanyModel>, ICompanyRepository
    {
        private AppDbContext _db;
        public CompanyRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(CompanyModel obj)
        {
            var objFromDB = _db.companyModels.FirstOrDefault(u => u.Comp_Id == obj.Comp_Id);
            if (objFromDB != null)
            {
                objFromDB.comp_name = obj.comp_name;  
                objFromDB.comp_City = obj.comp_City;
                objFromDB.comp_StAddress = obj.comp_StAddress;
                objFromDB.comp_State = obj.comp_State;
                objFromDB.PostalCode = obj.PostalCode;
                objFromDB.PhoneNumber = obj.PhoneNumber;
                

            }
        }
    }
}
 