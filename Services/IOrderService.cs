using AngularProject.Models;

namespace AngularProject.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        public Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, string userId);
    }
}

