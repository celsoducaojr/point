using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Point.API.Controllers.Base
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase { }
}
