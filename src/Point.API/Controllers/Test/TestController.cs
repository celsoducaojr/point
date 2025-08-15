using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Base;
using Point.Core.Application.Contracts;

namespace Point.API.Controllers.Test
{
    [Route("api/v{version:apiversion}/test")]
    public class TestController(IPointDbContext pointDbContext) : BaseController
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpGet("live")]
        public IActionResult IsLive()
        {
            return Ok(new { Status = "live" });
        }

        [HttpGet("ready")]
        public IActionResult IsReady()
        {
            try
            {
                var context = _pointDbContext as DbContext;
                return Ok(new { DatabaseConnected = context.Database.CanConnect() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { DatabaseConnected = false, Error = ex.Message });
            }
        }
    }
}
