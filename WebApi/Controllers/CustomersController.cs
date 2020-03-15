using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo customerRepo;

        public CustomersController(ICustomerRepo customerRepo)
        {
            this.customerRepo = customerRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await customerRepo.Customers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await customerRepo.CustomerById(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await customerRepo.Create(customer))
                return BadRequest();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!await customerRepo.Update(customer))
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            if (id == 0)
                return NotFound();

            if (!await customerRepo.DeleteById(id))
                return BadRequest();

            return Ok();
        }
    }
}