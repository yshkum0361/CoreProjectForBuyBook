using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<BullkyBookModel>, ICategoryRepository
    {
        private AppDbContext _db;
        public CategoryRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(BullkyBookModel obj)
        {
            _db.bullkyBookModels.Update(obj);
        }
    }
}
 