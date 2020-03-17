using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Contracts;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Test.FakeRepositories
{
    public class CustomerRepoFake : ICustomerRepo
    {
        private readonly List<Customer> customers;

        public CustomerRepoFake()
        {
            customers = new List<Customer>{
                new Customer { CustomerId = 1, Firstname = "Jessica", Lastname = "Santos"},
                new Customer { CustomerId = 2, Firstname = "Thompson", Lastname = "Lee"},
                new Customer { CustomerId = 3, Firstname = "Appos", Lastname = "LynÎ"},
            };
        }

        public async Task<bool> Create(Customer customer)
        {
            try
            {
                if (!IsStringIsNotNull(customer))
                    return false;

                customers.Add(customer);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> Update(Customer customer)
        {
            try
            {
                if (!IsStringIsNotNull(customer))
                    return false;
                // fake update
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<Customer> CustomerById(int id)
        {
            var customer = customers.Find(x => x.CustomerId == id);
            return await Task.FromResult(customer);
        }

        public async Task<List<Customer>> Customers()
        {
            return await Task.FromResult(customers);
        }

        public async Task<bool> DeleteById(int id)
        {
            var customer = await CustomerById(id);
            customers.Remove(customer);
            return await Task.FromResult(true);
        }

        private bool IsStringIsNotNull(Customer customer)
        {
            if (customer.Firstname.IsStringNullOrEmpty())
                return false;

            if (customer.Lastname.IsStringNullOrEmpty())
                return false;

            return true;
        }

        public async Task<bool> IsCustomerIdExists(int id)
        {
            return await Task.FromResult(customers.Any(x => x.CustomerId == id));
        }
    }
}