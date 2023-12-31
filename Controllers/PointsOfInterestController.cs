﻿using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        } // despite ASP.NET Core's built in container, we can still use anything we want, so the null check is still valid.

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"The city with the ID {cityId} wasn't found when accessing points of interest."); // logging the 404 response.
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            } catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
                // the importance here is that without logging, it would throw a 'Status 500 - Internal Server Error'
                // never return stack trace in a production environment.
            }
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetSinglePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return NotFound();
            }
            return Ok(poi);
        }
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            // find highest possibe ID number first.
            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointsOfInterest.Add(finalPointOfInterest);

            //for Post requests, Coded 201-created is recommended.
            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointOfInterestId = finalPointOfInterest.Id
            }, finalPointOfInterest
            );
        }

        [HttpPut("{pointofinterestid}")]

        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            // account for null values
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }
            // update the fields
            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            // recommended in this case to return a 204 No Content message to the user.
            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            // transfrom the pointOfInterestFromStore to a pointOfInterestForUpdateDto

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState); // ModelState here checks for validation errors on the POI that we are patching.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // returns 402
            }

            // prevents changes on what was previous valid model state through PointOfInterestForUpdateDto, checks if modelstate is valid AFTER applying the patchDocument
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            // once everything is fine, patch.

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]

        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            _mailService.Send(
               "Point of interest deleted.",
               $"Point of interest {pointOfInterestFromStore.Name} with ID {pointOfInterestFromStore.Id} was removed!");
            return NoContent();
        }
    }
}
