using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAll();
        public Task<Product> GetDetails(int id);
        public Task<Product> Update(int id, Product product);
        public Task<Product> Insert(Product product);
        public Task<Product> Delete(int id);

    }
}
