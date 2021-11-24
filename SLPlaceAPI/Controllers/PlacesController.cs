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
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceRepository _repository;
        private readonly IMapper _map;

        public PlacesController(IPlaceRepository repo, IMapper map)
        {
            _repository = repo;
            _map = map;
        }

        [HttpGet]
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

        [HttpGet("{placeId:int}", Name = "GetPlace")]
        public async Task<IActionResult> GetPlace(int placeId)
        {
            var placeFromDb = await _repository.GetPlaceAync(placeId);

            if (placeFromDb == null)
                return NotFound();
            
            var placeTorReturnDto = _map.Map<PlaceDto>(placeFromDb);

            return Ok(placeTorReturnDto);
        }

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

        [HttpPatch("{placeId:int}", Name = "UpdatePlace")]
        public IActionResult UpdatePlace(int placeId, [FromBody]PlaceDto placeDto)
        {
            if (placeDto == null || placeId != placeDto.Id)
                return BadRequest(ModelState);

            var place = _map.Map<Place>(placeDto);

             _repository.UpdatePlace(place);

            return Ok("Place Detail Updated!");
        }

        [HttpDelete("{placeId:int}", Name = "DeletePlace")]
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
