using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.Entities;

namespace VehicleRentalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController(VehicleRentalContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {

            if (vehicle is null) return BadRequest("Empty input");

            await context.Vehicles.AddAsync(vehicle);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vehicles = await context.Vehicles.ToListAsync();
            if (vehicles is null) return NotFound("No vehicles found");

            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle is null) return NotFound("No vehicle found");

            return Ok(vehicle);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Vehicle vehicle)
        {
            var vehicleToUpdate = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicleToUpdate == null) return NotFound("Vehicle not found");

            vehicleToUpdate.Model = vehicle.Model;
            vehicleToUpdate.LicensePlate = vehicle.LicensePlate;
            vehicleToUpdate.DailyRate = vehicle.DailyRate;
            vehicleToUpdate.Available = vehicle.Available;

            await context.SaveChangesAsync();
            return Ok("Updated successfuly");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null) return NotFound("Vehicle not found");

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();
            return Ok("Deleted successfuly");
        }
    }
}
