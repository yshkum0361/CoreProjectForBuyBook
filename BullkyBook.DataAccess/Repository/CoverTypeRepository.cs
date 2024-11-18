using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverTypeModel>, ICoverTypeRepository
    {
        private AppDbContext _db;
        public CoverTypeRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(CoverTypeModel obj)
        {
            _db.CoverTypeModels.Update(obj);
        }
    }
}
 