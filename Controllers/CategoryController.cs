using AngularProject.Models;
using AngularProject.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public ICategoryRepository CategoryRepository { get; set; }

        // request service of type "IStudentRepository"
        public CategoryController(ICategoryRepository _CategoryRepository)
        {
            CategoryRepository = _CategoryRepository;
        }


        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return CategoryRepository.GetAll();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return CategoryRepository.GetDetails(id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post([FromBody] Category cat)
        {
            CategoryRepository.Insert(cat);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Category cat)
        {
            CategoryRepository.Update(id,cat);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            CategoryRepository.Delete(id);
        }
    }
}
