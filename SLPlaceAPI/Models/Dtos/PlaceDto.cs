using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLPlaceAPI.Models.Dtos
{
    public class PlaceDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string District { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
