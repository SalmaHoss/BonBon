﻿using AngularProject.Areas.Identity.Data;
using AngularProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace AngularProject.Data.Cart
{
    public class ShoppingCart
    {

         public string ShoppingCartId { get; set; }
         public  List<ShoppingCartProduct> ShoppingCartProducts {get;set;}
         //Is He gonna use services?
         public AppDbContext _Context { get; set; }

        public ShoppingCart(AppDbContext context)
        {
            _Context = context;
        }


        //to be used in program
        public static ShoppingCart GetShoppingCart(IServiceProvider service)
        {    //If this is not null
            ISession? session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var ctxt = service.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(ctxt) { ShoppingCartId = cartId };
        }
        public  List<ShoppingCartProduct> GetShoppingCartProducts()
        {
            return  ShoppingCartProducts ?? _Context.ShoppingCartProducts.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n=>n.Product).ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            return _Context.ShoppingCartProducts.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Product.Price * n.Amount).Sum();
        }


        public void AddProductToCart(Product product)
        {
            var shoppingCartProduct = _Context.ShoppingCartProducts.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);
            if(shoppingCartProduct != null)
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
                shoppingCartProduct.Amount++;
            }
            _Context.SaveChanges();
        }


        public void RemoveProductToCart(Product product)
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
    }
}
