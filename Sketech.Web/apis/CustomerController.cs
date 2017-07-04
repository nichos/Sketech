using Sketech.Entities;
using Sketech.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sketech.Web.apis
{
    [Authorize]
    public class CustomerController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            var curUser = User.Identity.Name;
            return curUser;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var service = new CustomerService();
            return await service.GetCustomers();
        }
    }
}
