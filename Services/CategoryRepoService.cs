using AngularProject.Areas.Identity.Data;
using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class CategoryRepoService:ICategoryRepository
    {
        public AppDbContext Context { get; set; }
        public CategoryRepoService(AppDbContext context)
        {
            Context = context;
        }

        public List<Category> GetAll()
        {
            return Context.Categories.ToList();
        }

        public Category GetDetails(int id)
        {
            return Context.Categories.Find(id);
        }

        public async Task<ActionResult<Category>> Insert(Category cat)
        {
            Context.Categories.Add(cat);
            Context.SaveChanges();
            return cat;
        }

        public void Update(int id, Category cat)
        {
            Category catUpdated = Context.Categories.Find(id);
            catUpdated.Name = cat.Name;

            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            Context.Remove(Context.Categories.Find(id));
            Context.SaveChanges();
        }

    }
}
