using Amongus.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Amongus.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected AmongusContext Context { get; set; }
        protected BaseApiController(AmongusContext context)
        {
            Context = context;
        }
    }
}