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
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WithListOfOrders()
        {
            var orders = new List<Order> { new Order { Id = 1, CustomerId = 1 } };
            _mockOrderService.Setup(service => service.GetOrdersAsync()).ReturnsAsync(orders);

            var result = await _controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetOrder_ReturnsOkResult_WithOrder()
        {
            var order = new Order { Id = 1, CustomerId = 1 };
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);

            var result = await _controller.GetOrder(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFoundResult_WhenOrderNotFound()
        {
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            var result = await _controller.GetOrder(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult_WithOrder()
        {
            var order = new Order { Id = 1, CustomerId = 1 };
            _mockOrderService.Setup(service => service.CreateOrderAsync(order)).ReturnsAsync(order);

            var result = await _controller.CreateOrder(order);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsNoContentResult()
        {
            var order = new Order { Id = 1, CustomerId = 1 };
            _mockOrderService.Setup(service => service.UpdateOrderAsync(order)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateOrder(1, order);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsBadRequest_WhenIdMismatch()
        {
            var order = new Order { Id = 1, CustomerId = 1 };

            var result = await _controller.UpdateOrder(2, order);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContentResult()
        {
            _mockOrderService.Setup(service => service.DeleteOrderAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteOrder(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}