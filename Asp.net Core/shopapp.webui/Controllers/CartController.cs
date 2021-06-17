using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Identity;
using shopapp.webui.Models;


namespace shopapp.webui.Controllers
{
    [Authorize]
    public class CartController:Controller
    {
        private ICartService _cartService;

        private IOrderService _orderService;
        private UserManager<User> _userManager;

        
        public CartController(ICartService cartService,UserManager<User> userManager,IOrderService orderService)
        {   
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
        } 
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User)); // oturum açmış kullanıcı idsi ni verdik
           
            return View(new CartModel(){
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=> new CartItemModel(){
                    CartItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity


                }).ToList()
            });
        }
        
        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
             var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId,productId,quantity);
             return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId,productId);
            return RedirectToAction("Index");
        } 

        public IActionResult Checkout()
        {

            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User)); // oturum açmış kullanıcı idsi ni verdik
           
            var orderModel= new OrderModel(); 

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=> new CartItemModel(){
                    CartItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity


                }).ToList()
            };

            
            return View(orderModel);
        }

        [HttpPost]  
        public async Task<IActionResult> Checkout(OrderModel model,double totalPrice)
        {
           var user = await _userManager.GetUserAsync(User);
           var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
           model.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i=> new CartItemModel(){
                    CartItemId = i.Id,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity


                }).ToList()
            };
          
           if(user.wallet < totalPrice)
           {
               Console.WriteLine("Yetersiz Bakiye");
               return View("Detail","Yetersiz Bakiye");
           }
           // bakiye yetiyorsa

           //user.wallet = user.wallet - totalPrice;
           //await _userManager.UpdateAsync(user);

          var userId = _userManager.GetUserId(User);

          SaveOrder(model,userId);
          ClearCart(model.CartModel.CartId);


           return View("Detail","Siparişiniz Alındı");
        }

        
        public async Task<IActionResult> ApproveOrder(int id)
        {
              var order = _orderService.GetById(id);
               var user = await _userManager.GetUserAsync(User);
               user.wallet = user.wallet - order.TotalPrice;
               await _userManager.UpdateAsync(user);

               var admin = await _userManager.FindByNameAsync("Admin");
               admin.wallet = admin.wallet + order.TotalPrice;
               await _userManager.UpdateAsync(admin);

               //order silinmesi
               _orderService.Delete(order);
              //Console.WriteLine(id);
              return RedirectToAction("Index","Home");
        }

        public IActionResult GetOrders()
        { 
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            var orderlistmodel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.TotalPrice = order.TotalPrice;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;

                orderModel.OrderItems = order.OrderItems.Select(i=> new OrderListModel.OrderItemModel(){
                                OrderItemId =i.Id,
                                Name = i.Product.Name,
                                Price = (double)i.Price,
                                Quantity = i.Quantity,
                                ImageUrl = i.Product.ImageUrl                
                }).ToList();

                orderlistmodel.Add(orderModel); 

            }

            return View("Orders",orderlistmodel);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, string userId)
        {
            var order = new Order();
            
            order.OrderNumber = new Random().Next(111111,999999).ToString();
            order.OrderState = EnumOrderState.completed;
            order.TotalPrice = model.CartModel.TotalPrice();
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            
            order.OrderItems = new List<OrderItem>();
            
            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,

                };
                order.OrderItems = new List<entity.OrderItem>();
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
            
        }
    }
}