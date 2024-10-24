namespace API_Clients.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string LastName { get; set; } = string.Empty; 
        public string FirstName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty; 
        public string PhoneNumber { get; set; } = string.Empty; 
    }
}
