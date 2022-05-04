using AngularProject.Models;

namespace AngularProject.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        public Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts, string userId);
<<<<<<< HEAD
=======
        public Task UpdateOrderState(int orderid, OrderState state);

>>>>>>> 834f87c28afdc2c05114654d7e241947f39e6ead
    }
}

