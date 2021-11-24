using Microsoft.EntityFrameworkCore;
using SLPlaceAPI.Data;
using SLPlaceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLPlaceAPI.Repository
{
    public class PlaceRespository : IPlaceRepository
    {
        private readonly ApplicationDBContext _context;

        public PlaceRespository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddPlaceAync(Place place)
        {
            await _context.Places.AddAsync(place);
            await SaveAsync();
        }

        public async Task<Place> GetPlaceAync(int id)
        {
            return await _context.Places.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IReadOnlyList<Place>> GetPlacesAsync()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<bool> PlaceExistAync(int id)
        {
            var place =  await _context.Places.FirstOrDefaultAsync(c => c.Id == id);

            if (place == null)
                return false;

            return true;

        }

        public async Task<bool> PlaceNameExistAync(string name)
        {
            var value = await _context.Places.AnyAsync(a => a.Name.ToLower() == name.ToLower().Trim());

            return value;
        }

        public void UpdatePlace(Place place)
        {
             _context.Places.Update(place);
            Save();

        }

        public void DeletePlace(Place place)
        {
            _context.Places.Remove(place);
            Save();

        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private void Save()
        {
            _context.SaveChanges();
        }
    }
}
