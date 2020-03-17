using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Contracts;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Test.FakeRepositories;
using Xunit;

namespace WebApi.Test.Controllers.Tests
{
    public class CustomersControllerUnitTest
    {
        CustomersController controller;
        ICustomerRepo customerRepo;
        public CustomersControllerUnitTest()
        {
            customerRepo = new CustomerRepoFake();
            controller = new CustomersController(customerRepo);
        }

        [Fact]
        public void GetMethod_WhenCalled_ReturnOkResult()
        {
            // arrange

            // act
            var okResult = controller.Get();

            // assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async Task GetMethod_WhenCalled_CheckIfListAreMatch()
        {
            // arrange
            var mockRepo = new Mock<ICustomerRepo>();
            mockRepo.Setup(repo => repo.Customers()).Returns(Customers());
            var controller = new CustomersController(mockRepo.Object);

            var expected = await Task.FromResult(new List<Customer>(){
                new Customer{ CustomerId=1, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=2, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=3, Firstname = "asdfasdf", Lastname = "asdfasdfa"}
            });

            // act
            var result = await controller.Get();

            // assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Customer>>(viewResult.Value);
            expected.Should().BeEquivalentTo(model);
        }

        [Fact]
        public async Task GetMethod_CountCustomersAndEqualToEachOther_ReturnsTrue()
        {
            // arrange
            var mockRepo = new Mock<ICustomerRepo>();
            mockRepo.Setup(repo => repo.Customers()).Returns(Customers());

            var controller = new CustomersController(mockRepo.Object);

            // act
            var result = await controller.Get();
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Customer>>(viewResult.Value);
            // assert
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public void GetByIdMethod_CheckIfCustomerIsNull_ReturnNotFound()
        {
            // arrange

            // act
            var notFoundResult = controller.GetById(4);

            // assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetById_MatchCustomerByIdWhenItsExists_ReturnEqual(int id)
        {
            // arrange
            var mockRepo = new Mock<ICustomerRepo>();
            mockRepo.Setup(repo => repo.CustomerById(id)).Returns(Customer());
            var controller = new CustomersController(mockRepo.Object);
            // act
            var result = await controller.GetById(id);

            // assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Customer>(viewResult.Value);
            Assert.Equal(1, model.CustomerId);
        }

        [Fact]
        public void GetByIdMethod_IfCustomerIsNotNull_ReturnOkResult()
        {
            // arrange

            // act
            var okResult = controller.GetById(3);
            // assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void CreateMethod_CheckIfCreateIsNOTSuccessfull_ReturnBadRequest()
        {
            // arrange
            var customer1 = new Customer { CustomerId = 1, Lastname = "asdf" };
            var customer2 = new Customer { CustomerId = 1, Firstname = "asdf" };

            // act
            var badRequest1 = controller.Create(customer1);
            var badRequest2 = controller.Create(customer2);

            // assert
            Assert.IsType<BadRequestResult>(badRequest1.Result);
            Assert.IsType<BadRequestResult>(badRequest2.Result);
        }

        [Fact]
        public void CreateMethod_CheckIfCreateIsSuccessfully_ReturnOkResult()
        {
            // arrange
            var customer = new Customer { CustomerId = 1, Firstname = "asdf", Lastname = "asdfasdf" };
            // act
            var okResult = controller.Create(customer);

            // assert
            Assert.IsType<OkResult>(okResult.Result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Update_CheckIfIdIsZeroAndIfIdExists_ReturnNotFoundResult(int id)
        {
            // arrange
            var customer = new Customer { CustomerId = 1, Firstname = "asdf", Lastname = "asdfasdf" };

            // act
            var notFoundResult = controller.Update(id, customer);

            // assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Theory]
        [InlineData(1)]
        public void UpdateMethod_CheckIfUpdateIsNOTSuccessfull_ReturnBadRequest(int id)
        {
            // arrange
            var customer1 = new Customer { CustomerId = 1, Lastname = "asdf" };
            var customer2 = new Customer { CustomerId = 1, Firstname = "asdf" };

            // act
            var badRequest1 = controller.Update(id, customer1);
            var badRequest2 = controller.Update(id, customer2);

            // assert
            Assert.IsType<BadRequestResult>(badRequest1.Result);
            Assert.IsType<BadRequestResult>(badRequest2.Result);
        }

        [Theory]
        [InlineData(1)]
        public void UpdateMethod_CheckIfUpdateIsSuccessfully_ReturnOkResult(int id)
        {
            // arrange
            var customer = new Customer { CustomerId = 1, Firstname = "asdf", Lastname = "asdfasdf" };
            // act
            var okResult = controller.Update(id, customer);

            // assert
            Assert.IsType<OkResult>(okResult.Result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void DeleteMethod_CheckIfIdIsZeroAndIfIdExists_ReturnNotFoundResult(int id)
        {
            // arrange

            // act
            var notFoundResult = controller.DeleteById(id);

            // assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Theory]
        [InlineData(1)]
        public void DeleteMethod_CheckIfUpdateIsSuccessfully_ReturnOkResult(int id)
        {
            // arrange

            // act
            var okResult = controller.DeleteById(id);

            // assert
            Assert.IsType<OkResult>(okResult.Result);
        }

        private async Task<List<Customer>> Customers()
        {
            var customers = new List<Customer>(){
                new Customer{ CustomerId=1, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=2, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=3, Firstname = "asdfasdf", Lastname = "asdfasdfa"}
            };

            return await Task.FromResult(customers);
        }

        private async Task<Customer> Customer()
        {
            var customers = new List<Customer>(){
                new Customer{ CustomerId=1, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=2, Firstname = "asdfasdf", Lastname = "asdfasdfa"},
                new Customer{ CustomerId=3, Firstname = "asdfasdf", Lastname = "asdfasdfa"}
            };

            return await Task.FromResult(customers.FirstOrDefault());
        }
    }
}