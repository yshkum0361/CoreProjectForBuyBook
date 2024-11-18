using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private AppDbContext _db;
        public ProductRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(ProductModel obj)
        {
            var objFromDB = _db.productModels.FirstOrDefault(u => u.pro_id == obj.pro_id);
            if (objFromDB != null)
            {
                objFromDB.pro_name = obj.pro_name;  
                objFromDB.pro_description = obj.pro_description;
                objFromDB.price100 = obj.price100;
                objFromDB.price50 = obj.price50;
                objFromDB.Author = obj.Author;
                objFromDB.categoryID = obj.categoryID;
                objFromDB.coverTypeID = obj.coverTypeID;
                objFromDB.ListPrice = obj.ListPrice;

                if (obj.ImageURL != null)
                {
                    objFromDB.ImageURL = obj.ImageURL;
                }
            }
        }
    }
}
 