using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Location;
using api.Models;

namespace api.Mappers
{
    public static class LocationMapper
    {
        public static LocationDto ToLocationDto(this Models.Location location)
        {
            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                ParentLocationId = location.ParentLocationId,
                ChildLocations = location.ChildLocations?.Select(ToLocationDto).ToList() ?? new List<LocationDto>()
            };
        }

        public static Location ToCommentFromCreate(this CreateLocationDto locationDto)
        {
            return new Location
            {
                Name = locationDto.Name
            };
        }

        public static Location ToLocationFromUpdate(this UpdateLocationDto locationDto)
        {
            return new Location
            {
                Name = locationDto.Name
            };
        }
    }
}