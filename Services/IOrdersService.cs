using AngularProject.Models;

namespace AngularProject.Services
{
    public interface IOrdersService
    {
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        public Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, int userId);



    }
}
