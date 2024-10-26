using API_Clients.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API_Clients.Services
{
        public interface ICustomerService
        {
            Task<bool> CheckCustomerStatus(int customerId);
        }

    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckCustomerStatus(int customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            return customer != null && !string.IsNullOrEmpty(customer.Address);
        }

    }
}