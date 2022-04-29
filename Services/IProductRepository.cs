using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Services
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
        public Product GetDetails(int id);
        public Task<ActionResult<Product>> Insert(Product pro);
        public void Update(int id, Product pro);
        public void Delete(int id);
    }
}
