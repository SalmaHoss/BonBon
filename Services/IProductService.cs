using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Services
{
    public interface IProductService
    {
        public List<Product> GetAll();
        public Task<Product> GetDetails(int id);
        public void Insert(Product product);
        public void Update(int id, Product product);
        public void Delete(int id);
        public Task<List<Product>> GetProductsRecommended(int numberOfrecords);
        public Task<List<Product>> SearchProduct(string name);
        public Task<List<Product>> FilterProducts(int categoryId);
        public Task<ActionResult<IEnumerable<Product>>> GetProductsWithPormotion();
        public Task<List<Product>> SortByAlpha(bool ascd);
        public Task<List<Product>> SortByPrice(bool Cheapest);
        public Task<List<Product>> SortByBestSellers();






    }
}
