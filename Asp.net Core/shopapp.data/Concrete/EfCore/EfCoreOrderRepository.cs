using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using(var context = new ShopContext())
            {
                var orders = context.Orders
                                    .Include(o=>o.OrderItems)
                                    .ThenInclude(oi=>oi.Product)
                                    .AsQueryable();

               if(!string.IsNullOrEmpty(userId))
               {
                   orders = orders.Where(o=>o.UserId == userId);
               }     

               return orders.ToList();                
            }
        }
    }
}