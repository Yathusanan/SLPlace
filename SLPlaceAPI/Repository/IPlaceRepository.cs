using SLPlaceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLPlaceAPI.Repository
{
    public interface IPlaceRepository
    {
        Task<IReadOnlyList<Place>> GetPlacesAsync();
        Task<Place> GetPlaceAync(int id);
        Task AddPlaceAync(Place place);
        Task<bool> PlaceNameExistAync(string name);
        Task<bool> PlaceExistAync(int id);
        void UpdatePlace(Place place);
        void DeletePlace(Place place);
        Task SaveAsync();

    }
}
