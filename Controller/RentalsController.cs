using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.DTO;
using VehicleRentalAPI.Entities;

namespace VehicleRentalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController(VehicleRentalContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RentalDTO rentalDTO)
        {
            if (rentalDTO is null) return BadRequest("Empty input");

            var rental = new Rental
            {
                CustomerId = rentalDTO.CustomerId,
                VehicleId = rentalDTO.VehicleId,
                RentalDate = rentalDTO.RentalDate,
                ReturnDate = rentalDTO.ReturnDate,
            };

            await context.Rentals.AddAsync(rental);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rentals = await context.Rentals.Include(r => r.Vehicle).Include(c => c.Customer).ToListAsync();
            if (rentals is null) return NotFound("No rentals found");

            return Ok(rentals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rental = await context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental is null) return NotFound("No rental found");

            return Ok(rental);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Rental rental)
        {
            var rentalToUpdate = await context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rentalToUpdate == null) return NotFound("Rental not found");

            rentalToUpdate.RentalDate = rental.RentalDate;
            rentalToUpdate.ReturnDate = rental.RentalDate;

            await context.SaveChangesAsync();
            return Ok("Updated successfuly");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null) return NotFound("Rental not found");

            context.Rentals.Remove(rental);
            await context.SaveChangesAsync();
            return Ok("Deleted successfuly");
        }
    }
}
