﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProject.Models;
using AngularProject.Data.Cart;
using AngularProject.Services;
using AngularProject.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        //Add private readonly IProductService _productService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrdersController(ShoppingCart shoppingCart,
            IProductService productService,
            IOrderService orderService
            , IUserService userService)
        {
            _shoppingCart = shoppingCart;
            _productService = productService;
            _orderService = orderService;
            _userService = userService;

        }

        [HttpGet]
        public async Task<IActionResult> getAllOrders()
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string userRole = User.FindFirstValue(ClaimTypes.Role);
            //var orders = await orderRepository.GetOrderByUserIdRoleAsync(userId, userRole);

            

            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
            
        }

        [HttpGet("GetShoppingCartItems")]
        public IActionResult GetShoppingCartItems()
        {
            var products = _shoppingCart.GetShoppingCartProducts();
            _shoppingCart.ShoppingCartProducts = products;
            //var response = new ShoppingCartVM()
            //{
            //    ShoppingCart = _shoppingCart,
            //    ShoppingCartTotal = (double)_shoppingCart.GetShoppingCartTotal()
            //};
            return Ok(products);
        }

        [HttpPost("AddItem/{id}")]
        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var product = await _productService.GetDetails(id);

            if (product!= null)
            {
                _shoppingCart.AddProductToCart(product);
            }
            return NoContent();

        }

        [HttpPost("RemoveItem/{id}")]
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var product = await _productService.GetDetails(id);

            if (product != null)
            {
                _shoppingCart.RemoveProductFromCart(product);
            }
            return NoContent();
            

        }

        [HttpPost("completerOrder/{id}")]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            var user = await _userService.UserExistAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var products = _shoppingCart.GetShoppingCartProducts();
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.StoreOrder(products, id);
            await _shoppingCart.ClearShoppingCartAsync();

            return NoContent();
        }

        [HttpPost("changeOrderStateById/{id}")]
        public async Task<IActionResult> ChangeOrderState(int id, OrderState state)
        {            
            await _orderService.UpdateOrderState(id, state);

            return NoContent();
        }
        [HttpGet("GetOrdersByUserId/{id}")]
     
        public async Task<IActionResult> GetOrdersByUserId(string id) { 
        
        var orders = await _orderService.GetOrdersByUserIdAsync(id);
            return Ok(orders);

        }


        //public IActionResult CompleteOrder()
        //{
        //    var items = shoppingCart.GetShoppingCartItems();
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

        //     orderRepository.StoreOrderAsync(items, userId, userEmailAddress);
        //     shoppingCart.ClearShoppingCartAsync();

        //    return View("OrderCompleted");
        //}



        //public static ShoppingCart GetShoppingCart(IServiceProvider serviceProvider)
        //{
        //    ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        //    var context = serviceProvider.GetService<ApplicationDbContext>();
        //    string CartId = session.GetString("CartID") ?? Guid.NewGuid().ToString();
        //    session.SetString("CartId", CartId);
        //    return new ShoppingCart(context) { ShoppingCartId = CartId };

        //}
        //[HttpGet]

        //public  async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        //{
        //    return  await _
        //        //_shoppingCart.GetShoppingCartProducts();
        //}

        /*  public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
          {
              return await _
                  //_shoppingCart.GetShoppingCartProducts();
          }*/


        // GET: api/Products/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(int id, Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Orders
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        //}

        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}
    }
}