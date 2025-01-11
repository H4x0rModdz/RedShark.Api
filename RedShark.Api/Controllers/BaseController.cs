using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace RedShark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Consumes("application/json")]
//[Produces("application/json")]
//[EnableCors("AllowAll")]
public abstract class BaseController : ControllerBase
{
    protected ActionResult<TResult> OkOrNotFound<TResult>(TResult result)
        => result is null ? NotFound() : Ok(result);
}