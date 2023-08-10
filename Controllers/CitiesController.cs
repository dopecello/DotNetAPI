using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]  // this isn't strictly necessary, but it's a good idea to include it in order to improve performance
    [Route("api/cities")]
    public class CitiesController: ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var CityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (CityToReturn == null)
            {
                return NotFound();
            }
            return Ok(CityToReturn);
        }
    }
}
