using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Location
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentLocationId { get; set; }
        public List<LocationDto> ChildLocations { get; set; } = new List<LocationDto>();
    }
}