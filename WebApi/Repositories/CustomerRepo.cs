using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Data;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext context;
        public CustomerRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Customer>> Customers()
        {
            return await context.Customers.ToListAsync();
        }

        public async Task<Customer> CustomerById(int id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<bool> IsCustomerIdExists(int id)
        {
            return await context.Customers.AnyAsync(x => x.CustomerId == id);
        }

        public async Task<bool> Create(Customer customer)
        {
            try
            {
                if (!IsStringIsNotNull(customer))
                    return false;

                await context.Customers.AddAsync(customer);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> Update(Customer customer)
        {
            try
            {
                if (!IsStringIsNotNull(customer))
                    return false;
                    
                context.Customers.Update(customer);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var customer = await CustomerById(id);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    await SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        private async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        private bool IsStringIsNotNull(Customer customer)
        {
            if (customer.Firstname.IsStringNullOrEmpty())
                return false;

            if (customer.Lastname.IsStringNullOrEmpty())
                return false;

            return true;
        }
    }
}