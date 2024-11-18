using BullkyBook.DataAccess.Repository;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using BullkyBook.Model.ViewModels;
using BullkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace CoreTest2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]

        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Details(int order_id)
        {
            OrderVM = new OrderVM()
            {
                orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == order_id, includeProperties: "AppUser"),
                orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(u => u.Order_Id == order_id, includeProperties: "ProductModel"),
            };
            return View(OrderVM);
        }

        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == OrderVM.orderHeader.Order_id, includeProperties: "AppUser");
            OrderVM.orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(u => u.Id == OrderVM.orderHeader.Order_id, includeProperties: "Product");

            //stripe settings 
            var domain = "https://localhost:44300/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={OrderVM.orderHeader.Order_id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.orderHeader.Order_id}",
            };

            foreach (var item in OrderVM.orderDetails)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.price * 100),//20.00 -> 2000
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductModel.pro_name
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeaderRepository.UpdateStripePaymentID(OrderVM.orderHeader.Order_id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            
        }


        public IActionResult PaymetConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == orderHeaderid);

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);


                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeaderid, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();


                }
            }

            return View(orderHeaderid);
        }


        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        [ValidateAntiForgeryToken]

        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == OrderVM.orderHeader.Order_id,tracked:false);
            orderHeaderFromDb.User_name = OrderVM.orderHeader.User_name;
            orderHeaderFromDb.PhoneNumber = OrderVM.orderHeader.PhoneNumber;
            orderHeaderFromDb.StAddress = OrderVM.orderHeader.StAddress;
            orderHeaderFromDb.city = OrderVM.orderHeader.city;
            orderHeaderFromDb.state = OrderVM.orderHeader.state;
            orderHeaderFromDb.postalCode = OrderVM.orderHeader.postalCode;
            if (OrderVM.orderHeader.Carrier != null)
            {
                orderHeaderFromDb.Carrier = OrderVM.orderHeader.Carrier;
            }
            if (OrderVM.orderHeader.TrackingNumber != null)
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeaderRepository.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "order Details Updated successfully!!!";
            return RedirectToAction("Details","Order",new { order_id = orderHeaderFromDb.Order_id});
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeaderRepository.UpdateStatus(OrderVM.orderHeader.Order_id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "order status Updated successfully!!!";
            return RedirectToAction("Details", "Order", new { order_id = OrderVM.orderHeader.Order_id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == OrderVM.orderHeader.Order_id, tracked: false);
            orderHeader.TrackingNumber=OrderVM.orderHeader.TrackingNumber;
            orderHeader.Carrier=OrderVM.orderHeader.Carrier;
            orderHeader.OrderStatus=SD.StatusShipped;
            orderHeader.ShippingDate=DateTime.Now;

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDate=DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeaderRepository.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "order Shipped successfully!!!";
            return RedirectToAction("Details", "Order", new { order_id = OrderVM.orderHeader.Order_id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == OrderVM.orderHeader.Order_id, tracked: false);
            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund=service.Create(option);
                _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeader.Order_id, SD.StatusCancelled, SD.StatusRefunded);

            }
            else
            {
                _unitOfWork.OrderHeaderRepository.UpdateStatus(orderHeader.Order_id, SD.StatusCancelled, SD.StatusCancelled);

            }
            _unitOfWork.Save();
            TempData["success"] = "order Cancelled successfully!!!";
            return RedirectToAction("Details", "Order", new { order_id = OrderVM.orderHeader.Order_id });
        }


        #region API CALLS


        [HttpGet]


        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;


            if(User.IsInRole(SD.Role_Admin)|| User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "AppUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(u => u.AppUserId == claim.Value, includeProperties: "AppUser");

            }


            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }


            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
