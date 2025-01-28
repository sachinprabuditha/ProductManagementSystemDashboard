namespace ProductManagementSystemDashboard.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public decimal State { get; set; }
        public string City { get; set; }
        public string ProductionDocument { get; set; }
        public string CreatedOn { get; set; }
    }
}