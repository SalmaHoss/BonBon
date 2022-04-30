#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProject.Models;
using System.Security.Claims;
using AngularProject.Services;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductService ProductServices;

        public ProductsController(ProductService _ProductServices)
        {
            ProductServices = _ProductServices;
        }

        // GET: api/Products
        //[HttpGet(Name = "GetProducts")]
        [HttpPost("")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier);

            return Ok(ProductServices.GetAll());


        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = ProductServices.GetDetails(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult<Product> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var Product = ProductServices.Update(id, product);

            if(product == null)return BadRequest();
            else return Ok(Product);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            var addedproduct = ProductServices.Insert(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = ProductServices.Delete(id);
            if (product==null)
            {
                return NotFound();
            }
            else return NoContent();

            

            
        }
    }
}
