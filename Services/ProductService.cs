using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class ProductService : IProductService
    {
        public ApplicationDbContext Context { get; set; }
        public ProductService(ApplicationDbContext context)
        {
            Context = context;
        }

        public List<Product> GetAll()
        {
            return Context.Products.Include(i => i.Category).ToList();
        }

        public async Task<Product> GetDetails(int id)
        {
            return await Context.Products.Include(e => e.Category).FirstOrDefaultAsync(i => i.Id == id);
        }

        public void Insert(Product product)
        {
            Context.Products.Add(product);
            Context.SaveChanges();
        }

        public void Update(int id, Product product)
        {
            Context.Update(product);
            Context.SaveChanges();
        }
        public void Delete(int id)
        {
            Context.Remove(Context.Products.Find(id));
            Context.SaveChanges();
        }
    }
    }
