using AngularProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProject.Services
{
    public class ProductRepoService:IProductService
    {
        public ApplicationDbContext Context { get; set; }
        public ProductRepoService(ApplicationDbContext context)
        {
            Context = context;
        }

        public List<Product> GetAll()
        {
            return Context.Products.Include("Category").ToList();
        }

        public Product GetDetails(int id)
        {
            return Context.Products.Find(id);
        }

        public async Task<ActionResult<Product>> Insert(Product pro)
        {
            Context.Products.Add(pro);
            Context.SaveChanges();
            return pro;
        }

        public void Update(int id, Product pro)
        {
            Product proUpdated = Context.Products.Find(id);
        
            proUpdated.Title = pro.Title;
            proUpdated.Description= pro.Description;
            proUpdated.Price = pro.Price;
            proUpdated.IsPromoted=pro.IsPromoted;
            proUpdated.PromotionPercentage=pro.PromotionPercentage;
            proUpdated.Title=pro.Title;
            proUpdated.Category=pro.Category;
            proUpdated.CategoryId=pro.CategoryId;
            proUpdated.Quantity=pro.Quantity;
            proUpdated.ImageUrl=pro.ImageUrl;
            proUpdated.OverAllRating=pro.OverAllRating;

            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            Context.Remove(Context.Products.Find(id));
            Context.SaveChanges();
        }

        
    }
}
