using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlsmatikBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        [HttpGet("DBData/{UserID}")]
        public async Task<ActionResult<List<object>>> DBConnectionTest(int UserID)
        {
            var handler = DbHandler.GetDbHandlerInstance();
            Debug.WriteLine(UserID);

            var data = handler.GetChosenParams(UserID, 3, 0); //2050 us

            return Ok(data);
        }

        [HttpGet("EnvVariableTest")]
        public async Task<ActionResult<string>> EnvVariableTest()
        {
            string envVariable = Environment.GetEnvironmentVariable("SQLPDHSDetails", EnvironmentVariableTarget.Machine).ToString();
            var variables = envVariable.Split(',');
            return Ok(variables[3].ToString());
        }
    }

}
