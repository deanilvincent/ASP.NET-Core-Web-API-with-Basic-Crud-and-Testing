using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Data;
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

        public async Task<IEnumerable> Customers()
        {
            return await context.Customers.ToListAsync();
        }

        public async Task<Customer> CustomerById(int id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<bool> Create(Customer customer)
        {
            try
            {
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
                context.Customers.Remove(customer);
                await SaveChanges();
                return true;
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
    }
}