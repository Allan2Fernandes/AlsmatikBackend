using AlsmatikBackend.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlsmatikBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {


        [HttpGet(nameof(ControllerIsOnline))]
        public async Task<ActionResult<string>> ControllerIsOnline()
        {
            return Ok(new {Result = "Online"});
        }

        [HttpGet(nameof(ExecuteRawQuery) + "/{RawQuery}")]
        public async Task<ActionResult<List<object>>> ExecuteRawQuery(string RawQuery)
        {
            var handler = DbHandler.GetDbHandlerInstance();

            var data = handler.ExecuteRawQuery(RawQuery);

            return Ok(data);
        }

   

        [HttpPost(nameof(ExecuteRawQueryFromBody))]
        public async Task<ActionResult<List<object>>> ExecuteRawQueryFromBody(BodyQuery BodyQuery)
        {
            //Get an instance of the Db Handler
            var handler = DbHandler.GetDbHandlerInstance();
            //Execute the raw query
            var data = handler.ExecuteRawQuery(BodyQuery.Query);
            //Return the data
            return Ok(data);
        }

    }

}
 