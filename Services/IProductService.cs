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
    }
}
