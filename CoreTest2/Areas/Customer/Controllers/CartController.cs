using BullkyBook.DataAccess.Repository;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using BullkyBook.Model.ViewModels;
using BullkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace CoreTest2.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public ShoppingCartVm shoppingCartVm { get; set; }
        public int OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            shoppingCartVm = new ShoppingCartVm()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == claim.Value,
                includeProperties: "product"),
                OrderHeader = new()


            };

            foreach (var cart in shoppingCartVm.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.count, cart.product.ListPrice,
                    cart.product.price50, cart.product.price100);
                shoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.count);

            }
            return View(shoppingCartVm);
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            shoppingCartVm = new ShoppingCartVm()
            {
                ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == claim.Value,
                includeProperties: "product"),
                OrderHeader = new()
            };

            shoppingCartVm.OrderHeader.AppUser = _unitOfWork.AppUserRepository.getFirstOrDefault(
                u => u.Id == claim.Value);

            shoppingCartVm.OrderHeader.User_name = shoppingCartVm.OrderHeader.AppUser.UserName;
            shoppingCartVm.OrderHeader.PhoneNumber = shoppingCartVm.OrderHeader.AppUser.PhoneNumber;
            shoppingCartVm.OrderHeader.StAddress = shoppingCartVm.OrderHeader.AppUser.StAddress;
            shoppingCartVm.OrderHeader.state = shoppingCartVm.OrderHeader.AppUser.state;
            shoppingCartVm.OrderHeader.city = shoppingCartVm.OrderHeader.AppUser.city;
            shoppingCartVm.OrderHeader.postalCode = shoppingCartVm.OrderHeader.AppUser.postalCode;


            foreach (var cart in shoppingCartVm.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.count, cart.product.ListPrice,
                    cart.product.price50, cart.product.price100);
                shoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.count);
            }
            return View(shoppingCartVm);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartVm shoppingCartVm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVm.ListCart = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == claim.Value,
               includeProperties: "product");

            //if (AppUser..GetValueOrDefault() == 0)
            //{
            shoppingCartVm.OrderHeader.OrderDate = System.DateTime.Now;
            shoppingCartVm.OrderHeader.AppUserId = claim.Value;
            //}


            foreach (var cart in shoppingCartVm.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.count, cart.product.ListPrice,
                    cart.product.price50, cart.product.price100);
                shoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.count);
            }

            AppUser appUser = _unitOfWork.AppUserRepository.getFirstOrDefault(u => u.Id == claim.Value);

            if (appUser.Comp_Id.GetValueOrDefault() == 0)
            {
                shoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;

            }
            else
            {
                shoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitOfWork.OrderHeaderRepository.Add(shoppingCartVm.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in shoppingCartVm.ListCart)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.Pro_Id,
                    Order_Id = shoppingCartVm.OrderHeader.Order_id,
                    price = cart.Price,
                    Count = cart.count
                };
                _unitOfWork.OrderDetailsRepository.Add(orderDetails);
                _unitOfWork.Save();
            }



            if (appUser.Comp_Id.GetValueOrDefault() == 0)
            {
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
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartVm.OrderHeader.Order_id}",
                    CancelUrl = domain + $"customer/cart/index",
                };

                foreach (var item in shoppingCartVm.ListCart)
                {

                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.product.pro_name
                            },

                        },
                        Quantity = item.count,
                    };
                    options.LineItems.Add(sessionLineItem);

                }

                var service = new SessionService();
                Session session = service.Create(options);
                shoppingCartVm.OrderHeader.SessionId = session.Id;
                shoppingCartVm.OrderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.OrderHeaderRepository.UpdateStripePaymentID(shoppingCartVm.OrderHeader.Order_id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVm.OrderHeader.Order_id });
            }
        }







        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.getFirstOrDefault(u => u.Order_id == id,includeProperties:"AppUser");

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);


                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRepository.UpdateStripePaymentID(id, orderHeader.SessionId, session.PaymentIntentId);

                    _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();


                }
            }
            _emailSender.SendEmailAsync(orderHeader.AppUser.Email, "New Order-Book", "<p>New Order Created</p>");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId ==
            orderHeader.AppUserId).ToList();
            _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int Shop_id)
        {
            var cart = _unitOfWork.ShoppingCartRepository.getFirstOrDefault(u => u.Shop_id == Shop_id);
            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int Shop_id)
        {
            var cart = _unitOfWork.ShoppingCartRepository.getFirstOrDefault(u => u.Shop_id == Shop_id);
            if (cart.count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cart);
                var count = _unitOfWork.ShoppingCartRepository.GetAll(u => u.AppUserId == cart.AppUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int Shop_id)
        {
            var cart = _unitOfWork.ShoppingCartRepository.getFirstOrDefault(u => u.Shop_id == Shop_id);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCartRepository.GetAll(u=>u.AppUserId==cart.AppUserId).ToList().Count-1;
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            return RedirectToAction(nameof(Index));
        }


        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else
            {
                if (quantity <= 100)
                {
                    return price50;
                }
                return price100;
            }
        }
    }
}
