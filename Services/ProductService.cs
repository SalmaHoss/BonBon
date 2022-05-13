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

        public async Task<List<Product>> GetProductsRecommended(int numberOfrecords)
        {
            return await Context.Products
                .OrderByDescending(e => e.OverAllRating).
                 Take(numberOfrecords).Include(n=>n.Category).ToListAsync();

        }
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithPormotion()
        {
            return await Context.Products.Where(p => p.IsPromoted == true).ToListAsync();
        }
        public async Task<List<Product>> SearchProduct(string name)
        {
           
            var products = await Context.Products.
                Include(C => C.Category).Where(p =>p.Title.
                Contains(name)).ToListAsync();

            return products;
            

        }

        public async Task<List<Product>> FilterProducts(int categoryId)
        {

            var products = await Context.Products.
                Include(C => C.Category).Where(p=>p.CategoryId == categoryId).ToListAsync();

            return products;


        }

        public async Task<List<Product>> SortByAlpha(bool ascd)
        {
            //if ascd --> ture --> a --> Z

            if (ascd)
            {
                var products = await Context.Products.
                    Include(C => C.Category).OrderBy(p => p.Title).ToListAsync();
                return products;
            }
            else
            {
                var products = await Context.Products.
                    Include(C => C.Category).OrderByDescending(p => p.Title).ToListAsync();
                return products;
            }
        }

        public async Task<List<Product>> SortByPrice(bool Cheapest)
        {
            //if ascd --> ture --> a --> Z

            if (Cheapest)
            {
                var products = await Context.Products.
                    Include(C => C.Category).OrderBy(p => p.Price).ToListAsync();
                return products;
            }
            else
            {
                var products = await Context.Products.
                    Include(C => C.Category).OrderByDescending(p => p.Price).ToListAsync();
                return products;
            }
        }
        public async Task<List<Product>> SortByBestSellers()
        {
         
           var products = await Context.Products.
                    Include(C => C.Category).OrderByDescending(p => p.OverAllRating).ToListAsync();
                return products;
            
      
        }
    }
    }
