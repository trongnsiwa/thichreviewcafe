using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILoggerManager _logger;
        public LocationRepository(ApplicationDBContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Location?> AddChildToParentAsync(int parentId, Location locationModel)
        {
            var parent = await _context.Locations.Include(l => l.ChildLocations).FirstOrDefaultAsync(l => l.Id == parentId);

            if (parent == null)
            {
                _logger.LogInfo($"Parent Location with id: {parentId} not found");
                return null;
            }

            parent.ChildLocations ??= new List<Location>();
            parent.ChildLocations.Add(locationModel);

            await _context.SaveChangesAsync();

            return locationModel;
        }

        public async Task<Location> CreateAsync(Location locationModel)
        {
            await _context.Locations.AddAsync(locationModel);
            await _context.SaveChangesAsync();
            return locationModel;
        }

        public async Task<Location?> DeleteAsync(int id)
        {
            var locationModel = await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);

            if (locationModel == null)
            {
                _logger.LogInfo($"Location with id: {id} not found");
                return null;
            }

            _context.Locations.Remove(locationModel);
            await _context.SaveChangesAsync();
            return locationModel;
        }

        public async Task<List<Location>> GetAllAsync(LocationQueryObject query)
        {
            var locations = _context.Locations.Include(l => l.ChildLocations).AsQueryable();

            if (query.IsParent)
            {
                locations = locations.Where(l => l.ChildLocations.Count > 0);
            }

            return await locations.ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations.Include(l => l.ChildLocations).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Location>> GetChildsAsync(int parentId)
        {
            var parentLocation = await _context.Locations.Include(l => l.ChildLocations).FirstOrDefaultAsync(l => l.Id == parentId);

            if (parentLocation == null)
            {
                _logger.LogInfo($"Location with id: {parentId} not found");
                return new List<Location>();
            }

            return parentLocation.ChildLocations ?? new List<Location>();
        }

        public async Task<Location?> UpdateAsync(int id, Location locationModel)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                _logger.LogInfo($"Location with id: {id} not found");
                return null;
            }

            _context.Entry(location).CurrentValues.SetValues(locationModel);

            await _context.SaveChangesAsync();

            return location;
        }
    }
}