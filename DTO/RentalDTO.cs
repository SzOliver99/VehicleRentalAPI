namespace VehicleRentalAPI.DTO
{
    public class RentalDTO
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }
    }
}
