using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularProject.Services
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetDetails(int id);
        public Task<ActionResult<Category>> Insert(Category Cat);
        public void Update(int id, Category Cat);
        public  void Delete(int id);
    }
}
