using Microsoft.AspNetCore.Mvc;

namespace Enxaquecapp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CausesController : ApiControllerBase
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <returns>
        /// OK
        /// </returns>
        [HttpGet]
        public ActionResult TestAsync()
            => Ok();
    }
}