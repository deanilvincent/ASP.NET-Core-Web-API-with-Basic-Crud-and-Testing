using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Models;

namespace WebApi.Test.FakeRepositories
{
    public class CustomerRepoFake : ICustomerRepo
    {
        private readonly IList<Customer> customers;

        public CustomerRepoFake()
        {
            customers = new List<Customer>{
                new Customer { CustomerId = 1, Firstname =}
            }
        }

        public Task<bool> Create(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Task<Customer> CustomerById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable> Customers()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}