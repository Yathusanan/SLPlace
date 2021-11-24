using AutoMapper;
using SLPlaceAPI.Models;
using SLPlaceAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLPlaceAPI.Mapping
{
    public class SLPMapper : Profile
    {
        public SLPMapper()
        {
            CreateMap<Place, PlaceDto>().ReverseMap();
        }
    }
}
