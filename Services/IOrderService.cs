using AngularProject.Models;

namespace AngularProject.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        public Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, string userId);
        public Task UpdateOrderState(int orderid, string state);

        public Task<List<Order>> GetOrderDetailsAsync(int Id);
        public Task<List<ShoppingCartProduct>> GetshoppingCartIdAsync(string shoppingCartId);
        public Task<Boolean> DeleteShoppingCartIdAsync(string Id);
        Task DeleteOrderById(int id);
    }
}

