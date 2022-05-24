﻿
using AngularProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace AngularProject.Data.Cart
{
    public class ShoppingCart
    {
        //in this session
        public string ShoppingCartId { get; set; }
         public  List<ShoppingCartProduct> ShoppingCartProducts {get;set;}
        //Is He gonna use services?
        [IgnoreDataMember]
        public ApplicationDbContext _Context { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _Context = context;
        }

        
        public static ShoppingCart GetShoppingCart(IServiceProvider service)
        {    //If this is not null
            ISession? session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            var ctxt = service.GetService<ApplicationDbContext>();
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            return new ShoppingCart(ctxt) { ShoppingCartId = cartId };
        }

        
  
        public List<ShoppingCartProduct> GetShoppingCartProducts(string ShoppingCartCurrentId)
        {
            return  ShoppingCartProducts ?? _Context.ShoppingCartProducts.Include(n => n.Product).Where(n => n.ShoppingCartId == ShoppingCartCurrentId).ToList();
        }
        public decimal GetShoppingCartTotal(string ShoppingCartCurrentId)
        {
            return _Context.ShoppingCartProducts.Where(n => n.ShoppingCartId == ShoppingCartCurrentId).Select(n => n.Product.Price * n.Amount).Sum();
        }


        public void AddProductToCart(Product product)
        {                                              
            var shoppingCartProduct = _Context.ShoppingCartProducts.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);
            if(shoppingCartProduct == null)
            {
                shoppingCartProduct = new ShoppingCartProduct()
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Amount = 1
                };
                _Context.ShoppingCartProducts.Add(shoppingCartProduct);
            }
            else
            {
                if (shoppingCartProduct.Amount < product.Quantity)
                    shoppingCartProduct.Amount++;
                else
                    return;

            }
            _Context.SaveChanges();
        }


        public void RemoveProductFromCart(Product product)
        {
            var shoppingCartProduct = _Context.ShoppingCartProducts.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCartProduct != null)
            {
                if(shoppingCartProduct.Amount > 1)
                {
                    shoppingCartProduct.Amount--;
                }
                else
                {
                    _Context.ShoppingCartProducts.Remove(shoppingCartProduct);
                }
            }
            _Context.SaveChanges();
        }


        public async Task ClearShoppingCartAsync()
        {
            var items = await _Context.ShoppingCartProducts.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _Context.ShoppingCartProducts.RemoveRange(items);
            await _Context.SaveChangesAsync();
        }

        internal void RemoveProductTotalAmountFromCart(Product product)
        {
            var shoppingCartProduct = _Context.ShoppingCartProducts.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCartProduct != null)
            {
           
                    _Context.ShoppingCartProducts.Remove(shoppingCartProduct);
                
            }
            _Context.SaveChanges();
        }
    }
}
