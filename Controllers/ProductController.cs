using AngularProject.Models;
using AngularProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        public IProductRepository ProductRepository { get; set; }

        // request service of type "IStudentRepository"
        public ProductController(IProductRepository _ProductRepository)
        {
            ProductRepository = _ProductRepository;
        }


        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductRepository.GetAll();
        }

        //TODO : Products with promotions ===> allow anonymous 


        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return ProductRepository.GetDetails(id);
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] Product pro)
        {
            ProductRepository.Insert(pro);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product pro)
        {
            ProductRepository.Update(id, pro);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            ProductRepository.Delete(id);
        }
    }
}
