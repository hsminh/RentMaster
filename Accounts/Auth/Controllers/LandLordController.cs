using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Models;
using RentMaster.Accounts.Services;
using RentMaster.Core.Controllers;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class LandLordController : BaseController<LandLord>
    {
        public LandLordController(LandLordService service) 
            : base(service)
        {
        }
    }
}