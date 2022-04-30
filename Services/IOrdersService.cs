using AngularProject.Models;

namespace AngularProject.Services
{
    public interface IOrdersService
    {
        Task StoreOrder(List<ShoppingCartProduct> shoppingCartProducts,int userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

    }
}
