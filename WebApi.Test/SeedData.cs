using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Test
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext context)
        {
            // Remove all
            // var savedCustomers = await context.Customers.ToListAsync();
            // Console.WriteLine(savedCustomers);
            // foreach (var savedCus in savedCustomers)
            // {
            //     context.Customers.Remove(savedCus);
            // }

            // await context.SaveChangesAsync();
            context.Customers.Add(new Customer { CustomerId = 0, Firstname = "Steve", Lastname = "Fox" });
            context.Customers.Add(new Customer { CustomerId = 0, Firstname = "Paul", Lastname = "Phoenix" });
            context.Customers.Add(new Customer { CustomerId = 0, Firstname = "Anna", Lastname = "Williams" });

            context.SaveChanges();
        }
    }
}