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

        [HttpGet("{placeId:int}")]
        public async Task<IActionResult> GetPlace(int placeId)
        {
            var placeFromDb = await _repository.GetPlaceAync(placeId);

            if (placeFromDb == null)
                return NotFound();
            
            var placeTorReturnDto = _map.Map<PlaceDto>(placeFromDb);

            return Ok(placeTorReturnDto);
        }
    }
}
