﻿using AngularProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class OrderService : IOrdersService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = _context.Orders.Include(n => n.OrderProducts).ThenInclude(n => n.Product).Where(n => n.UserId == userId).ToListAsync();
            return await orders;
        }
        public async Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, int userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress,
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
    }
}