using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private AppDbContext _db;
        public OrderHeaderRepository(AppDbContext db): base(db)
        {
            _db = db;
        }
      

        public void Update(OrderHeader obj)
        {
            _db.orderHeaders.Update(obj);
        }

        

        public void UpdateStatus(int id, string OrderStatus, string? paymentStatus = null)
        {
            var OrderFromDb=_db.orderHeaders.FirstOrDefault(u=>u.Order_id == id);
            if (OrderFromDb != null)
            {
                OrderFromDb.OrderStatus=OrderStatus;
                if (paymentStatus != null)
                {
                    OrderFromDb.PaymentStatus =paymentStatus; 
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string? paymentIntentId )
        {
            var OrderFromDb = _db.orderHeaders.FirstOrDefault(u => u.Order_id == id);
            OrderFromDb.PaymentDate = DateTime.Now; 
            OrderFromDb.SessionId = sessionId;
            OrderFromDb.PaymentIntentId = paymentIntentId;
        }
    }
}
 