

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Yash.BusinessLogicExtractor;

namespace YashCustomToolRitesh
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class YashCustomToolController : ControllerBase

    {


        private readonly IConfiguration _configuration;

        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<YashCustomToolController> _logger;

        //public YashCustomToolController(IOptions<AppSettings> settings)
        //{
        //    _appSettings = settings;
        //}



        public YashCustomToolController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("GetForecastDetails")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]

            })
            .ToArray();
        }

        [HttpGet("GetProjectDetail")]
        public async Task<IActionResult> GetProjectDetail()
        {
            AIClass aIClass = new AIClass();

            string projectLocation = _configuration["ProjectLocation"].ToString();

            var fileContent = await aIClass.ProcessCodeFiles(projectLocation);
            //aIClass.SaveDocuemnt(FileContent, "YashCustomTool_");


     

            // Convert string to byte array
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            // Set file name and content type
            string fileName = "Yash_CustomTools_Result_" + DateTime.Now.ToString("yyyyMMdd") + ".md"; 
            string contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);

            //return null;
        }


        //[HttpGet("download")]
        //public IActionResult DownloadFile(string fileName)
        //{
        //    // Path to the file
        //    var filePath = Path.Combine(_env.ContentRootPath, "Files", fileName);

        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound("File not found.");

        //    var bytes = System.IO.File.ReadAllBytes(filePath);
        //    var contentType = "application/octet-stream"; // or use MimeTypes if needed

        //    return File(bytes, contentType, fileName);
        //}


        public class WeatherForecast
        {
            public DateTime Date { get; set; }

            public int TemperatureC { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

            public string Summary { get; set; }
        }


        [HttpGet("GetUserData")]
        public List<string> GetUser()
        {
            List<string> User = new List<string>();

            User.Add("Yash");
            User.Add("Pratik");
            User.Add("Ankit");


            return User;
        }


    }
}