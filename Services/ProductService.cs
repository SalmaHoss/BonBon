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

        public async Task<List<Product>> GetAll()
        {
            return await Context.Products.Include(i => i.Category).ToListAsync();
        }

        public async Task<Product> GetDetails(int id)
        {
            return await Context.Products.Include(e => e.Category).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Insert(Product product)
        {
            Context.Products.Add(product);
            await Context.SaveChangesAsync();

        }

        public async Task<Product> Update(int id, Product product)
        {
            Context.Update(product);
            await Context.SaveChangesAsync();

            return await Context.Products.Include(e => e.Category).FirstOrDefaultAsync(i => i.Id == id);

        }
        public async Task Delete(int id)
        {
            var product = await Context.Products.FindAsync(id);
            Context.Remove(product);
            await Context.SaveChangesAsync();
        }
    }
    }
