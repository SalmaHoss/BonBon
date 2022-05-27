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

        public async Task DeleteOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

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

        public async Task<Boolean> DeleteShoppingCartIdAsync(string shoppingCartId)
        {
            var shoppingCart = _context.ShoppingCartProducts.Where(n => n.ShoppingCartId == shoppingCartId).FirstOrDefault();
            if (shoppingCart != null)
            {
                _context.ShoppingCartProducts.Remove(shoppingCart);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {

                return false;
            }
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

        public async Task UpdateOrderState(int orderid, string state)
        {
            var order = await _context.Orders.FindAsync(orderid);
            if (order != null)
            {
                order.State = (OrderState)Enum.Parse(typeof(OrderState), state);
                if(order.State == OrderState.Accepted)
                {
                    var orderProducts = await _context.OrderProducts.Where(order => order.OrderId == orderid).ToListAsync();
                    foreach(var orderProduct in orderProducts)
                    {
                        Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == orderProduct.ProductId);
                        product.Quantity -= orderProduct.Amount;
                    //    _context.Products.Update(product);
                    }
                 }
                await _context.SaveChangesAsync();
            }
            
        }




    }
}
