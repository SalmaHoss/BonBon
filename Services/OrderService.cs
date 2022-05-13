using AngularProject.Data.Cart;
using AngularProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            // var orders = await _context.Orders.Include(n => n.OrderProducts).ThenInclude(n => n.Product).Include(n => n.User).ToListAsync();

            //User do not want to be returned
            //var orders = await _context.Orders.Include(n => n.User).ToListAsync();
             var orders = await _context.Orders.Include(n => n.OrderProducts).ThenInclude(n => n.Product).ToListAsync();

            return orders;

        }
        public async Task<List<Order>> GetOrderDetailsAsync(int orderId)
        {
            var orderDetails = _context.Orders.Include(e => e.OrderProducts).ThenInclude(n => n.Product).Where(n => n.Id == orderId).ToListAsync();
            return await orderDetails;
        }


        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {    

            var orders = _context.Orders.Include(n => n.OrderProducts).ThenInclude(n => n.Product).Where(n => n.UserId == userId).ToListAsync();
            return await orders;
        }
        
        public async Task<List<ShoppingCartProduct>> GetshoppingCartIdAsync(string shoppingCartId)
        {
            var shoppingCart = _context.ShoppingCartProducts.Where(n => n.ShoppingCartId == shoppingCartId).ToListAsync();
            return await shoppingCart;
        }
        
        public async Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, string userId)
        {
         
            var order = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            foreach (var product in shoppingCartProducts)
            {
                var orderProd = new OrderProduct()
                {
                    Amount = product.Amount,
                    ProductId = product.Product.Id,
                    OrderId = order.Id,
                    Price = (double)product.Product.Price
                };
                await _context.OrderProducts.AddAsync(orderProd);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderState(int orderid, OrderState state)
        {
            var order = await _context.Orders.FindAsync(orderid);
            if (order != null)
            {
                order.State = state;
                await _context.SaveChangesAsync();
            }
            
            
        }




    }
}
