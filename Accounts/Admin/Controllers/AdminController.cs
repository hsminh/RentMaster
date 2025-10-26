using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Services;
using RentMaster.Core.Middleware;

namespace RentMaster.Accounts.Admin.Controllers;

[ApiController]
[Attributes.AdminScope]
[Route("[controller]/api")]
public class AdminController : ControllerBase
{
    private readonly AdminService _service;
    private readonly LandLordService _landlordService;
    public AdminController(AdminService service, LandLordService landLordService)
    {
        _service = service;
        _landlordService = landLordService;
    }
    
    [HttpGet("landlords")]
    public async Task<IActionResult> GetAllLandlords()
    {
        var user = HttpContext.Items["CurrentUser"] as Models.Admin;

        // Trả về danh sách LandLord (không phải 1 object)
        var landlords = await _landlordService.GetAllAsync();

        return Ok(landlords);
    }

}