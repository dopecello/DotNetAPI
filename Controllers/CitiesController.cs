﻿using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]  // this isn't strictly necessary, but it's a good idea to include it in order to improve performance
    [Route("api/cities")]
    public class CitiesController: ControllerBase
    {
        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(
                new List<object>
                {
                    new { id = 1, Name = "New York City" },
                    new { id = 2, Name = "Baltimore" }
                });
        }
    }
}
