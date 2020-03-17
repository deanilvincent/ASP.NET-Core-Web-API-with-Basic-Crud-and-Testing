using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using WebApi.Models;
using Xunit;

namespace WebApi.Test.Controllers.Tests
{
    public class CustomersControllerIntegrationTest : IClassFixture<CustomWebAppFactory<Startup>>
    {
        private HttpClient client { get; set; }
        private HttpContent httpContent;

        private const string customerWebApi = "/api/customers/";
        public CustomersControllerIntegrationTest(CustomWebAppFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task GetMethod_GetCustomers()
        {
            // arrange

            // act
            var response = await client.GetAsync(customerWebApi);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetMethod_GetCustomerById()
        {
            // arrange

            // act
            var response = await client.GetAsync($"{customerWebApi}{2}");
            var responseData = await response.Content.ReadAsStringAsync();
            // assert
            var model = JsonConvert.DeserializeObject<Customer>(responseData);
            Assert.Equal(2, model.CustomerId);
        }

        [Fact]
        public async Task CreateMethod_WhenCalledInsertData_ReturnTRUEStatusCode()
        {
            // arrange
            var customer = new Customer { CustomerId = 0, Firstname = "Jeje", Lastname = "Monster" };

            // act
            var json = JsonConvert.SerializeObject(customer);
            httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await client.PostAsync(customerWebApi, httpContent);

            // assert
            result.EnsureSuccessStatusCode();
            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task CreateMethod_WhenCalledInsertDataFails_ReturnFALSEStatusCode()
        {
            // arrange
            var customer1 = new Customer { CustomerId = 22, Lastname = "Monster" };
            var customer2 = new Customer { CustomerId = 22, Firstname = "Jerjer" };

            // act
            var json = JsonConvert.SerializeObject(customer1);

            httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await client.PostAsync(customerWebApi, httpContent);

            // Assert
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task UpdateMethod_WhenCalledUpdateData_ReturnTrueStatusCode()
        {
            // arrange
            var customerId = 2;
            var customer = new Customer { CustomerId = customerId, Firstname = "UpdatedFirstname", Lastname = "UpdatedLastname" };

            var json = JsonConvert.SerializeObject(customer);
            httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // act
            var result = await client.PutAsync($"{customerWebApi}{customerId}", httpContent);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task UpdateMethod_WhenCalledData_ReturnNotFoundHttpCodeIfIdDoesNotExists()
        {
            // arrange
            var customerId = -1;
            var customer = new Customer { CustomerId = customerId, Firstname = "asdfasdf", Lastname = "asdfasdfa"};
            var json = JsonConvert.SerializeObject(customer);
            httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // act
            var response = await client.PutAsync($"{customerWebApi}{customerId}", httpContent);
        
            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateMethod_WhenCalledUpdateData_ReturnFALSEStatusCodeIfError()
        {
            // arrange
            var customerId = 2;
            var customer = new Customer { CustomerId = customerId, Firstname = "UpdatedFirstname" };

            var json = JsonConvert.SerializeObject(customer);
            httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // act
            var result = await client.PutAsync($"{customerWebApi}{customerId}", httpContent);
            // assert
            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        // ########################### REMOVE to avoid conflict in all operations #############################
        // [Fact]
        // public async Task DeleteMethod_WhenCalledDeleteData_ReturnTrueSuccessStatusCode()
        // {
        //     // arrange
        //     var customerId = 1;
        //     // act
        //     var result = await client.DeleteAsync($"{customerWebApi}{customerId}");

        //     // assert
        //     Assert.True(result.IsSuccessStatusCode);
        // }

        [Fact]
        public async Task DeleteMethod_WhenCalledDeleteData_ReturnFalseFailedStatusCode()
        {
            // arrange
            var customerId = -1;
            // act
            var result = await client.DeleteAsync($"{customerWebApi}{customerId}");

            // assert
            Assert.False(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteMethod_WhenCalledDeleteData_ReturnNotFound()
        {
            // arrange
            var customerId = -1;
            // act
            var result = await client.DeleteAsync($"{customerWebApi}{customerId}");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}