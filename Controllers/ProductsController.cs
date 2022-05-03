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
    [Authorize]
    public class ProductsController : ControllerBase
    {
        // private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products =  await _productService.GetAll();
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetDetails(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateProductByiD/{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
           
            if (id != product.Id)
            {
                return BadRequest();
            }
            var getProduct = await _productService.GetDetails(id);
            
            if (getProduct == null)
            {
                    return NotFound();
            }
            else
            {
                var updatedProduct = _productService.Update(id, product);
                return Ok(updatedProduct);
            }
            

            
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddProduct/{id}")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _productService.Insert(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productService.GetDetails(id);
            if (product == null)
            {
                return NotFound();
            }

            _productService.Delete(id);

            return NoContent();
        }

      
    }
}