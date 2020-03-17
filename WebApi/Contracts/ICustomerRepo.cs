using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contracts
{
    public interface ICustomerRepo
    {
         Task<List<Customer>> Customers();
         Task<Customer> CustomerById(int id);

         Task<bool> IsCustomerIdExists(int id);
         Task<bool> Create(Customer customer);
         Task<bool> Update(Customer customer);
         Task<bool> DeleteById(int id);
    }
}