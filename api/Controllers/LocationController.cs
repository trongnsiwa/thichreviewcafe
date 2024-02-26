using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Location;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    /// <summary>
    /// Manage locations
    /// </summary>
    [ApiController]
    [Route("api/location")]
    [SwaggerTag("Manage locations")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepo;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// LocationController constructor
        /// </summary>
        /// <param name="locationRepo"></param>
        /// <param name="logger"></param>
        public LocationController(ILocationRepository locationRepo, ILoggerManager logger)
        {
            _locationRepo = locationRepo;
            _logger = logger;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <param name="query">included isParent query object</param>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] LocationQueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var locations = await _locationRepo.GetAllAsync(query);
            var locationDto = locations.Select(l => l.ToLocationDto());
            return Ok(locations);
        }

        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id">Location id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var location = await _locationRepo.GetByIdAsync(id);

            if (location == null) return NotFound("Location not found");

            return Ok(location.ToLocationDto());
        }

        /// <summary>
        /// Add a child location to a parent location
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("parent/{parentId:int}")]
        public async Task<IActionResult> AddChildToParent([FromRoute] int parentId, [FromBody] CreateLocationDto locationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var locationModel = locationDto.ToCommentFromCreate();

            var location = await _locationRepo.AddChildToParentAsync(parentId, locationModel);

            if (location == null) return NotFound("Parent location not found");

            return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created location</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationDto locationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var locationModel = locationDto.ToCommentFromCreate();

            await _locationRepo.CreateAsync(locationModel);

            return CreatedAtAction(nameof(GetById), new { id = locationModel.Id }, locationModel);
        }

        //TODO - Add update and delete methods
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLocationDto locationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var locationModel = locationDto.ToLocationFromUpdate();

            var location = await _locationRepo.UpdateAsync(id, locationModel);

            if (location == null) return NotFound("Location not found");

            return Ok(location);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var location = await _locationRepo.DeleteAsync(id);

            if (location == null) return NotFound("Location not found");

            return Ok(location);
        }
    }
}