using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLPlaceAPI.Models;
using SLPlaceAPI.Models.Dtos;
using SLPlaceAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLPlaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceRepository _repository;
        private readonly IMapper _map;

        public PlacesController(IPlaceRepository repo, IMapper map)
        {
            _repository = repo;
            _map = map;
        }

        /// <summary>
        /// Get List Of Places
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(201, Type = typeof(List<PlaceDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPalces()
        {
            var placesToReturn = new List<PlaceDto>();

            var placesFromDb = await _repository.GetPlacesAsync();

            foreach(var place in placesFromDb)
            {
                placesToReturn.Add(_map.Map<PlaceDto>(place));
            }

            return Ok(placesToReturn);
        }


        /// <summary>
        /// Get Individual Place
        /// </summary>
        /// <param name="placeId"> The Id of a Place</param>
        /// <returns></returns>
        [HttpGet("{placeId:int}", Name = "GetPlace")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PlaceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlace(int placeId)
        {
            var placeFromDb = await _repository.GetPlaceAync(placeId);

            if (placeFromDb == null)
                return NotFound();
            
            var placeTorReturnDto = _map.Map<PlaceDto>(placeFromDb);

            return Ok(placeTorReturnDto);
        }


        /// <summary>
        /// Create New Place
        /// </summary>
        /// <param name="placeDto">Properties Of A Specific place</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromBody]PlaceDto placeDto)
        {
            if (placeDto == null)
                return BadRequest(ModelState);

            var existPlaceName = await _repository.PlaceNameExistAync(placeDto.Name);

            if (existPlaceName)
            {
                ModelState.AddModelError("", "This Place Name Is Already Exist!");
                return StatusCode(404, ModelState);
            }

            var place = _map.Map<Place>(placeDto);

            await _repository.AddPlaceAync(place);

            return CreatedAtRoute("GetPlace", new { placeId = place.Id }, place);

        }

        /// <summary>
        /// Update A specific Place
        /// </summary>
        /// <param name="placeId">Id of a Place</param>
        /// <param name="placeDto">New Properties Of A Specific place</param>
        /// <returns></returns>
        [HttpPatch("{placeId:int}", Name = "UpdatePlace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePlace(int placeId, [FromBody]PlaceDto placeDto)
        {
            if (placeDto == null || placeId != placeDto.Id)
                return BadRequest(ModelState);

            var place = _map.Map<Place>(placeDto);

             _repository.UpdatePlace(place);

            return Ok("Place Detail Updated!");
        }


        /// <summary>
        /// Delete a specific Place
        /// </summary>
        /// <param name="placeId">Id of a place</param>
        /// <returns></returns>
        [HttpDelete("{placeId:int}", Name = "DeletePlace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlace(int placeId)
        {
            if (placeId == 0)
                return BadRequest(ModelState);

            var placeFromDb = await _repository.GetPlaceAync(placeId);

            if (placeFromDb == null)
                return NotFound();

            _repository.DeletePlace(placeFromDb);

            return Ok("Place Deleted!");
        }
    }
}
