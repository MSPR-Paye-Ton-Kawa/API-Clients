using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using API_Clients.Controllers;
using API_Clients.Models;

namespace API_Clients.Tests
{
    public class CustomersControllerTests
    {
        private readonly AppDbContext _context;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            // Configuration du DbContext en mémoire
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _controller = new CustomersController(_context);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomer_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { FirstName = "Alice", LastName = "Smith" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCustomer(customer.CustomerId);
            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var returnedCustomer = Assert.IsType<Customer>(actionResult.Value);
            Assert.Equal(customer.FirstName, returnedCustomer.FirstName);
            Assert.Equal(customer.LastName, returnedCustomer.LastName);
        }


        [Fact]
        public async Task DeleteCustomer_RemovesCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var deletedCustomer = await _context.Customers.FindAsync(1);
            Assert.Null(deletedCustomer);
        }

        [Fact]
        public async Task PostCustomer_CreatesCustomer_WhenCustomerIsValid()
        {
            // Arrange
            var newCustomer = new Customer { FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _controller.PostCustomer(newCustomer);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedCustomer = Assert.IsType<Customer>(createdAtActionResult.Value);

            var customerInDb = await _context.Customers.FindAsync(returnedCustomer.CustomerId);
            Assert.NotNull(customerInDb);
            Assert.Equal(newCustomer.FirstName, customerInDb.FirstName);
            Assert.Equal(newCustomer.LastName, customerInDb.LastName);
        }



    }
}
