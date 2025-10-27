using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Models;
using RentMaster.Accounts.Services;
using RentMaster.Core.Controllers;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class ConsumerController : BaseController<Consumer>
    {
        public ConsumerController(ConsumerService service) 
            : base(service)
        {
        }
    }
}