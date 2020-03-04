using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Controllers
{
    /// <summary>
    /// Temporary controller for InTechNet API
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {

        /// <summary>
        /// Dummy "Hello World" endpoint
        /// </summary>
        /// <returns>"Hello World !" raw string</returns>
        [HttpGet]
        public ActionResult<string> HelloWorld()
        {
            return Ok("Hello World !");
        }
    }
}