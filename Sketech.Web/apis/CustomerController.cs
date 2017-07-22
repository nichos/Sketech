using Sketech.Entities;
using Sketech.Services;
using Sketech.Web.ActionFilters;
using Sketech.Web.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sketech.Web.apis
{
    [SkApiAuthorize]
    public class CustomerController : ApiController
    {
        [HttpGet]
        public string Ping()
        {
            var curUser = User.Identity.Name;
            return curUser;
        }

        [HttpGet]
        [AuditLog("GetCustomer", "test audit customer")]
        [SkApiAuthorize]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var service = new CustomerService();
            var response = await service.GetCustomers();

            return response.Value;
        }
    }
}
