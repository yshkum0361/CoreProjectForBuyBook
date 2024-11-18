using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private AppDbContext _db;
        public OrderDetailsRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(OrderDetails obj)
        {
            _db.orderDetail.Update(obj);
        }
    }
}
 