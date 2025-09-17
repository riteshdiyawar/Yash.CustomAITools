
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;


namespace Identity.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase

    {


        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<UsersController> _logger;
      
        public UsersController()
        {
            //_loggerService = loggerService;

            //_appSettings = appSettings;
            //_logger = logger;

            //_UsersBL = usersBL;
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