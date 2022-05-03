using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public Task<Product> GetDetails(int id);
        public Task Insert(Product product);
        public Task<Product> Update(int id, Product product);
        public Task Delete(int id);
    }
}
