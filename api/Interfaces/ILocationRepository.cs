using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ILocationRepository
    {
        // LIST ALL LOCATIONS
        Task<List<Location>> GetAllAsync(LocationQueryObject query);
        // GET BY ID LOCATION
        Task<Location?> GetByIdAsync(int id);
        // LIST ALL CHILD LOCATIONS BASE ON PARENT LOCATION ID
        Task<List<Location>> GetChildsAsync(int parentId);
        // CREATE LOCATION
        Task<Location> CreateAsync(Location locationModel);
        // ADD CHILD TO PARENT LOCATION
        Task<Location?> AddChildToParentAsync(int parentId, Location locationModel);
        // UPDATE LOCATION
        Task<Location?> UpdateAsync(int id, Location locationModel);
        // DELETE LOCATION
        Task<Location?> DeleteAsync(int id);
    }
}