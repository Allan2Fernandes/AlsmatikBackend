using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlsmatikBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImageController : ControllerBase
    {
        [HttpGet(nameof(getImageOnID) + "/{ID}")]
        //[ResponseCache(Duration = 2592000, Location = ResponseCacheLocation.Any, NoStore = false)]

        public async Task<IActionResult> getImageOnID(int ID)
        {
            var handler = DbHandler.GetDbHandlerInstance();
            string query = $"SELECT RecipeValue FROM RecipeValue_STRING WHERE ID = {ID}";
            var fileDataAsBase64String = handler.ExecuteRawQuery(query)[0][0]["RecipeValue"].ToString().Trim();

            // Remove the prefix
            string prefix = "data:image/jpeg;base64,";
            fileDataAsBase64String = fileDataAsBase64String.Substring(prefix.Length);

            // Decode the base64-encoded image data
            byte[] imageBytes = Convert.FromBase64String(fileDataAsBase64String);

            // Determine the appropriate content type based on the image data (e.g., "image/jpeg")
            string contentType = "image/jpeg"; // Change based on your image type

            // Return the image as a file with the appropriate content type and headers
            return File(imageBytes, contentType);
        }


        [HttpGet(nameof(getImageOnFileName) + "/{FileName}")]
        //[ResponseCache(Duration = 2592000, Location = ResponseCacheLocation.Any, NoStore = false)]

        public async Task<IActionResult> getImageOnFileName(string FileName)
        {
            var handler = DbHandler.GetDbHandlerInstance();
            // Check this query. Max is being used without an order by
            string query = $"EXECUTE [dbo].[sp_GetImageEncodingOnName] @imageName = '{FileName}';";
            var fileDataAsBase64String = handler.ExecuteRawQuery(query)[0][0]["ParamValue"].ToString().Trim();

            // No need to remove the prefix this time

            // Decode the base64-encoded image data
            byte[] imageBytes = Convert.FromBase64String(fileDataAsBase64String);

            // Determine the appropriate content type based on the image data (e.g., "image/jpeg")
            string contentType = "";
            if(fileDataAsBase64String[0] == '/')
            {
                contentType = "image/jpeg";
            }else if (fileDataAsBase64String[0] == 'i')
            {
                contentType = "image/png";
            }         

            // Return the image as a file with the appropriate content type and headers
            return File(imageBytes, contentType);
        }

        [HttpGet(nameof(getReducedImageOnFileName) + "/{FileName}")]
        //[ResponseCache(Duration = 2592000, Location = ResponseCacheLocation.Any, NoStore = false)]

        public async Task<IActionResult> getReducedImageOnFileName(string FileName)
        {
            var handler = DbHandler.GetDbHandlerInstance();
            string query = $"SELECT ParamValue FROM [File_STRING] WHERE SetID = (SELECT MAX(SetID) FROM [File_STRING] WHERE ParamValue = '{FileName}' GROUP BY ParamValue) AND ParamID = 35010";
            var fileDataAsBase64String = handler.ExecuteRawQuery(query)[0][0]["ParamValue"].ToString().Trim();

            // No need to remove the prefix this time

            // Decode the base64-encoded image data
            byte[] imageBytes = Convert.FromBase64String(fileDataAsBase64String);

            // Determine the appropriate content type based on the image data (e.g., "image/jpeg")
            string contentType = "image/jpeg"; // Change based on your image type

            // Return the image as a file with the appropriate content type and headers
            return File(imageBytes, contentType);
        }

    }
}
