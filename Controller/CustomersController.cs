using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.Entities;

namespace VehicleRentalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(VehicleRentalContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            if (customer is null) return BadRequest("Empty input");

            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await context.Customers.ToListAsync();
            if (customers is null) return NotFound("No customers found");

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer is null) return NotFound("No customer found");

            return Ok(customer);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            var customerToUpdate = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customerToUpdate == null) return NotFound("Customer not found");

            customerToUpdate.Name = customer.Name;
            customerToUpdate.Email = customer.Email;
            customerToUpdate.PhoneNumber = customer.PhoneNumber;

            await context.SaveChangesAsync();
            return Ok("Updated successfuly");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null) return NotFound("Customer not found");

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return Ok("Deleted successfuly");
        }
    }
}
