using Sketech.Dals.Extensions;
using Sketech.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sketech.Dals.Repositories
{
    public class CustomerRepository : RepositoryBase
    {
        public CustomerRepository(SkRepositorySession session) : base(session)
        {
            
        }

        private async Task<Customer> ConvertToCustomer(SqlDataReader data)
        {
            return new Customer
            {
                Id = await data.GetValueAsync<Guid>("CustomerId"),
                Firstname = await data.GetValueAsync<string>("Firstname"),
                Lastname = await data.GetValueAsync<string>("Lastname")
            };
        }

        public async Task<DataAccessResult<IEnumerable<Customer>>> GetCustomers()
        {
            var query = new StringBuilder();
            query.Append(@"SELECT CustomerId, Firstname, Lastname FROM dbo.vwCustomer");
            query.Append(" ORDER BY Firstname, LastName");

            var response = await SelectDbViewAsync(query.ToString(), ConvertToCustomer);
            var data = response.ToList();
            var result = new DataAccessResult<IEnumerable<Customer>>
            {
                Value = data.ToList(),
                TotalResultCount = data.Count
            };

            return result;
        }
    }
}
