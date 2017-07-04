using System.Collections;
using System.Threading.Tasks;
using Sketech.Dals.Repositories;
using System.Collections.Generic;
using Sketech.Entities;

namespace Sketech.Services
{
    public class CustomerService : ServiceBase
    {
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            using(var session = GetRepositorySession())
            {
                var repo = new CustomerRepository(session);
                var data = await repo.GetCustomers();

                return data.Value;
            }
        }
    }
}
