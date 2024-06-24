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
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Test Product", Price = 10.0M } };
            _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync(products);

            var result = await _controller.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.0M };
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.GetProduct(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFoundResult_WhenProductNotFound()
        {
            _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            var result = await _controller.GetProduct(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtActionResult_WithProduct()
        {
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.0M };
            _mockProductService.Setup(service => service.CreateProductAsync(product)).ReturnsAsync(product);

            var result = await _controller.CreateProduct(product);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContentResult()
        {
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.0M };
            _mockProductService.Setup(service => service.UpdateProductAsync(product)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateProduct(1, product);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdMismatch()
        {
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.0M };

            var result = await _controller.UpdateProduct(2, product);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContentResult()
        {
            _mockProductService.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}