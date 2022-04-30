using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class ProductService:IProductService
    {
        public ApplicationDbContext Context { get; set; }
        public ProductService(ApplicationDbContext context)
        {
            Context = context;
        }

       
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await Context.Products.Include("Category").ToListAsync();
        }

        public async Task<Product> GetDetails(int id)
        {
            var product = await Context.Products.FindAsync(id);
            return product;
        }

        
        public async Task<Product> Update(int id, Product product)
        {
            
            Context.Entry(product).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
           var updatedproduct = await Context.Products.FindAsync(id);
            return product;

        }

        public async Task<Product> Insert(Product product)
        {
            Context.Products.Add(product);
            await Context.SaveChangesAsync();

            var addedproduct = await Context.Products.FindAsync(product.Id);
            return product;
        }



        public async Task<Product> Delete(int id)
        {
            var product = await Context.Products.FindAsync(id);

            if (product != null)
            {
                Context.Products.Remove(product);
                await Context.SaveChangesAsync();
            }
            return product;
        }
        public bool Exists(int id)
        {
            return Context.Products.Any(e => e.Id == id);
        }


    }
}
