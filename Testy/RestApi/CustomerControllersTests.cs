using Microsoft.AspNetCore.Mvc;
using Moq;
using RestApi.Controllers;
using RestApi.Models;
using RestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy.RestApi
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            var customers = new List<Customer> { new Customer { Id = 1, Name = "Test Customer" } };
            _mockCustomerService.Setup(service => service.GetCustomersAsync()).ReturnsAsync(customers);

            var result = await _controller.GetCustomers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult_WithCustomer()
        {
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockCustomerService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

            var result = await _controller.GetCustomer(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNotFoundResult_WhenCustomerNotFound()
        {
            _mockCustomerService.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync((Customer)null);

            var result = await _controller.GetCustomer(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtActionResult_WithCustomer()
        {
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockCustomerService.Setup(service => service.CreateCustomerAsync(customer)).ReturnsAsync(customer);

            var result = await _controller.CreateCustomer(customer);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsNoContentResult()
        {
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockCustomerService.Setup(service => service.UpdateCustomerAsync(customer)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCustomer(1, customer);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsBadRequest_WhenIdMismatch()
        {
            var customer = new Customer { Id = 1, Name = "Test Customer" };

            var result = await _controller.UpdateCustomer(2, customer);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContentResult()
        {
            _mockCustomerService.Setup(service => service.DeleteCustomerAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCustomer(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}