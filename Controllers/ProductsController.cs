#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProject.Models;
using Microsoft.AspNetCore.Authorization;
using AngularProject.Services;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ProductsController(ApplicationDbContext context,
            IProductService productService)
        {
            _context = context;
            _productService = productService;
            
        }

        // GET: api/Products
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include("Category").ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        //[Authorize]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _context.Products.Include(e => e.Category).FirstOrDefaultAsync(i => i.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Admin")]

        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetProductsWithPormotion")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsWithPormotion()
        {
            return await _context.Products.Include("Category").Where(p => p.IsPromoted == true).ToListAsync();
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        [HttpGet("getProductRecommende/{productsNum}")]
        public async Task<IActionResult> getProductRecommended(int productsNum)
        {
            var products = await _productService.GetProductsRecommended(productsNum);

            return Ok(products);
        }

        [HttpGet("searchProduct/{productName}")]
        public async Task<IActionResult> SearchProdByName(string productName)
        {
            var products = await _productService.SearchProduct(productName);

            return Ok(products);
        }

        [HttpGet("FilterProducts/{CategotyId}")]
        public async Task<IActionResult> FilterProductsByCategoryID(int CategotyId)
        {
            var products = await _productService.FilterProducts(CategotyId);

            return Ok(products);
        }


    }
}